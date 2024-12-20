using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class World : Node3D
{
	PackedScene ResourceCell = ResourceLoader.Load<PackedScene>("res://Scenes/Cell.tscn");
	[Export] PackedScene Map;
	List<Cell> cells = new();
	Minimap minimap;
	public override void _Ready()
	{
		minimap = GetNode<Minimap>("UI/SubViewportContainer/SubViewport/Minimap");
		GenerateMap();
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("reload"))
		{
			Random rnd = new Random();
			Globals.SEED = rnd.Next();
			GetTree().ReloadCurrentScene();	//Recharge le jeu
		}
			
	}

	public void GenerateMap()
	{
		Map map = Map.Instantiate() as Map;
		TileMapLayer tileMap = map.GetTileMap();
		Godot.Collections.Array<Vector2I> usedTiles = tileMap.GetUsedCells();
		
		foreach(var tile in usedTiles)
		{
			Cell cell = ResourceCell.Instantiate() as Cell;
			AddChild(cell);
			cell.Position = new Vector3(tile.X*Globals.GRID_SIZE, 0, tile.Y*Globals.GRID_SIZE);
            cells.Add(cell);
			if(tileMap.GetCellAtlasCoords(tile) == new Vector2I(1, 0))	//Puts the player at the entrance
				GetNode<Player>("Player").SetPosition(tile);
		}
		
		foreach(Cell cell in cells)
		{
			cell.UpdateFaces(tileMap);
		}

		minimap.GenerateMap(tileMap);

		//map.Free();
	}
}
