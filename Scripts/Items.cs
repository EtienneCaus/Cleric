using Godot;
using System;

public partial class Items : Area3D
{
    public void On_body_entered(Node3D body)
    {
        Player player;
        if (GetNode<Node3D>("Gold").Visible && body.IsInGroup("Player"))
        {
            player = (Player)body;
            Random rnd = new();
            Globals.GOLD += 1 + (Globals.LEVEL * rnd.Next(10));
            player.GetGold();
            QueueFree();
        }
    }
}
