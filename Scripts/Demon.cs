using Godot;
using System;
using static Godot.Mathf;

public partial class Demon : Enemy
{
    RayCast3D raycast;
    Player target, theTarget, tempTarget;
    bool slashing, walking, walkAnim = false;
    bool fireball = true;
    float speed = 0.8f;
    public override void _Ready()
    {
        gotHit = GetNode<Timer>("GotHit");
        fireHit = GetNode<Timer>("FireHit");
        health = 200;
        flamability = 6;
        raycast = GetNode<RayCast3D>("RayCast3D");
    }
    public override void _Process(double delta)
    {
        Vector3 velocity = Velocity;
        raycast.TargetPosition = GetNode<Player>("/root/World/Player").GlobalPosition - GlobalPosition;

        if (raycast.IsColliding())
        {
            target = raycast.GetCollider() as Player;
            if (target != null && target.IsInGroup("Player"))
            {
                walking = true;
                if(fireball && target.GlobalPosition.DistanceTo(GlobalPosition) > 3)
                    Fire();    
            }
                
        }

        if (target != null && target.GlobalPosition.DistanceTo(GlobalPosition) < 0.55f
         && slashing == false && Globals.HEALTH > 0)
            Slash();


        if (walking && !slashing  && Globals.HEALTH > 0)
        {
            if (target != null && target.IsInGroup("Player"))
            {
                velocity.X = Lerp(velocity.X, raycast.TargetPosition.X * speed, (float)delta * 3.0f);
                velocity.Z = Lerp(velocity.Z, raycast.TargetPosition.Z * speed, (float)delta * 3.0f);
                //velocity = Position + raycast.TargetPosition.AngleTo(Position) * speed;
                Velocity = velocity;
                MoveAndSlide();
            }
            if (!walkAnim)
                Walk();
        }
        else if (!walking && health > 0 && !slashing)   //Normal sprite
        {  
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(0, 64, 32, 64);
        }
    }

/*
    public void On_area_3d_body_entered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            tempTarget = (Player)body;
        }
    }
    public void On_area_3d_body_exited(Node3D body)
    {
        if (body.IsInGroup("Player"))
            tempTarget = null;
    }
    */

    async protected void Slash()    //Skeleton basic attack
    {
        isGettingBlocked = false;
        slashing = true;
        if (health > 0)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(64, 0, 32, 64);
            await ToSignal(GetTree().CreateTimer(1 - speed), "timeout");
        }
        if (health > 0)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(64, 64, 32, 64);
            if (isGettingBlocked && Globals.STAMINA >= 40)
            {
                Globals.STAMINA -= 40;
                GetNode<AudioStreamPlayer3D>("BlockPlayer3D").Play();
            }
            else if (health > 0 && target != null && target.IsInGroup("Player")
                && target.GlobalPosition.DistanceTo(GlobalPosition) < 0.55f)
            {
                target.GetHit(40, "fire");  //Hits the player
            }
            await ToSignal(GetTree().CreateTimer(1 - speed), "timeout");
        }
        if (health > 0)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(0, 64, 32, 64);
            await ToSignal(GetTree().CreateTimer(1 - speed), "timeout");
        }
        slashing = false;
    }
    async protected void Walk()     //Animation for walking
    {
        walkAnim = true;
        if (health > 0 && !slashing)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(32, 64, 32, 64);
            await ToSignal(GetTree().CreateTimer(0.2), "timeout");
        }

        if (health > 0 && !slashing)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(32, 0, 32, 64);
            await ToSignal(GetTree().CreateTimer(0.2), "timeout");
        }
        walkAnim = false;
        walking = false;
    }

    async protected void Fire()    //Demon basic attack
    {
        walking = false;
        fireball = false;
        slashing = true;
        if (health > 0)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(0, 0, 32, 64);
            await ToSignal(GetTree().CreateTimer(1 - speed), "timeout");
        }
        if (health > 0 && target != null)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(64, 64, 32, 64);

            PackedScene fire = ResourceLoader.Load("res://Scenes/Fireball.tscn") as PackedScene;	//Create Fireball
            Fireball fireTemp = fire.Instantiate() as Fireball;
            fireTemp.Position = new Vector3(GlobalPosition.X, 0.3f, GlobalPosition.Z);
            fireTemp.setTarget(target.GlobalPosition - GlobalPosition);
            GetTree().CurrentScene.AddChild(fireTemp);
            await ToSignal(GetTree().CreateTimer(1 - speed), "timeout");
        }
        if (health > 0)
        {
            GetNode<Sprite3D>("Sprite3D").RegionRect = new Rect2(0, 64, 32, 64);
            await ToSignal(GetTree().CreateTimer(1 - speed), "timeout");
        }
        slashing = false;
        await ToSignal(GetTree().CreateTimer(5), "timeout");
        fireball = true;
    }
}
