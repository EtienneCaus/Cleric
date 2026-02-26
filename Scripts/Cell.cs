using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using static Godot.Mathf;

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
		Godot.Collections.Array<Vector2I> cellList = tileMap.GetUsedCells();    //Turns the tilemap into a list of used cells

		Vector2I myGridPosition = new Vector2I((int)Position.X / Globals.GRID_SIZE, (int)Position.Z / Globals.GRID_SIZE);
		if (cellList.Contains(myGridPosition + Vector2I.Right))
			eastFace.QueueFree();
		if (cellList.Contains(myGridPosition + Vector2I.Left))
			westFace.QueueFree();
		if (cellList.Contains(myGridPosition + Vector2I.Down))
			southFace.QueueFree();
		if (cellList.Contains(myGridPosition + Vector2I.Up))
			northFace.QueueFree();
		//topFace.QueueFree(); // -> FOR LARGE CAVERNS

		if (tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(1, 0))
		{
			southFace.GetChild<Wall>(0).SetWall(1); //southFace.Texture = entrance
			type = 1;
		}

		if (tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(2, 0))
		{
			northFace.GetChild<Wall>(0).SetWall(2); //southFace.Texture = exit
			type = 2;

			CharacterBody3D demon;
			if(Globals.LEVEL == 5)
			{
				demon = ResourceLoader.Load<PackedScene>("res://Scenes/Demon.tscn").Instantiate() as CharacterBody3D;
				demon.Position = new Vector3((GlobalPosition.X - Position.X) * Globals.GRID_SIZE, 0, (GlobalPosition.Y - Position.Y) * Globals.GRID_SIZE);
				AddChild(demon);
			}
		}

		if (tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(4, 0))   //Dummy
		{
			CharacterBody3D dummy;
			Random rnd = new Random();
			if(Globals.LEVEL > 0)
			{
				if (rnd.Next(0, Globals.LEVEL) < 1)
					dummy = ResourceLoader.Load<PackedScene>("res://Scenes/Dummy.tscn").Instantiate() as CharacterBody3D;
				else if(rnd.Next(1, 100) <= Globals.SPIDER_SPAWN && Globals.LEVEL > 2 ||
						Globals.LEVEL == 4 && Globals.SPIDER_SPAWN > 0)
					dummy = ResourceLoader.Load<PackedScene>("res://Scenes/Spider.tscn").Instantiate() as CharacterBody3D;
				else if(rnd.Next(1, 100) <= Globals.DEMON_SPAWN && Globals.LEVEL >= 6)
					dummy = ResourceLoader.Load<PackedScene>("res://Scenes/Demon.tscn").Instantiate() as CharacterBody3D;
				else
					dummy = ResourceLoader.Load<PackedScene>("res://Scenes/Skeleton.tscn").Instantiate() as CharacterBody3D;
				AddChild(dummy);
				dummy.Position = new Vector3((GlobalPosition.X - Position.X) * Globals.GRID_SIZE, 0, (GlobalPosition.Y - Position.Y) * Globals.GRID_SIZE);
			}
		}
		else if (tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(5, 0))  //Torch
		{
			Area3D walltorch = ResourceLoader.Load<PackedScene>("res://Scenes/Items.tscn").Instantiate() as Area3D;
			type = 3;

			if (!cellList.Contains(myGridPosition + Vector2I.Up))
			{
				northFace.GetChild<Wall>(0).SetWall(3); //southFace.Texture = Torch
				walltorch.GetChild<Node3D>(2).Position = new Vector3(0, 0.5f, -0.44f);
				walltorch.GetChild<Node3D>(2).Rotation = new Vector3(DegToRad(30), DegToRad(0), 0);
			}
			else if (!cellList.Contains(myGridPosition + Vector2I.Down))
			{
				southFace.GetChild<Wall>(0).SetWall(3);
				walltorch.GetChild<Node3D>(2).Position = new Vector3(0, 0.5f, 0.44f);
				walltorch.GetChild<Node3D>(2).Rotation = new Vector3(DegToRad(30), DegToRad(180), 0);
			}
			else if (!cellList.Contains(myGridPosition + Vector2I.Right))
			{
				eastFace.GetChild<Wall>(0).SetWall(3);
				walltorch.GetChild<Node3D>(2).Position = new Vector3(0.44f, 0.5f, 0);
				walltorch.GetChild<Node3D>(2).Rotation = new Vector3(DegToRad(30), DegToRad(270), 0);
			}
			else if (!cellList.Contains(myGridPosition + Vector2I.Left))
			{
				westFace.GetChild<Wall>(0).SetWall(3);
				walltorch.GetChild<Node3D>(2).Position = new Vector3(-0.44f, 0.5f, 0);
				walltorch.GetChild<Node3D>(2).Rotation = new Vector3(DegToRad(30), DegToRad(90), 0);
			}
			else
				type = 0;

			if (type == 3)
			{
				walltorch.Position = new Vector3((GlobalPosition.X - Position.X) * Globals.GRID_SIZE, 0, (GlobalPosition.Y - Position.Y) * Globals.GRID_SIZE);
				AddChild(walltorch);
			}
		}
		else if (tileMap.GetCellAtlasCoords(myGridPosition) == new Vector2I(6, 0))  //Gold
		{
			Area3D gold = ResourceLoader.Load<PackedScene>("res://Scenes/Items.tscn").Instantiate() as Area3D;
			gold.GetNode<Node3D>("Torch").Visible = false;
			gold.GetNode<Node3D>("Gold").Visible = true;
			AddChild(gold);
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
