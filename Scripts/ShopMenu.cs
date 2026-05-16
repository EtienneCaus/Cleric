using Godot;
using System;

public partial class ShopMenu : Control
{
    Button antidote, health, stamina, mana;
    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        GetTree().Paused = true;
        GetNode<Button>("PanelContainer/Shop/Continue").GrabFocus();
        SellAll();

        antidote = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/Antidote/Button");
        antidote.Text = (Globals.PRIX["antidote"] * Globals.LEVEL).ToString() + "   ";
        antidote.Pressed += () => _on_button_pressed("antidote");
        // The "() =>" structure acts as a bridge, passing your value directly into the method.
        health = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/HPotion/Button");
        health.Text = (Globals.PRIX["health"] * Globals.LEVEL).ToString() + "   ";
        health.Pressed += () => _on_button_pressed("health");
        stamina = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/SPotion/Button");
        stamina.Text = (Globals.PRIX["stamina"] * Globals.LEVEL).ToString() + "   ";
        stamina.Pressed += () => _on_button_pressed("stamina");
        mana = GetNode<Button>("PanelContainer/Shop/HBoxContainer/VBoxRight/MPotion/Button");
        mana.Text = (Globals.PRIX["mana"] * Globals.LEVEL).ToString() + "   ";
        mana.Pressed += () => _on_button_pressed("mana");
    }

    public void _on_button_pressed(string type)
    {
        for(int i = 0; i < Globals.INVENTORY.Length; ++i)
        {
            if(Globals.INVENTORY[i] == null && Globals.GOLD >= Globals.PRIX[type] * Globals.LEVEL)
            {
                Globals.INVENTORY[i] = new Potion(type);
                GetTree().CurrentScene.GetNode<Inventory>("%InvViewport").Update(i);
                Globals.GOLD -= Globals.PRIX[type] * Globals.LEVEL;
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
                Globals.GOLD += Globals.INVENTORY[i].GetPrice() * Globals.LEVEL;
            Globals.INVENTORY[i] = null;
            GetTree().CurrentScene.GetNode<Inventory>("%InvViewport").Update(i);
        }
    }
}
