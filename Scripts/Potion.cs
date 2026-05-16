using Godot;
using System;

public partial class Potion : RigidBody3D
{
    string type = null;
    int quantity = 0;
    public Color color = new Color(0xFFFFFFFF);
    public Rect2 region = new Rect2(0,0, new Vector2(16,16));

    public override void _Ready()
    {
        GetNode<Sprite3D>("Sprite3D/Fluid").RegionRect = region;
        GetNode<Sprite3D>("Sprite3D/Fluid").Modulate = color;
    }
    public Potion(){}
    public Potion(string type = null, byte quantity = 5)
    {
        this.type = type;
        this.quantity = quantity;

        switch (type)
        {
            case "health":
                color = new Color(0xFF0000FF);
                break;
            case "stamina":
                color = new Color(0x00FF00FF);
                break;
            case "mana":
                color = new Color(0x0000FFFF);
                break;
            case "antidote":
                color = new Color(0x000000FF);
                break;
            default:
                color = new Color(0xFFFFFFFF);
                break;
        }
        region.Position = new Vector2(16*quantity, 0);
    }
    public Potion(Potion potion)
    {
        type = potion.type;
        quantity = potion.quantity;

        color = potion.color;
        region = potion.region;
    }

    public string GetPotionType()
    {
        return type;
    }

    public bool Drink()
    {
        --quantity;
        region.Position -= new Vector2(16, 0);

        if(quantity <= 0)
            return true;
        else
            return false;
    }

    public int GetPrice()
    {
        int price = Globals.PRIX[type];
        price *= quantity / 5;
        return price;
    }
}
