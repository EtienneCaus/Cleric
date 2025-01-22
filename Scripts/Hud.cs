using Godot;
using System;

public partial class Hud : SubViewportContainer
{
    public override void _Process(double delta)
	{
        GetNode<ProgressBar>("SubViewport/StaminaBar").Value = Globals.STAMINA;
    }
}
