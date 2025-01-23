using Godot;
using System;

public partial class Wall : MeshInstance3D
{
    public void SetWall(int wall)
    {
        if(wall == 1)
        {
            Material material = ResourceLoader.Load<Material>("res://Materials/StoneEntrance.tres");
            SetSurfaceOverrideMaterial(0, material);
        }
        if(wall == 2)
        {
            Material material = ResourceLoader.Load<Material>("res://Materials/StoneExit.tres");
            SetSurfaceOverrideMaterial(0, material);
        }
        if(wall == 3)
        {
            Material material = ResourceLoader.Load<Material>("res://Materials/StoneTorch.tres");
            SetSurfaceOverrideMaterial(0, material);
        }
    }
}
