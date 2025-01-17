using Godot;
using System;

public partial class Cell : StaticBody3D
{
	Node3D topFace, northFace, eastFace, southFace, westFace, bottomFace;
	int type = 0;
	
	public override void _Ready()
	{
		topFace		= GetNode<Node3D>("Ceiling");
		northFace	= GetNode<Node3D>("North");
		eastFace	= GetNode<Node3D>("East");
		southFace	= GetNode<Node3D>("South");
		westFace	= GetNode<Node3D>("West");
		bottomFace	= GetNode<Node3D>("Floor");
	}

	public override void _Process(double delta)
	{
	}

	public void UpdateFaces(TileMapLayer tileMap)
	{
		Godot.Collections.Array<Vector2I> cellList = tileMap.GetUsedCells();	//Turns the tilemap into a list of used cells

		Vector2I myGridPosition = new Vector2I((int)Position.X/Globals.GRID_SIZE, (int)Position.Z/Globals.GRID_SIZE);
		if(cellList.Contains(myGridPosition + Vector2I.Right))
			eastFace.QueueFree();
		if(cellList.Contains(myGridPosition + Vector2I.Left))
			westFace.QueueFree();
		if(cellList.Contains(myGridPosition + Vector2I.Down))
			southFace.QueueFree();
		if(cellList.Contains(myGridPosition + Vector2I.Up))
			northFace.QueueFree();
		//topFace.QueueFree(); // -> FOR LARGE CAVERNS

		if(tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(1, 0))
		{
			southFace.GetChild<Wall>(0).SetWall(1); //southFace.Texture = entrance
			type = 1;
		}			

		if(tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(2, 0))
		{
			northFace.GetChild<Wall>(0).SetWall(2);	//southFace.Texture = exit
			type = 2;
		}

		if(tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(4, 0))	//Dummy
		{
			CharacterBody3D dummy = ResourceLoader.Load<PackedScene>("res://Scenes/Dummy.tscn").Instantiate() as CharacterBody3D;
			AddChild(dummy);
			dummy.Position = new Vector3((GlobalPosition.X - Position.X)*Globals.GRID_SIZE, 0, (GlobalPosition.Y - Position.Y)*Globals.GRID_SIZE);
		}
	}

	public void Interact()
	{
		if(type == 2)
		{
			Random rnd = new Random();
			Globals.SEED = rnd.Next();
			GetTree().ReloadCurrentScene();	//Recharge le jeu
			Globals.LEVEL++;
		}
	}
}
