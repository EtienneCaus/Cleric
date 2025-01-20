using Godot;
using System;
using static Godot.Mathf;

public partial class Player : CharacterBody3D
{
	float speed;
	public const float WalkSpeed = 3.0f;
	public const float SprintSpeed = 5.0f;
	public const float JumpVelocity = 2f;
	//public const float Sensitivity = 0.002f;

	//Bob variables
	public const float BobFrequency = 2.0f;
	public const float BobAmplitude = 0.04f;
	float timeBob = 0f;
	public Node3D head;
	AnimationPlayer anim;
	RayCast3D raycast, rayinteract;
	Camera3D camera, weaponCam;
	SpotLight3D spotLight;
	float damage;
	string type;

    public override void _Ready()
    {
		head = GetNode<Node3D>("Head");	//Set the head and camera
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		raycast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");
		rayinteract = GetNode<RayCast3D>("Head/Camera3D/RayInteract");

		camera = GetNode<Camera3D>("Head/Camera3D");
		weaponCam = GetNode<Camera3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D");
		GetNode<SubViewport>("Head/Camera3D/SubViewportContainer/SubViewport").Size = DisplayServer.WindowGetSize();
		spotLight = GetNode<SpotLight3D>("Head/SpotLight3D");
		Input.MouseMode = Input.MouseModeEnum.Captured; //Capture the mouse
        base._Ready();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
		if(@event is InputEventMouseMotion ev)
		{
			head.RotateY(-ev.Relative.X * Globals.Sensitivity);
			camera.RotateX(-ev.Relative.Y * Globals.Sensitivity);	//Bouge la caméra du joueur et empêche de dépasser le 90°
			camera.Rotation = new Vector3(Clamp(camera.Rotation.X, DegToRad(-90), DegToRad(90)), camera.Rotation.Y, camera.Rotation.Z);
			spotLight.Rotation = camera.Rotation;
		}
        base._UnhandledInput(@event);
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		//Handles Sprinting
		if(Input.IsActionPressed("sprint"))
			speed = SprintSpeed;
		else
			speed = WalkSpeed;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		Vector2 CameraDir = Input.GetVector("lookleft", "lookright", "lookup", "lookdown");
		Vector3 direction = (head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();


		//Moves the Camera
		head.RotateY(-CameraDir.X * Globals.Sensitivity * 30);
		camera.RotateX(-CameraDir.Y * Globals.Sensitivity * 10);	//Bouge la caméra du joueur et empêche de dépasser le 90°
		camera.Rotation = new Vector3(Clamp(camera.Rotation.X, DegToRad(-90), DegToRad(90)), camera.Rotation.Y, camera.Rotation.Z);
		spotLight.Rotation = camera.Rotation;

		//weaponCam.GlobalTransform = camera.GlobalTransform;		//Weapon camera
		GetNode<SubViewport>("Head/Camera3D/SubViewportContainer/SubViewport").Size = DisplayServer.WindowGetSize();

		if(IsOnFloor())
		{
			if (direction != Vector3.Zero)
			{
				velocity.X = direction.X * speed;
				velocity.Z = direction.Z * speed;
			}
			else
			{
				velocity.X = Lerp(velocity.X, direction.X * speed, (float)delta * 7.0f);	//Gradually stops the player
				velocity.Z = Lerp(velocity.Z, direction.Z * speed, (float)delta * 7.0f);	
			}
		}
		else
		{
			velocity.X = Lerp(velocity.X, direction.X * speed, (float)delta * 3.0f);	//Handles the movement in mid-air
			velocity.Z = Lerp(velocity.Z, direction.Z * speed, (float)delta * 3.0f);	//Makes it so that the player has less control in the air
		}
			
		//Head Bob
		timeBob += (float)delta * velocity.Length() * Convert.ToSingle(IsOnFloor());	//If the player is on floor; add bobbing relative to velocity

		Transform3D transBob = camera.Transform;
		transBob.Origin = HeadBob(timeBob);	//Perform Headbobs
		camera.Transform = transBob;	//Change the Origin of the Camera

		Node3D rightHand = GetNode<Node3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D/RightHand");
		Node3D leftHand = GetNode<Node3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D/LeftHand");
		//Right Hand
		Transform3D transWeapon = rightHand.Transform;
		transWeapon.Origin = WeaponBob(timeBob, true);
		rightHand.Transform = transWeapon;
		//Left Hand
		transWeapon = leftHand.Transform;
		transWeapon.Origin = WeaponBob(timeBob, false);
		leftHand.Transform = transWeapon;

		Velocity = velocity;
		MoveAndSlide();
		WeaponHit();

		if(Input.IsActionJustPressed("interact") && rayinteract.IsColliding())
		{
			Interact(rayinteract.GetCollider());
		}
	}

	Vector3 HeadBob(float time)
	{
		Vector3 position = Vector3.Zero;

		//Moves the head in a sine wave, ie. up and down
		position.Y = Sin(time * BobFrequency) * BobAmplitude;
		position.X = Cos(time / 2) * BobAmplitude;

		return position;
	}
	Vector3 WeaponBob(float time, bool mainHand)
	{
		Vector3 position = Vector3.Zero;

		//Moves the head in a sine wave, ie. up and down
		position.Y = Sin(time * BobFrequency) * (BobAmplitude / 2) - 0.2f;
		if(mainHand)	//if Right Hand
			position.X = Cos(time * BobFrequency / 2) * (BobAmplitude / 2) + 0.2f;
		else			//if Left Hand
			position.X = Cos(time * BobFrequency / 2) * (BobAmplitude / 2) - 0.2f;
		position.Z = -0.25f;

		return position;
	}


	public void SetPosition(Vector2I tile)
	{
		Vector3 position = Position;
		position.X = tile.X;
		position.Z = tile.Y;
		Position = position;
	}

	public void WeaponHit()
	{
		Enemy target;

		if(!anim.IsPlaying())
		{
			if(Input.IsActionPressed("fire"))
			{
				anim.Play("Mace_Hit");
				damage = 20;
				type = "blunt";
			}
			else if(Input.IsActionPressed("altfire"))
			{
				anim.Play("Alt_Weapon_Hit");
				damage = 10;
				type = "fire";
			}
		}
		//	anim.Stop();
		if(anim.IsPlaying() && raycast.IsColliding())
		{
			target = raycast.GetCollider() as Enemy;
			if(target != null && target.IsInGroup("Enemy"))
				target.GetHit(damage, type);
		}
	}

	public void Interact(GodotObject obj)
	{
		GD.Print(obj.GetType());
		if(obj.GetType().ToString() == "Cell")
		{
			((Cell)obj).Interact();
		}
	}
}
