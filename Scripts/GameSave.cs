using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class GameSave : RefCounted
{
    [Export] public int PLAYER_LEVEL {get; set;} = 1;
    [Export] public double MAX_HEALTH = 100;
    [Export] public double MAX_STAMINA = 100;
    [Export] public double MAX_MANA = 100;
    [Export] public int GOLD = 0;
    [Export] public string spellType = "healing";
    [Export] public Array<Dictionary> Inventory {get; set;} = new();
}

public partial class SaveManager
{
    private const string SavePath = "user://savegame.dat";

    public void SaveGame()
    {
        GameSave dataToSave = new GameSave
        {
            PLAYER_LEVEL = Globals.PLAYER_LEVEL,
            MAX_HEALTH = Globals.MAX_HEALTH,
            MAX_STAMINA = Globals.MAX_STAMINA,
            MAX_MANA = Globals.MAX_MANA,
            GOLD = Globals.GOLD,
            spellType = Globals.spellType
        };

        if(Globals.INVENTORY != null)       //Saves the inventory
        {
            foreach(Potion potion in Globals.INVENTORY)
            {
                if(potion == null) continue; //Ignores empty elements in the array

                Dictionary data = new()
                {
                    {"type", potion.GetPotionType()},
                    {"quantity", potion.quantity},
                    {"texture", potion.GetTexture()},
                    {"color", potion.color},
                    {"region", potion.region}
                };

                dataToSave.Inventory.Add(data);
            }
        }

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Write);
        if (file != null)
        {
            file.StoreVar(dataToSave, fullObjects: true); // Save everything
            GD.Print("Game Saved!");
        }
    }

    public void LoadGame()
    {
        if (FileAccess.FileExists(SavePath))
        {
            using FileAccess file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
            if (file != null)
            {
                GameSave data = (GameSave)file.GetVar(allowObjects : true).AsGodotObject();

                Globals.PLAYER_LEVEL = data.PLAYER_LEVEL;
                Globals.MAX_HEALTH = data.MAX_HEALTH;
                Globals.MAX_STAMINA = data.MAX_STAMINA;
                Globals.MAX_MANA = data.MAX_MANA;
                Globals.HEALTH = Globals.MAX_HEALTH;
                Globals.STAMINA = Globals.MAX_STAMINA;
                Globals.MANA = Globals.MAX_MANA;
                Globals.GOLD = data.GOLD;
                Globals.spellType = data.spellType;

                if(data.Inventory != null)       //Saves the inventory
                {
                    int index = 0;

                    foreach(Dictionary obj in data.Inventory)
                    {
                        //PotionData potion = (PotionData)obj;
                        if(obj == null) continue; //Ignores empty elements in the array

                        Potion newPotion = new Potion()
                        {
                            type = obj["type"].AsString(),
                            quantity = obj["quantity"].AsInt32(),
                            color = obj["color"].AsColor(),
                            region = obj["region"].AsRect2()
                        };
                        newPotion.SetTexture(obj["texture"].AsString());

                        Globals.INVENTORY[index] = newPotion;
                        index++;
                    }
                }
                GD.Print("Game Loaded!");
            }
        } else GD.Print("Couldn't find save file!");
    }
/*
    public GameSave LoadGame()
    {
        if (!FileAccess.FileExists(SavePath)) return new GameSave(); // Retourne les valeurs par défaut

        using var file = FileAccess.Open(SavePath, FileAccess.ModeFlags.Read);
        if (file != null)
        {
            var data = file.GetVar();
            return (GameSave)data.AsGodotObject();
        }
        return new GameSave();
    }
*/
}