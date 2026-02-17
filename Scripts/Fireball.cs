using Godot;
using System;

public partial class Fireball : Area3D
{
    private Vector3 direction, player;
    private float directionAngle;
    private bool destructible = false;
    private const float SPEED = 4;

    public override void _Process(double delta)
    {
        
        //Vector3 position = Position;
        //Vector3 velocity = Velocity;
       
        /*
        position.X = Mathf.Lerp(Position.X, direction.X, (float)delta * 3.0f);
        position.Z = Mathf.Lerp(Position.Z, direction.Z, (float)delta * 3.0f);
        */
        
        
        //direction = new Vector3(Mathf.Cos(directionAngle), 0, -Mathf.Sin(directionAngle));
        //direction = direction.Normalized();

        //Vector3 velocity = direction * 2 * (float)delta;
        //position += velocity;
        
        
        //direction = Vector3.Forward;
        //Velocity = direction * 2;

        //direction = Vector3.Forward.Rotated(Vector3.Up, directionAngle);
        //Vector3 velocity;
        //velocity = direction * 2;
        //Velocity = velocity;

        //player = GetNode<Player>("/root/World/Player").GlobalPosition;
        /*
        Vector3 directionToTarget = player - GlobalPosition;
        float angle3d = Mathf.RadToDeg(Vector3.Right.AngleTo(directionToTarget)); // Angle relative to the positive x-axis
        if(directionToTarget.Z < Position.Z)
            angle3d = 360 - angle3d;    
        //directionAngle = Position.AngleTo(player);
        //GD.Print(directionToTarget.Z + " | " + GlobalPosition.Z + " : " + angle3d);
        GD.Print(angle3d);
        angle3d = Mathf.DegToRad(angle3d);

        direction = new Vector3(Mathf.Sin(angle3d), 0, -Mathf.Cos(angle3d));
        //direction = direction.Normalized();
        Position += direction * 2 * (float)delta;
        */



        
        // Calculer l'angle initial vers le joueur (sur le plan XZ)
        //Vector3 directionToTarget = player - GlobalPosition;
        //float angle3d = Mathf.Atan2(directionToTarget.X, directionToTarget.Z);
        
        // Puis utilisez votre formule dans le Process :
        //direction = new Vector3(Mathf.Sin(angle3d), 0, Mathf.Cos(angle3d));
        Position += direction * SPEED * (float)delta;
        


        //velocity.X = direction.X * 2;
        //velocity.Z = direction.Z * 2;

        //Position = position;
        //Velocity = velocity;

        //MoveAndSlide();
    }

    public void setTarget(Vector3 target)
    {
        //directionAngle = Mathf.RadToDeg(GlobalPosition.Normalized().AngleTo(direction.Normalized()));
        //directionAngle = Position.SignedAngleTo(direction, Vector3.Up);
        //direction = target.GlobalPosition;
        //directionAngle = Vector3.Right.AngleTo(direction);
        //if(direction.Z > GlobalPosition.Z)
        //    directionAngle = 2*Mathf.Pi - directionAngle;
        //GD.Print(Mathf.RadToDeg(directionAngle));
        //GD.Print(Position.X + " , " + Position.Z);
        //GD.Print(directionAngle);
        //LookAt(direction);
        //LookAtFromPosition(GlobalPosition, direction);

        // Calculer l'angle initial vers le joueur (sur le plan XZ)
        Vector3 directionToTarget = target;// - GlobalPosition;
        float angle3d = Mathf.Atan2(directionToTarget.X, directionToTarget.Z);

        // Puis utilisez votre formule dans le Process :
        direction = new Vector3(Mathf.Sin(angle3d), 0, Mathf.Cos(angle3d));
    }

    public void On_body_entered(PhysicsBody3D body)
    {
        if(destructible)
        {
            if (body.IsInGroup("Player"))
            {
                ((Player)body).GetHit(30, "fire");
            }
            else if (body.IsInGroup("Enemy"))
            {
                ((Enemy)body).GetHit(30, "fire");
            }
            QueueFree();
        }
    }

    public void On_body_exited(PhysicsBody3D body)
    {
        if (body.IsInGroup("Enemy"))
        {
            destructible = true;
        }
    }
}
