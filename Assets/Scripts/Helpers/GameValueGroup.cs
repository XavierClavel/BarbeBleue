
using System.Collections.Generic;
using UnityEngine;

public class GameValueGroup
{
    private List<string> groupFlags = new List<string>();
    private Dictionary<string, GameValueInt> dictInt = new Dictionary<string, GameValueInt>
    {
        {"MaxHealth", new GameValueInt()},
        {"MaxShields", new GameValueInt()},
        {"Projectiles", new GameValueInt()},
        {"MaxBlue", new GameValueInt()},
        {"MaxOrange", new GameValueInt()},
        {"MaxGreen", new GameValueInt()},
        {"Pierce", new GameValueInt()},
        {"Magazine", new GameValueInt()},
        {"Knockback", new GameValueInt()},
    };
    private Dictionary<string, GameValueFloat> dictFloat = new Dictionary<string, GameValueFloat>
    {
        {"Damage", new GameValueFloat()},
        {"Speed", new GameValueFloat()},
        {"Range", new GameValueFloat()},
        {"Spread", new GameValueFloat()},
        {"Cooldown", new GameValueFloat()},
        {"Cooldown", new GameValueFloat()},
    };
    private Dictionary<string, GameValueVector2Int> dictV2Int = new Dictionary<string, GameValueVector2Int>
    {
        
    };
    private Dictionary<string, GameValueVector2> dictV2 = new Dictionary<string, GameValueVector2>
    {
        
    };
    private Dictionary<string, GameValueFlags> dictFlags = new Dictionary<string, GameValueFlags>
    {
        {"Flags", new GameValueFlags()},
    };
    private Dictionary<string,IGameValue> dictAll = new Dictionary<string,IGameValue>();
    
    public void applyOperation(string key, string value)
    {
        dictAll[key].applyOperation(value);
    }

    public int getValueInt(string key)
    {
        List<BonusValue> bonuses = groupFlags.map(it => GameValueManager.getBonus(it, key));
        
        return dictInt[key].getValue();
    }

    public float getValueFloat(string key)
    {
        return dictFloat[key].getValue();
    }

    public Vector2Int getV2Int(string key)
    {
        return dictV2Int[key].getValue();
    }

    public Vector2 getV2(string key)
    {
        return dictV2[key].getValue();
    }

    public List<string> getFlags(string key)
    {
        return dictFlags[key].getValue();
    }

    public bool hasFlag(string key, string flag)
    {
        return getFlags(key).Contains(flag);
    }
}
