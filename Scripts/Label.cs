using Godot;
using System;

public partial class Label : Godot.Label
{
    public override void _Process(double delta)
	{
        Text = "FPS " + Engine.GetFramesPerSecond() + "\nLEVEL " + Globals.LEVEL + "\nGOLD " + Globals.GOLD;
    }
}
