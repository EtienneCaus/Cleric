using Godot;
using System;

public partial class Label : Godot.Label
{
    public override void _Ready()
    {
        Visible = true;
    }
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("function"))  //Toggles showtext
        {
            Globals.showtext++;
            if (Globals.showtext > 7)
                Globals.showtext = 0;
        }

        switch (Globals.showtext)
        {
            case 0:
                Text = "";
                break;
            case 1:
                Text = "COINS " + Globals.GOLD;
                break;
            case 2:
                Text = "LEVEL " + Globals.LEVEL + "\nCOINS " + Globals.GOLD;
                break;
            case 3:
                Text = "FPS " + Engine.GetFramesPerSecond() + "\nLEVEL " + Globals.LEVEL + "\nCOINS " + Globals.GOLD;
                break;
            case 4:
                Text = "FPS " + Engine.GetFramesPerSecond() + "\nLEVEL " + Globals.LEVEL;
                break;
            case 5:
                Text = "FPS " + Engine.GetFramesPerSecond() + "\nCOINS " + Globals.GOLD;
                break;
            case 6:
                Text = "FPS " + Engine.GetFramesPerSecond();
                break;
            case 7:
                Text = "LEVEL " + Globals.LEVEL;
                break;

/*          case 0:
                Text = "";
                break;
            case 1:
                Text = "COINS " + Globals.GOLD;
                break;
            case 2:
                Text = "\nCOINS " + Globals.GOLD;
                break;
            case 3:
                Text = "LEVEL " + Globals.LEVEL + "\nCOINS " + Globals.GOLD;
                break;
            case 4:
                Text = "\nLEVEL " + Globals.LEVEL + "\nCOINS " + Globals.GOLD;
                break;
            case 5:
                Text = "FPS " + Engine.GetFramesPerSecond() + "\nLEVEL " + Globals.LEVEL + "\nCOINS " + Globals.GOLD;
                break;
            case 6:
                Text = "FPS " + Engine.GetFramesPerSecond() + "\n\nCOINS " + Globals.GOLD;
                break;
            case 7:
                Text = "FPS " + Engine.GetFramesPerSecond() + "\nCOINS " + Globals.GOLD;
                break;
            case 8:
                Text = "FPS " + Engine.GetFramesPerSecond() + "\nLEVEL " + Globals.LEVEL;
                break;
            case 9:
                Text = "FPS " + Engine.GetFramesPerSecond();
                break;
            case 10:
                Text = "LEVEL " + Globals.LEVEL;
                break;
*/      }
    }
}
