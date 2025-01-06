using Godot;
using System;

public partial class Globals : Node
{
    static Random rnd = new Random();
    public const int GRID_SIZE = 1;

    public static int SEED = rnd.Next();

    public static int WALKERS = 4;  //Size multiplier
    public static int STEPS = 100;  //Numbers of tiles
    public static int CORRIDORS_LENGTH = 4; //Max lenght of corridors
    public static int HALLWAYS_CHANCES = 0;//Chances of generating long hallways
    public static int ROOMS_CHANCES = 100;   //Chances of generating a room
    public static int ROOMS_SIZE_MIN = 2;  //Minimum size of rooms
    public static int ROOMS_SIZE_MAX = 4;  //Maximum size of rooms
    public static bool CENTER_ON = false;   //Creates a center connection
    public static bool CAVERN = false;  //Set the map generation mode to Caverns
    public static int ENEMY_SPAWN = 20;
    public static int ENEMY_ROOMS = 20;
}
