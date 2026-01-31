using Godot;
using System;
using static Godot.Mathf;

public partial class Spider : Enemy
{
    RayCast3D raycast;
    Player target, theTarget;
    bool jumping, slashing = false;
    bool moving = true;
    float speed = 50f;
    public override void _Ready()
    {
        gotHit = GetNode<Timer>("GotHit");
        fireHit = GetNode<Timer>("FireHit");
        health = 50;
        flamability = 2;
        raycast = GetNode<RayCast3D>("RayCast3D");
    }
    public override void _Process(double delta)
    {
        raycast.TargetPosition = GetNode<Player>("/root/World/Player").GlobalPosition - GlobalPosition;

        if (raycast.IsColliding())
        {
            theTarget = raycast.GetCollider() as Player;
            if(theTarget != null && theTarget.IsInGroup("Player")
             && theTarget.GlobalPosition.DistanceTo(GlobalPosition) < 1.2)
                jumping = true;
        }  
    }

    public override void _PhysicsProcess(double delta)
	{
        Vector3 velocity = Velocity;
        
        if (jumping && Globals.HEALTH > 0 && moving)
        {
            if (theTarget != null && theTarget.IsInGroup("Player") && IsOnFloor())
            {
                velocity.X = Lerp(velocity.X, raycast.TargetPosition.X * speed, (float)delta * 4.0f);
                velocity.Z = Lerp(velocity.Z, raycast.TargetPosition.Z * speed, (float)delta * 4.0f);
                velocity.Y += 2.6f; 
                Jump();       
            }
        }
        else if (health > 0 && IsOnFloor() && !moving)   //Normal sprite
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(0, 0, 32, 32);
            velocity.X = 0;
            velocity.Z = 0;
        }
        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }
        Velocity = velocity;
        MoveAndSlide();

        if(theTarget != null && theTarget.IsInGroup("Player") && slashing && !IsOnFloor()
             && theTarget.GlobalPosition.DistanceTo(GlobalPosition) < 0.55)
        {
            if (isGettingBlocked && Globals.STAMINA >= 20)
            {
                Globals.STAMINA -= 20;
                GetNode<AudioStreamPlayer3D>("BlockPlayer3D").Play();
            }
            else if (health > 0 && target != null && target.IsInGroup("Player"))
            {
                target.GetHit(40, "poison");  //Hits the player
            }
            slashing = false;
        }

		if (onFire && fireHit.IsStopped() && health > 0)
		{
			health -= 10;
			if (health <= 0)
				Death();
			else
			{
				fireHit.Start();
				GetNode<AudioStreamPlayer3D>("FlamePlayer3D").Play();
				Random rnd = new();
				Tween tween = GetTree().CreateTween();
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(rnd.NextSingle() - 0.5f, -rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", new Vector2(-rnd.NextSingle() + 0.5f, rnd.NextSingle()), 0.02f);
				tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "offset", Vector2.Zero, 0.02f);
			}
		}
        isGettingBlocked = false;
	}

    public void On_area_3d_body_entered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            target = (Player)body;
        }
    }
    public void On_area_3d_body_exited(Node3D body)
    {
        if (body.IsInGroup("Player"))
            target = null;
    }
    
    async protected void Jump()     //Animation for jumping
    {
        slashing = true;
        moving = false;
        if (health > 0)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(32, 0, 32, 32);
            await ToSignal(GetTree().CreateTimer(1.2), "timeout");
        }
        moving = true;
        jumping = false;
        slashing = false;
    }
}
