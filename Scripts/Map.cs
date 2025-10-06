using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class Map : Node2D
{
    readonly Vector2I[] DIRECTIONS = {Vector2I.Right, Vector2I.Left, Vector2I.Up, Vector2I.Down};
    Vector2I position = Vector2I.Zero;
    Vector2I entrance, exit;
    Vector2I direction = Vector2I.Up;
    int stepsTurn = 0;
    Random rnd = new Random(Globals.SEED);//new Random(int seed);

    public override void _Ready()
    {
    }
    public TileMapLayer GetTileMap()
    {
        CreateMap(Globals.WALKERS, Globals.STEPS);
        return GetNode<TileMapLayer>("MapCreator");
    }

    public void CreateMap(int nbr, int steps)
    {
        entrance = position;    //Sets the entrance temporaraly at position 0,0
        
        while(nbr > 0)
        {
            if(Globals.CENTER_ON) //Puts the walker at the center of the dungeon each walker rotation
                position = Vector2I.Zero;
            direction = Vector2I.Up;
            int tmpSteps = steps;
            while(tmpSteps > 0)
            {
                if(GetNode<TileMapLayer>("MapCreator").GetCellAtlasCoords(position) == new Vector2I(-1, -1))    //If there's no tile...
                {
                    GetNode<TileMapLayer>("MapCreator").SetCell(position, 0, new Vector2I(0,0));    //Sets down a new tile
                    if(rnd.Next(100)<Globals.ENEMY_SPAWN)
                        GetNode<TileMapLayer>("MapCreator").SetCell(position, 0, new Vector2I(4,0));    //Set down an ennemy
                    else if(rnd.Next(100)<Globals.TORCH_SPAWN)
                        GetNode<TileMapLayer>("MapCreator").SetCell(position, 0, new Vector2I(5,0));    //Set down a torch
                    else if(rnd.Next(100)<Globals.GOLD_SPAWN)
                        GetNode<TileMapLayer>("MapCreator").SetCell(position, 0, new Vector2I(6,0));    //Set down gold
                    tmpSteps--;
                }
                
                position += direction;  //moves one tile
                stepsTurn++;
                if( (Globals.CAVERN && (rnd.Next(100) > Globals.HALLWAYS_CHANCES || stepsTurn > Globals.CORRIDORS_LENGTH)) || //If Cavern
                    (!Globals.CAVERN && rnd.Next(100) > Globals.HALLWAYS_CHANCES && stepsTurn > Globals.CORRIDORS_LENGTH))   //If Dungeon
                {
                    if(rnd.Next(100) < Globals.ROOMS_CHANCES)    //If it's a dungeon...
                        tmpSteps -= CreateRoom();   //Creates a room
                    stepsTurn = 0;
                    Vector2I tempdir;
                    do{
                        tempdir = DIRECTIONS[rnd.Next(0,4)];
                    }while(tempdir == direction);
                    direction = tempdir;
                }
            }
            nbr--;
        }
        exit = position;    //Temporarelly sets the exit down

        Godot.Collections.Array<Vector2I> cellList = GetNode<TileMapLayer>("MapCreator").GetUsedCells();

        while(cellList.Contains(entrance + Vector2I.Down)) //Puts the entrance next to wall
            entrance += Vector2I.Down;

        while(cellList.Contains(exit+Vector2I.Up)) //Puts the exit next to wall
            exit += Vector2I.Up;
        GetNode<TileMapLayer>("MapCreator").SetCell(entrance, 0, new Vector2I(1,0));    //Sets down the entrance
        GetNode<TileMapLayer>("MapCreator").SetCell(exit, 0, new Vector2I(2,0));    //Sets down the exit
    }

    public int CreateRoom()
    {
        Vector2I size = new Vector2I(rnd.Next(Globals.ROOMS_SIZE_MIN, Globals.ROOMS_SIZE_MAX), rnd.Next(Globals.ROOMS_SIZE_MIN, Globals.ROOMS_SIZE_MAX));
        Vector2I topLeftCorner = position - size/2;
        int steps =0;

        for(int y=0; y<=size.Y; y++)
            for(int x=0; x <= size.X; x++)
            {
                Vector2I newStep =  topLeftCorner + new Vector2I(x,y);
                if(GetNode<TileMapLayer>("MapCreator").GetCellAtlasCoords(newStep) == new Vector2I(-1, -1))
                {
                    GetNode<TileMapLayer>("MapCreator").SetCell(newStep, 0, new Vector2I(0,0));
                    if(rnd.Next(100)<Globals.ENEMY_ROOMS)
                        GetNode<TileMapLayer>("MapCreator").SetCell(newStep, 0, new Vector2I(4,0));    //Set down an ennemy
                    else if(rnd.Next(100)<Globals.TORCH_SPAWN)
                        GetNode<TileMapLayer>("MapCreator").SetCell(newStep, 0, new Vector2I(5,0));    //Set down a Torch
                    else if(rnd.Next(100)<Globals.GOLD_SPAWN)
                        GetNode<TileMapLayer>("MapCreator").SetCell(newStep, 0, new Vector2I(6,0));    //Set down Gold
                    steps++;
                }
            }
        return steps;
    }
}