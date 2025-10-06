using Godot;
using System;
using static Godot.Mathf;

public partial class Player : CharacterBody3D
{
	float speed;
	public const float WalkSpeed = 2.4f; //2.6f; //3.0f;
	public const float SprintSpeed = 4.0f; //5.0f;
	public const float JumpVelocity = 2f;
	//public const float Sensitivity = 0.002f;

	//Bob variables
	public const float BobFrequency = 2.0f;
	public const float BobAmplitude = 0.04f;
	float timeBob = 0f;
	public Node3D head;
	AnimationPlayer anim;
	RayCast3D raycast, rayinteract, rayblock;
	Camera3D camera, weaponCam;
	SpotLight3D spotLight;
	float damage;
	string type;

	string altFireMode = "Torch";
	bool isBlocking, isSprinting, isDead = false;
	protected Timer gotHit;

	public override void _Ready()
	{
		head = GetNode<Node3D>("Head"); //Set the head and camera
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		raycast = GetNode<RayCast3D>("Head/Camera3D/RayCast3D");
		rayinteract = GetNode<RayCast3D>("Head/Camera3D/RayInteract");
		rayblock = GetNode<RayCast3D>("Head/Camera3D/RayBlock");
		gotHit = GetNode<Timer>("GotHit");

		camera = GetNode<Camera3D>("Head/Camera3D");
		weaponCam = GetNode<Camera3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D");
		GetNode<SubViewport>("Head/Camera3D/SubViewportContainer/SubViewport").Size = DisplayServer.WindowGetSize();
		spotLight = GetNode<SpotLight3D>("Head/SpotLight3D");
		Input.MouseMode = Input.MouseModeEnum.Captured; //Capture the mouse
		base._Ready();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion ev)
		{
			head.RotateY(-ev.Relative.X * Globals.Sensitivity);
			camera.RotateX(-ev.Relative.Y * Globals.Sensitivity);   //Bouge la caméra du joueur et empêche de dépasser le 90°
			camera.Rotation = new Vector3(Clamp(camera.Rotation.X, DegToRad(-90), DegToRad(90)), camera.Rotation.Y, camera.Rotation.Z);
			spotLight.Rotation = camera.Rotation;
		}
		base._UnhandledInput(@event);
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if (!isDead)
		{

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

			if (Input.IsActionJustPressed("sprintToggle"))
			{
				if (isSprinting)
					isSprinting = false;
				else
					isSprinting = true;
			}

			//Handles Sprinting
			if (Input.IsActionPressed("sprint") || isSprinting)
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
			camera.RotateX(-CameraDir.Y * Globals.Sensitivity * 10);    //Bouge la caméra du joueur et empêche de dépasser le 90°
			camera.Rotation = new Vector3(Clamp(camera.Rotation.X, DegToRad(-90), DegToRad(90)), camera.Rotation.Y, camera.Rotation.Z);
			spotLight.Rotation = camera.Rotation;

			//weaponCam.GlobalTransform = camera.GlobalTransform;		//Weapon camera
			if (!isDead)
				GetNode<SubViewport>("Head/Camera3D/SubViewportContainer/SubViewport").Size = DisplayServer.WindowGetSize();

			if (IsOnFloor())
			{
				if (direction != Vector3.Zero)
				{
					velocity.X = direction.X * speed;
					velocity.Z = direction.Z * speed;
				}
				else
				{
					velocity.X = Lerp(velocity.X, direction.X * speed, (float)delta * 7.0f);    //Gradually stops the player
					velocity.Z = Lerp(velocity.Z, direction.Z * speed, (float)delta * 7.0f);
				}
			}
			else
			{
				velocity.X = Lerp(velocity.X, direction.X * speed, (float)delta * 3.0f);    //Handles the movement in mid-air
				velocity.Z = Lerp(velocity.Z, direction.Z * speed, (float)delta * 3.0f);    //Makes it so that the player has less control in the air
			}

			//Head Bob
			timeBob += (float)delta * velocity.Length() * Convert.ToSingle(IsOnFloor());    //If the player is on floor; add bobbing relative to velocity

			Transform3D transBob = camera.Transform;
			transBob.Origin = HeadBob(timeBob); //Perform Headbobs
			camera.Transform = transBob;    //Change the Origin of the Camera

			GetNode<Camera3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D").Rotation = head.Rotation;
			GetNode<Camera3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D").Position = head.Position;
			GetNode<Camera3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D").Transform = head.Transform;

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

			if (inputDir != Vector2.Zero && (Input.IsActionPressed("sprint") || isSprinting))
			{
				Globals.STAMINA--;
			}

			if (Globals.STAMINA > 0)
				WeaponHit();
			if (Globals.STAMINA < 100)
				Globals.STAMINA++;

			if (Globals.MANA < 100)
				Globals.MANA += 0.1;

			if (Input.IsActionJustPressed("interact") && rayinteract.IsColliding())
			{
				Interact(rayinteract.GetCollider());
			}

			if (Input.IsActionJustPressed("weapon"))
			{
				anim.Play("AltHandIn");
				if (altFireMode == "Torch")
				{
					GetNode<Node3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D/LeftHand/Torch").Visible = false;
					GetNode<Node3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D/LeftHand/Shield").Visible = true;
					altFireMode = "ShieldBlock";
					GetNode<OmniLight3D>("Head/OmniLight3D").LightEnergy = 0.1f;
				}
				else if (altFireMode == "ShieldBlock")
				{
					GetNode<Node3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D/LeftHand/Torch").Visible = true;
					GetNode<Node3D>("Head/Camera3D/SubViewportContainer/SubViewport/Camera3D/LeftHand/Shield").Visible = false;
					altFireMode = "Torch";
					GetNode<OmniLight3D>("Head/OmniLight3D").LightEnergy = 0.5f;
				}
			}
			if (Input.IsActionJustPressed("magic") && Globals.MANA >= 80 && Globals.HEALTH < 100)
			{
				Globals.MANA -= 80;
				Globals.HEALTH += 40;
				if (Globals.HEALTH > 100)
				{
					Globals.MANA += (Globals.HEALTH - 100) * 2;	//refunds the lost mana
					Globals.HEALTH = 100;
				}
			}
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
		if (mainHand)   //if Right Hand
			position.X = Cos(time * BobFrequency / 2) * (BobAmplitude / 2) + 0.2f;
		else            //if Left Hand
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

		if (!anim.IsPlaying())  //If no animation is playing
		{
			if (Input.IsActionPressed("fire") && Globals.STAMINA > 50)
			{
				Globals.STAMINA -= 30;
				anim.Play("Mace_Hit");
				damage = 20;
				type = "blunt";
			}
			else if (Input.IsActionPressed("altfire"))
			{
				if (altFireMode == "Torch" && Globals.STAMINA > 25)
				{
					Globals.STAMINA -= 15;
					anim.Play("Alt_Weapon_Hit");
					damage = 10;
					type = "fire";
				}
				else if (altFireMode == "ShieldBlock")
				{
					if (!isBlocking)
					{
						isBlocking = true;
						anim.Play("ShieldBlock_In");
					}
					else
					{
						anim.Play("ShieldBlock");
					}
				}
			}
		}
		else    //If animation playing
		{
			if (raycast.IsColliding())
			{
				target = raycast.GetCollider() as Enemy;
				if (target != null && target.IsInGroup("Enemy"))
				{
					if (type == "fire" && Globals.MANA < 20)
						type = "blunt";
					target.GetHit(damage, type);
				}
			}
		}
		if (altFireMode == "ShieldBlock" && isBlocking)
		{
			if (!Input.IsActionPressed("altfire"))
			{
				anim.Play("ShieldBlock_Out");
				isBlocking = false;
			}
			else if (rayblock.IsColliding())
			{
				target = rayblock.GetCollider() as Enemy;
				if (target != null && target.IsInGroup("Enemy"))
					target.GetBlocked();
			}
		}
	}

	public void Interact(GodotObject obj)
	{
		GD.Print(obj.GetType());
		if (obj.GetType().ToString() == "Cell")
		{
			((Cell)obj).Interact();
		}
	}
	public void GetHit(float force, String type)
	{
		if (gotHit.IsStopped())
		{
			GetNode<AudioStreamPlayer>("AudioStreamPlayer").Play();
			gotHit.Start();
			Globals.HEALTH -= (int)force;

			if (Globals.HEALTH <= 0)
			{
				GetNode<AudioStreamPlayer>("DeathPlayer").Play();
				//GetTree().ReloadCurrentScene(); //Recharge le jeu
				RemoveFromGroup("Player");
				GetNode<Node>("Head/Camera3D/SubViewportContainer").QueueFree();
				isDead = true;
				Globals.STAMINA = 0;
				Globals.MANA = 0;
			}
		}
	}
	public void GetGold()
	{
		GetNode<AudioStreamPlayer>("GoldPlayer").Play();
	}
}
