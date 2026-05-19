using Godot;
using System;

public partial class Hud : SubViewportContainer
{
    public override void _Process(double delta)
    {
        GetNode<ProgressBar>("SubViewport/HealthBar").Value = Globals.HEALTH;
        GetNode<ProgressBar>("SubViewport/HealthBar").MaxValue = Globals.MAX_HEALTH;
        GetNode<ProgressBar>("SubViewport/StaminaBar").Value = Globals.STAMINA;
        GetNode<ProgressBar>("SubViewport/StaminaBar").MaxValue = Globals.MAX_STAMINA;
        GetNode<ProgressBar>("SubViewport/ManaBar").Value = Globals.MANA;
        GetNode<ProgressBar>("SubViewport/ManaBar").MaxValue = Globals.MAX_MANA;

        switch(Globals.spellType)
        {
            case "healing" : 
                GetNode<Sprite2D>("SubViewport/Spell").RegionRect = new Rect2(16,0, new Vector2(16,16));
                break;
            case "fireball":
                GetNode<Sprite2D>("SubViewport/Spell").RegionRect = new Rect2(32,0, new Vector2(16,16));
                break;
            case "light":
                GetNode<Sprite2D>("SubViewport/Spell").RegionRect = new Rect2(48,0, new Vector2(16,16));              
                break;
            default:
                GetNode<Sprite2D>("SubViewport/Spell").RegionRect = new Rect2(0,0, new Vector2(16,16));
                break;
        }
    }
}
