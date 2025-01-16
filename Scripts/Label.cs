using Godot;
using System;

public partial class Label : Godot.Label
{
    public override void _Process(double delta)
	{
        Text = "LEVEL " + Globals.LEVEL + "\nFPS " + Engine.GetFramesPerSecond();
    }
}
