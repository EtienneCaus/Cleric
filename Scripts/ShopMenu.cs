using Godot;
using System;

public partial class ShopMenu : Control
{
    Button antidote, health, stamina, mana, healing, fireball, light;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().Paused = true;
        GetNode<Button>("PanelContainer/Shop/Continue").GrabFocus();
        SellAll();

        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Torch/Label").Text =
            GetTree().CurrentScene.GetNode<Player>("Player").GetAltFireMode() == "Torch" ? "Equipped   " : "Owned   ";
        GetNode<Godot.Label>("PanelContainer/Shop/HBoxContainer/VBoxLeft/Shield/Label").Text =
            GetTree().CurrentScene.GetNode<Player>("Player").GetAltFireMode() == "ShieldBlock" ? "Equipped   " : "Owned   ";

        antidote = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/Antidote/Button");
        antidote.Text = (Globals.PRIX["antidote"] * ((Globals.LEVEL + 1) / 2)).ToString() + "   ";
        antidote.Pressed += () => _on_button_pressed("antidote");
        // The "() =>" structure acts as a bridge, passing your value directly into the method.
        health = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/HPotion/Button");
        health.Text = (Globals.PRIX["health"] * ((Globals.LEVEL + 1) / 2)).ToString() + "   ";
        health.Pressed += () => _on_button_pressed("health");
        stamina = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/SPotion/Button");
        stamina.Text = (Globals.PRIX["stamina"] * ((Globals.LEVEL + 1) / 2)).ToString() + "   ";
        stamina.Pressed += () => _on_button_pressed("stamina");
        mana = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/MPotion/Button");
        mana.Text = (Globals.PRIX["mana"] * ((Globals.LEVEL + 1) / 2)).ToString() + "   ";
        mana.Pressed += () => _on_button_pressed("mana");

        healing = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/HScroll/Button");
        healing.Text = (Globals.PRIX["scroll"] * ((Globals.LEVEL + 1) / 2)).ToString() + "   ";
        healing.Pressed += () => _on_button_pressed("scroll",1);
        fireball = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/FScroll/Button");
        fireball.Text = (Globals.PRIX["scroll"] * ((Globals.LEVEL + 1) / 2)).ToString() + "   ";
        fireball.Pressed += () => _on_button_pressed("scroll",2);
        light = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/LScroll/Button");
        light.Text = (Globals.PRIX["scroll"] * ((Globals.LEVEL + 1) / 2)).ToString() + "   ";
        light.Pressed += () => _on_button_pressed("scroll",3);
    }

    public void _on_button_pressed(string type, byte quantity = 5)
    {
        for(int i = 0; i < Globals.INVENTORY.Length; ++i)
        {
            if(Globals.INVENTORY[i] == null && Globals.GOLD >= Globals.PRIX[type] * ((Globals.LEVEL + 1) / 2))
            {
                Globals.INVENTORY[i] = new Potion(type, quantity);
                GetTree().CurrentScene.GetNode<Inventory>("%InvViewport").Update(i);
                Globals.GOLD -= Globals.PRIX[type] * ((Globals.LEVEL + 1) / 2);
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
