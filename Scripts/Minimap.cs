using Godot;
using System;
using System.Collections.Generic;
using static Godot.Mathf;

public partial class Minimap : Node2D
{
    Sprite2D cursor;
    Player player;
    List<Vector2I> cells= new();    //list of unknown cells
    TileMapLayer tileMap_full =new();//Full tilemap
    int mapStatus=1;

    public override void _Ready()
	{
        cursor = GetNode<Sprite2D>("Cursor");
        player = GetTree().CurrentScene.GetNode<Player>("Player");
	}

    public override void _Process(double delta)
	{
        Vector2I myGridPosition = new Vector2I((int)Math.Floor(player.Position.X/Globals.GRID_SIZE), (int)Math.Floor(player.Position.Z/Globals.GRID_SIZE));
        cursor.Position = new Vector2(player.Position.X * 8 + 4, player.Position.Z * 8 + 4);
        cursor.Rotation = player.head.Rotation.Y * -1;  //Places and rotate the player's cursor
        foreach(Vector2I tile in cells)
        {                                   //If the cell is close enough and there's no cells there already...
            if(tile.DistanceTo(myGridPosition) <= 3) //&& GetNode<TileMapLayer>("TileMapLayer").GetCellAtlasCoords(tile) == new Vector2I(-1, -1))
            {
                if(tileMap_full.GetCellAtlasCoords(tile) == new Vector2I(1, 0))
                    GetNode<TileMapLayer>("TileMapLayer").SetCell(new Vector2I(tile.X, tile.Y), 0, new Vector2I(1,0));
                else if(tileMap_full.GetCellAtlasCoords(tile) == new Vector2I(2, 0))
                    GetNode<TileMapLayer>("TileMapLayer").SetCell(new Vector2I(tile.X, tile.Y), 0, new Vector2I(2,0));
                else
                    GetNode<TileMapLayer>("TileMapLayer").SetCell(new Vector2I(tile.X, tile.Y), 0, new Vector2I(0,0));
                //GD.Print(cells.Remove(tile));
            }
        }

        if(Input.IsActionJustPressed("map"))
        {
            switch(mapStatus)
            {
                case 0:
                    Visible = false;
                    mapStatus++;
                    break;
                case 1:
                    Visible = true;
                    GetNode<Camera2D>("Cursor/Camera2D").IgnoreRotation = false;
                    (FindParent("SubViewport") as SubViewport).Size = new Vector2I(256,256);
                    mapStatus++;
                    break;
                case 2:
                    Visible = true;
                    GetNode<Camera2D>("Cursor/Camera2D").IgnoreRotation = true;
                    (FindParent("SubViewport") as SubViewport).Size = DisplayServer.WindowGetSize();
                    mapStatus = 0;
                    break;
            }
        }
	}

    public void GenerateMap(TileMapLayer tileMap)
    {
        tileMap_full = tileMap;
        Godot.Collections.Array<Vector2I> usedTiles = tileMap.GetUsedCells();

        foreach(Vector2I tile in usedTiles)
        {
            cells.Add(tile);
            //    GetNode<TileMapLayer>("TileMapLayer").SetCell(new Vector2I(tile.X, tile.Y), 0, new Vector2I(0,0));    //Reveals the map
        }
    }
}
