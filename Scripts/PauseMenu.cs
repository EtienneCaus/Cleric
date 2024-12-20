using Godot;
using System;

public partial class PauseMenu : Control
{
    static Random rnd = new Random();
    LineEdit seed, size, multiplier, corridors, hallways, rooms, rsMin, rsMax;
    Button center, cavern;
    public override void _Ready()
	{
        seed = GetNode<LineEdit>("PanelContainer/VBoxContainer/SeedLabel/Seed");
        seed.Text = Globals.SEED.ToString();
        size = GetNode<LineEdit>("PanelContainer/VBoxContainer/SizeLabel/Size");
        size.Text = Globals.STEPS.ToString();
        multiplier = GetNode<LineEdit>("PanelContainer/VBoxContainer/MultiplierLabel/Multiplier");
        multiplier.Text = Globals.WALKERS.ToString();
        corridors = GetNode<LineEdit>("PanelContainer/VBoxContainer/CorridorsLabel/Corridors");
        corridors.Text = Globals.CORRIDORS_LENGTH.ToString();
        hallways = GetNode<LineEdit>("PanelContainer/VBoxContainer/HallwaysLabel/Hallways");
        hallways.Text = Globals.HALLWAYS_CHANCES.ToString();
        rooms = GetNode<LineEdit>("PanelContainer/VBoxContainer/RoomsLabel/Rooms");
        rooms.Text = Globals.ROOMS_CHANCES.ToString();
        rsMin = GetNode<LineEdit>("PanelContainer/VBoxContainer/rsMinLabel/rsMin");
        rsMin.Text = Globals.ROOMS_SIZE_MIN.ToString();
        rsMax = GetNode<LineEdit>("PanelContainer/VBoxContainer/rsMaxLabel/rsMax");
        rsMax.Text = Globals.ROOMS_SIZE_MAX.ToString();
        center = GetNode<Button>("PanelContainer/VBoxContainer/CenterLabel/Center");
        center.Text = Globals.CENTER_ON.ToString();
        cavern = GetNode<Button>("PanelContainer/VBoxContainer/CavernLabel/Cavern");
        cavern.Text = Globals.CAVERN.ToString();

	}
    public void resume()
    {
        GetTree().Paused = false;
        Visible = false;
        Input.MouseMode = Input.MouseModeEnum.Captured; //Captures the mouse
    }

    public void pause()
    {
        GetTree().Paused = true;
        Visible = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("escape") && !GetTree().Paused)
            pause();
        else if(Input.IsActionJustPressed("escape") && GetTree().Paused)
            resume();
    }

    public void _on_quit_pressed()
    {
        GetTree().Quit();
    }

    public void _on_restart_pressed()
    {
        resume();
        GetTree().ReloadCurrentScene(); //Recharge le jeu
    }

    public void _on_seed_text_changed(string str)
    {
        int.TryParse(str, out Globals.SEED);
    }
    public void _on_seed_text_submitted(string str)
    {
        rnd = new Random();
        Globals.SEED = rnd.Next();
        seed.Text = Globals.SEED.ToString();
    }

    public void _on_size_text_changed(string str)
    {
        int.TryParse(str, out Globals.STEPS);
    }

    public void _on_multiplier_text_changed(string str)
    {
        int.TryParse(str, out Globals.WALKERS);
    }

    public void _on_corridors_text_changed(string str)
    {
        int.TryParse(str, out Globals.CORRIDORS_LENGTH);
    }

    public void _on_hallways_text_changed(string str)
    {
        int.TryParse(str, out Globals.HALLWAYS_CHANCES);
    }

    public void _on_rooms_text_changed(string str)
    {
        int.TryParse(str, out Globals.ROOMS_CHANCES);
    }

    public void _on_rs_min_text_changed(string str)
    {
        int.TryParse(str, out Globals.ROOMS_SIZE_MIN);
    }

    public void _on_rs_max_text_changed(string str)
    {
        int.TryParse(str, out Globals.ROOMS_SIZE_MAX);
    }

    public void _on_center_toggled(bool state)
    {
        Globals.CENTER_ON = state;
        center.Text = Globals.CENTER_ON.ToString();
    }

    public void _on_cavern_toggled(bool state)
    {
        Globals.CAVERN = state;
        cavern.Text = Globals.CAVERN.ToString();
    }
}
