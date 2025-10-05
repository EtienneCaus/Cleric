using Godot;
using System;

public partial class Hud : SubViewportContainer
{
    public override void _Process(double delta)
    {
        GetNode<ProgressBar>("SubViewport/HealthBar").Value = Globals.HEALTH;
        GetNode<ProgressBar>("SubViewport/StaminaBar").Value = Globals.STAMINA;
        GetNode<ProgressBar>("SubViewport/ManaBar").Value = Globals.MANA;
    }
}
