using Godot;
using System;
using static Godot.Mathf;

public partial class Player : CharacterBody3D
{
	float speed;
	public const float WalkSpeed = 3.0f;
	public const float SprintSpeed = 5.0f;
	public const float JumpVelocity = 2f;
	public const float Sensitivity = 0.005f;

	//Bob variables
	public const float BobFrequency = 2.0f;
	public const float BobAmplitude = 0.04f;
	float timeBob = 0f;
	public Node3D head;
	Camera3D camera;
	SpotLight3D spotLight;

    public override void _Ready()
    {
		head = GetNode<Node3D>("Head");	//Set the head and camera
		camera = GetNode<Camera3D>("Head/Camera3D");
		spotLight = GetNode<SpotLight3D>("Head/SpotLight3D");
		Input.MouseMode = Input.MouseModeEnum.Captured; //Capture the mouse
        base._Ready();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
		if(@event is InputEventMouseMotion ev)
		{
			head.RotateY(-ev.Relative.X * Sensitivity);
			camera.RotateX(-ev.Relative.Y * Sensitivity);	//Bouge la caméra du joueur et empêche de dépasser le 90°
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
		Vector3 direction = (head.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

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

		Velocity = velocity;
		MoveAndSlide();
	}

	Vector3 HeadBob(float time)
	{
		Vector3 position = Vector3.Zero;

		//Moves the head in a sine wave, ie. up and down
		position.Y = Sin(time * BobFrequency) * BobAmplitude;
		position.X = Cos(time / 2) * BobAmplitude;

		return position;
	}

	public void SetPosition(Vector2I tile)
	{
		Vector3 position = Position;
		position.X = tile.X;
		position.Z = tile.Y;
		Position = position;
	}
}
