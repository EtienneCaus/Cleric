using Godot;
using System;

public partial class Inventory : SubViewportContainer
{
    public override void _Ready()
    {
        for(int i = 0; i < Globals.INVENTORY.Length; ++i)
            Update(i);
    }
    public void Update(int position)
    {
        if(Globals.INVENTORY[position] == null)
            GetNode<Sprite2D>("SubViewport/Inv"+(position+1)+"/Sprite2D").Visible = false;
        else
        {
            GetNode<Sprite2D>("SubViewport/Inv"+(position+1)+"/Sprite2D").Visible = true;
            GetNode<Sprite2D>("SubViewport/Inv"+(position+1)+"/Sprite2D/Fluid").RegionRect = Globals.INVENTORY[position].region;
            GetNode<Sprite2D>("SubViewport/Inv"+(position+1)+"/Sprite2D/Fluid").Modulate = Globals.INVENTORY[position].color;
        }

        if(CheckEmpty(Globals.INVENTORY))
            Visible = false;
        else
            Visible = true;
    }

    private bool CheckEmpty<T>(T[] array)
    {
        for(int i = 0; i < array.Length; ++i)
            if(array[i] != null)
                return false;
        return true;
    }
}
