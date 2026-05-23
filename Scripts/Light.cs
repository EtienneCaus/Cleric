using Godot;
using System;

public partial class Light : Area3D
{
    Node3D direction;
    private double time;

    public override void _Process(double delta)
    {
        Vector3 position = Position;

        time += delta;
        float newY = 0.75f + (float)Math.Sin(time * 2f) * 0.1f;
       
        
        position.X = Mathf.Lerp(Position.X, direction.GlobalPosition.X, (float)delta * 3.0f);
        position.Y = Mathf.Lerp(Position.Y, newY, (float)delta * 3.0f);
        position.Z = Mathf.Lerp(Position.Z, direction.GlobalPosition.Z, (float)delta * 3.0f);
        
        Position = position;
    }

    public void setTarget(Node3D direction)
    {
        this.direction = direction;
    }

    public async void _on_timer_timeout()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(GetNode<Sprite3D>("Sprite3D"), "pixel_size", 0.001, 0.28f);
        await ToSignal(GetTree().CreateTimer(0.28), "timeout");
        QueueFree();
    }
}
