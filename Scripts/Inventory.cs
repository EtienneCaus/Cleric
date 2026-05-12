using Godot;
using System;

public partial class Inventory : SubViewportContainer
{
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
    }
}
