using Godot;
using System;

public partial class PauseMenu : Control
{
    static Random rnd = new Random();
    LineEdit seed, size, multiplier, corridors, hallways, rooms, rsMin, rsMax, espawn, esroom, torches, gold;
    Button center, cavern;
    public override void _Ready()
    {
        seed = GetNode<LineEdit>("PanelContainer/DungeonSettings/SeedLabel/Seed");
        seed.Text = Globals.SEED.ToString();
        size = GetNode<LineEdit>("PanelContainer/DungeonSettings/SizeLabel/Size");
        size.Text = Globals.STEPS.ToString();
        multiplier = GetNode<LineEdit>("PanelContainer/DungeonSettings/MultiplierLabel/Multiplier");
        multiplier.Text = Globals.WALKERS.ToString();
        corridors = GetNode<LineEdit>("PanelContainer/DungeonSettings/CorridorsLabel/Corridors");
        corridors.Text = Globals.CORRIDORS_LENGTH.ToString();
        hallways = GetNode<LineEdit>("PanelContainer/DungeonSettings/HallwaysLabel/Hallways");
        hallways.Text = Globals.HALLWAYS_CHANCES.ToString();
        rooms = GetNode<LineEdit>("PanelContainer/DungeonSettings/RoomsLabel/Rooms");
        rooms.Text = Globals.ROOMS_CHANCES.ToString();
        rsMin = GetNode<LineEdit>("PanelContainer/DungeonSettings/rsMinLabel/rsMin");
        rsMin.Text = Globals.ROOMS_SIZE_MIN.ToString();
        rsMax = GetNode<LineEdit>("PanelContainer/DungeonSettings/rsMaxLabel/rsMax");
        rsMax.Text = Globals.ROOMS_SIZE_MAX.ToString();
        center = GetNode<Button>("PanelContainer/DungeonSettings/CenterLabel/Center");
        center.Text = Globals.CENTER_ON.ToString();
        cavern = GetNode<Button>("PanelContainer/DungeonSettings/CavernLabel/Cavern");
        cavern.Text = Globals.CAVERN.ToString();
        espawn = GetNode<LineEdit>("PanelContainer/DungeonSettings/EnemySpawnLabel/EnemySpawn");
        espawn.Text = Globals.ENEMY_SPAWN.ToString();
        esroom = GetNode<LineEdit>("PanelContainer/DungeonSettings/EnemyRoomLabel/EnemyRoom");
        esroom.Text = Globals.ENEMY_ROOMS.ToString();
        torches = GetNode<LineEdit>("PanelContainer/DungeonSettings/TorchesLabel/Torches");
        torches.Text = Globals.TORCH_SPAWN.ToString();
        gold = GetNode<LineEdit>("PanelContainer/DungeonSettings/GoldLabel/Gold");
        gold.Text = Globals.GOLD_SPAWN.ToString();
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
        GetNode<VBoxContainer>("PanelContainer/MainMenu").Visible = true;
        GetNode<VBoxContainer>("PanelContainer/DungeonSettings").Visible = false;
        GetNode<VBoxContainer>("PanelContainer/Options").Visible = false;
        GetNode<Button>("PanelContainer/MainMenu/Continue").GrabFocus();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("escape") && !GetTree().Paused)
            pause();
        else if (Input.IsActionJustPressed("escape") && GetTree().Paused)
            resume();
    }
    public void _on_restart_level_pressed()
    {
        Random rnd = new Random();
        Globals.MANA = 100;
        Globals.STAMINA = 100;
        Globals.HEALTH = 100;
        resume();
        GetTree().ReloadCurrentScene(); //Recharge le jeu
    }
    public void _on_continue_pressed()
    {
        resume();
    }

    public void _on_start_pressed()
    {
        GetNode<VBoxContainer>("PanelContainer/MainMenu").Visible = false;
        GetNode<VBoxContainer>("PanelContainer/DungeonSettings").Visible = true;
        GetNode<VBoxContainer>("PanelContainer/Options").Visible = false;
        GetNode<Button>("PanelContainer/DungeonSettings/Restart").GrabFocus();
    }
    public void _on_options_pressed()
    {
        GetNode<VBoxContainer>("PanelContainer/MainMenu").Visible = false;
        GetNode<VBoxContainer>("PanelContainer/DungeonSettings").Visible = false;
        GetNode<VBoxContainer>("PanelContainer/Options").Visible = true;
        GetNode<Button>("PanelContainer/Options/Back").GrabFocus();
        GetNode<HSlider>("PanelContainer/Options/SensibilityLabel/Sensibility").Value = Globals.Sensitivity * 12000;
    }
    public void _on_back_pressed()
    {
        GetNode<VBoxContainer>("PanelContainer/MainMenu").Visible = true;
        GetNode<VBoxContainer>("PanelContainer/DungeonSettings").Visible = false;
        GetNode<VBoxContainer>("PanelContainer/Options").Visible = false;
        GetNode<Button>("PanelContainer/MainMenu/Start").GrabFocus();
    }

    public void _on_quit_pressed()
    {
        GetTree().Quit();
    }

    public void _on_sensibility_value_changed(float sensitivity)
    {
        Globals.Sensitivity = sensitivity / 12000;
    }
    
    public void _on_sound_value_changed(float sound)
    {
        Globals.Sound = sound;
        AudioServer.SetBusVolumeLinear(0, Globals.Sound);
    }
    //----------------------------------------------------------------------------------------------------------
    public void _on_restart_pressed()
    {
        Globals.LEVEL = 1;
        Globals.HEALTH = 100;
        Globals.STAMINA = 100;
        Globals.MANA = 100;
        Globals.GOLD = 0;
        resume();
        GetTree().ReloadCurrentScene(); //Recharge le jeu
    }

    public void _on_seed_text_changed(string str)
    {
        int.TryParse(str, out Globals.SEED);
    }

    public void _on_seed_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            rnd = new Random();
            Globals.SEED = rnd.Next();
            seed.Text = Globals.SEED.ToString();
        }
    }

    public void _on_size_text_changed(string str)
    {
        int.TryParse(str, out Globals.STEPS);
    }
    public void _on_size_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.STEPS += 10;
            size.Text = Globals.STEPS.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.STEPS -= 10;
            size.Text = Globals.STEPS.ToString();
        }

    }

    public void _on_multiplier_text_changed(string str)
    {
        int.TryParse(str, out Globals.WALKERS);
    }
    public void _on_multiplier_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.WALKERS++;
            multiplier.Text = Globals.WALKERS.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.WALKERS--;
            multiplier.Text = Globals.WALKERS.ToString();
        }
    }

    public void _on_corridors_text_changed(string str)
    {
        int.TryParse(str, out Globals.CORRIDORS_LENGTH);
    }
    public void _on_corridors_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.CORRIDORS_LENGTH++;
            corridors.Text = Globals.CORRIDORS_LENGTH.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.CORRIDORS_LENGTH--;
            corridors.Text = Globals.CORRIDORS_LENGTH.ToString();
        }
    }

    public void _on_hallways_text_changed(string str)
    {
        int.TryParse(str, out Globals.HALLWAYS_CHANCES);
    }
    public void _on_hallways_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.HALLWAYS_CHANCES += 5;
            if (Globals.HALLWAYS_CHANCES > 100)
                Globals.HALLWAYS_CHANCES = 100;
            hallways.Text = Globals.HALLWAYS_CHANCES.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.HALLWAYS_CHANCES -= 5;
            if (Globals.HALLWAYS_CHANCES < 0)
                Globals.HALLWAYS_CHANCES = 0;
            hallways.Text = Globals.HALLWAYS_CHANCES.ToString();
        }
    }

    public void _on_rooms_text_changed(string str)
    {
        int.TryParse(str, out Globals.ROOMS_CHANCES);
    }
    public void _on_rooms_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.ROOMS_CHANCES += 5;
            if (Globals.ROOMS_CHANCES > 100)
                Globals.ROOMS_CHANCES = 100;
            rooms.Text = Globals.ROOMS_CHANCES.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.ROOMS_CHANCES -= 5;
            if (Globals.ROOMS_CHANCES < 0)
                Globals.ROOMS_CHANCES = 0;
            rooms.Text = Globals.ROOMS_CHANCES.ToString();
        }
    }

    public void _on_rs_min_text_changed(string str)
    {
        int.TryParse(str, out Globals.ROOMS_SIZE_MIN);
    }
    public void _on_rs_min_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.ROOMS_SIZE_MIN++;
            rsMin.Text = Globals.ROOMS_SIZE_MIN.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.ROOMS_SIZE_MIN--;
            rsMin.Text = Globals.ROOMS_SIZE_MIN.ToString();
        }
    }

    public void _on_rs_max_text_changed(string str)
    {
        int.TryParse(str, out Globals.ROOMS_SIZE_MAX);
    }
    public void _on_rs_max_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.ROOMS_SIZE_MAX++;
            rsMax.Text = Globals.ROOMS_SIZE_MAX.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.ROOMS_SIZE_MAX--;
            rsMax.Text = Globals.ROOMS_SIZE_MAX.ToString();
        }
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

    public void _on_enemy_spawn_text_changed(string str)
    {
        int.TryParse(str, out Globals.ENEMY_SPAWN);
    }

    public void _on_enemy_spawn_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.ENEMY_SPAWN += 5;
            if (Globals.ENEMY_SPAWN > 100)
                Globals.ENEMY_SPAWN = 100;
            espawn.Text = Globals.ENEMY_SPAWN.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.ENEMY_SPAWN -= 5;
            if (Globals.ENEMY_SPAWN < 0)
                Globals.ENEMY_SPAWN = 0;
            espawn.Text = Globals.ENEMY_SPAWN.ToString();
        }
    }

    public void _on_enemy_room_text_changed(string str)
    {
        int.TryParse(str, out Globals.ENEMY_ROOMS);
    }

    public void _on_enemy_room_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.ENEMY_ROOMS += 5;
            if (Globals.ENEMY_ROOMS > 100)
                Globals.ENEMY_ROOMS = 100;
            esroom.Text = Globals.ENEMY_ROOMS.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.ENEMY_ROOMS -= 5;
            if (Globals.ENEMY_ROOMS < 0)
                Globals.ENEMY_ROOMS = 0;
            esroom.Text = Globals.ENEMY_ROOMS.ToString();
        }
    }

    public void _on_torches_text_changed(string str)
    {
        int.TryParse(str, out Globals.TORCH_SPAWN);
    }
    public void _on_torches_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.TORCH_SPAWN += 5;
            if (Globals.TORCH_SPAWN > 100)
                Globals.TORCH_SPAWN = 100;
            torches.Text = Globals.TORCH_SPAWN.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.TORCH_SPAWN -= 5;
            if (Globals.TORCH_SPAWN < 0)
                Globals.TORCH_SPAWN = 0;
            torches.Text = Globals.TORCH_SPAWN.ToString();
        }
    }

    public void _on_gold_text_changed(string str)
    {
        int.TryParse(str, out Globals.GOLD_SPAWN);
    }
    public void _on_gold_gui_input(InputEvent inputEvent)
    {
        if (Input.IsActionJustPressed("ui_right"))
        {
            Globals.GOLD_SPAWN += 5;
            if (Globals.GOLD_SPAWN > 100)
                Globals.GOLD_SPAWN = 100;
            gold.Text = Globals.GOLD_SPAWN.ToString();
        }
        else if (Input.IsActionJustPressed("ui_left"))
        {
            Globals.GOLD_SPAWN -= 5;
            if (Globals.GOLD_SPAWN < 0)
                Globals.GOLD_SPAWN = 0;
            gold.Text = Globals.GOLD_SPAWN.ToString();
        }
    }

    public void _on_next_pressed()
    {
        seed.GetParent<Control>().Visible = !seed.GetParent<Control>().Visible;
        size.GetParent<Control>().Visible = !size.GetParent<Control>().Visible;
        multiplier.GetParent<Control>().Visible = !multiplier.GetParent<Control>().Visible;
        corridors.GetParent<Control>().Visible = !corridors.GetParent<Control>().Visible;
        hallways.GetParent<Control>().Visible = !hallways.GetParent<Control>().Visible;
        rooms.GetParent<Control>().Visible = !rooms.GetParent<Control>().Visible;
        rsMin.GetParent<Control>().Visible = !rsMin.GetParent<Control>().Visible;
        rsMax.GetParent<Control>().Visible = !rsMax.GetParent<Control>().Visible;
        center.GetParent<Control>().Visible = !center.GetParent<Control>().Visible;
        cavern.GetParent<Control>().Visible = !cavern.GetParent<Control>().Visible;
        espawn.GetParent<Control>().Visible = !espawn.GetParent<Control>().Visible;
        esroom.GetParent<Control>().Visible = !esroom.GetParent<Control>().Visible;
        torches.GetParent<Control>().Visible = !torches.GetParent<Control>().Visible;
        gold.GetParent<Control>().Visible = !gold.GetParent<Control>().Visible;
    }
}
