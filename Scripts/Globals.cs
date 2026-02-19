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
    public static int TORCH_SPAWN = 10; //Set the spawn rate of torches
    public static int GOLD_SPAWN = 5;   //Set the spawn rate of gold
    public static int ENEMY_SPAWN = 5; //Enemy spawn rate in corridors/caverns
    public static int ENEMY_ROOMS = 10; //Enemy spawn rate in rooms

    public static int SPIDER_SPAWN = 30; //% of ennemies spawning as spiders
    public static int DEMON_SPAWN = 20; //% of ennemies spawning as spiders

    public static int LEVEL = 1; //Starting level

    //--------------------------------------------------------------------------------

    public static float Sensitivity = 0.002f;
    public static float Sound = 1f;
    public static byte showtext = 0;

    //--------------------------------------------------------------------------------

    public static double HEALTH = 100;
    public static double STAMINA = 100;
    public static double MANA = 100;
    public static int GOLD = 0;
}
