using Godot;
using System;

public partial class ShopMenu : Control
{
    Button antidote, health, stamina, mana, healing, fireball, light, maxHealth, maxStamina, maxMana;
    public override void _Ready()
    {
        Globals.HEALTH = Globals.MAX_HEALTH;
        Globals.STAMINA = Globals.MAX_STAMINA;
        Globals.MANA = Globals.MAX_MANA;

        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().Paused = true;
        GetNode<Button>("PanelContainer/Shop/Continue").GrabFocus();
        SellAll();

        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Class/Label").Text = 
                "Level "+ Globals.PLAYER_LEVEL.ToString() + " Cleric   ";
        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Torch/Label").Text =
            GetTree().CurrentScene.GetNode<Player>("Player").GetAltFireMode() == "Torch" ? "Equipped   " : "Owned   ";
        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Shield/Label").Text =
            GetTree().CurrentScene.GetNode<Player>("Player").GetAltFireMode() == "ShieldBlock" ? "Equipped   " : "Owned   ";

        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Health/Label").Text = Globals.MAX_HEALTH + "/          ";
        maxHealth = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Health/Button");
        maxHealth.Text = Globals.MAX_HEALTH.ToString();
        maxHealth.Pressed += () => _left_button_pressed(ref Globals.MAX_HEALTH, ref maxHealth);
        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Stamina/Label").Text = (int)Globals.MAX_STAMINA + "/          ";
        maxStamina = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Stamina/Button");
        maxStamina.Text = Globals.MAX_STAMINA.ToString();
        maxStamina.Pressed += () => _left_button_pressed(ref Globals.MAX_STAMINA, ref maxStamina);
        maxMana = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Mana/Button");
        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Mana/Label").Text = (int)Globals.MAX_MANA + "/          ";
        maxMana.Text = Globals.MAX_MANA.ToString();
        maxMana.Pressed += () => _left_button_pressed(ref Globals.MAX_MANA, ref maxMana);

        antidote = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/Antidote/Button");
        antidote.Text = (Globals.PRIX["antidote"] * (Globals.LEVEL / 2)).ToString() + "   ";
        antidote.Pressed += () => _on_button_pressed("antidote");
        // The "() =>" structure acts as a bridge, passing your value directly into the method.
        health = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/HPotion/Button");
        health.Text = (Globals.PRIX["health"] * (Globals.LEVEL / 2)).ToString() + "   ";
        health.Pressed += () => _on_button_pressed("health");
        stamina = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/SPotion/Button");
        stamina.Text = (Globals.PRIX["stamina"] * (Globals.LEVEL / 2)).ToString() + "   ";
        stamina.Pressed += () => _on_button_pressed("stamina");
        mana = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/MPotion/Button");
        mana.Text = (Globals.PRIX["mana"] * (Globals.LEVEL / 2)).ToString() + "   ";
        mana.Pressed += () => _on_button_pressed("mana");

        healing = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/HScroll/Button");
        healing.Text = (Globals.PRIX["scroll"] * (Globals.LEVEL / 2)).ToString() + "   ";
        healing.Pressed += () => _on_button_pressed("scroll",1);
        fireball = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/FScroll/Button");
        fireball.Text = (Globals.PRIX["scroll"] * (Globals.LEVEL / 2)).ToString() + "   ";
        fireball.Pressed += () => _on_button_pressed("scroll",2);
        light = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/LScroll/Button");
        light.Text = (Globals.PRIX["scroll"] * (Globals.LEVEL / 2)).ToString() + "   ";
        light.Pressed += () => _on_button_pressed("scroll",3);
    }

    public void _left_button_pressed(ref double value, ref Button button)
    {
        if(Globals.GOLD >= value)
        {
            Globals.GOLD -= (int)value;
            value += 10;
            button.Text = value.ToString();
            ++Globals.PLAYER_LEVEL;
            GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Class/Label").Text = 
                "Level "+ Globals.PLAYER_LEVEL.ToString() + " Cleric   ";
        }
    }

    public void _on_button_pressed(string type, byte quantity = 5)
    {
        for(int i = 0; i < Globals.INVENTORY.Length; ++i)
        {
            if(Globals.INVENTORY[i] == null && Globals.GOLD >= Globals.PRIX[type] * (Globals.LEVEL / 2))
            {
                Globals.INVENTORY[i] = new Potion(type, quantity);
                GetTree().CurrentScene.GetNode<Inventory>("%InvViewport").Update(i);
                Globals.GOLD -= Globals.PRIX[type] * (Globals.LEVEL / 2);
                break;
            }
        }
    }

    public void _on_continue_pressed()
    {
        GetTree().Paused = false;
        Random rnd = new Random();
        Globals.SEED = rnd.Next();
        GetTree().ReloadCurrentScene();	//Recharge le jeu
        Globals.LEVEL++;

        SaveManager saveManager = new SaveManager();
		saveManager.SaveGame();
    }

    private void SellAll()
    {
        for(int i = 0; i < Globals.INVENTORY.Length; ++i)
        {
            if(Globals.INVENTORY[i] != null)
                Globals.GOLD += Globals.INVENTORY[i].GetPrice() * (Globals.LEVEL / 2);
            Globals.INVENTORY[i] = null;
            GetTree().CurrentScene.GetNode<Inventory>("%InvViewport").Update(i);
        }
    }
}
