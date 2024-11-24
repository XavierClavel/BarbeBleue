
using System.Collections.Generic;

public static class GameValueManager
{
    private static Dictionary<string, GameValueGroup> dict = new Dictionary<string, GameValueGroup>();
    private static Dictionary<string, Dictionary<string,BonusValue>> dictBonus = new Dictionary<string, Dictionary<string, BonusValue>>();

    public static GameValueGroup getValueGroup(string key)
    {
        return dict[key];
    }

    public static BonusValue getBonus(string key, string attribute)
    {
        return dictBonus[key][attribute];
    }

    public static void addAdditiveBonus(string key, string attribute, float value)
    {
        var dictKey = dictBonus.getorPut(key, new Dictionary<string, BonusValue>());
        BonusValue bonus = dictKey.getorPut(attribute, new BonusValue());
        bonus.addAdditiveBonus(value);
    }
    
    public static void addMultiplicativeBonus(string key, string attribute, float value)
    {
        var dictKey = dictBonus.getorPut(key, new Dictionary<string, BonusValue>());
        BonusValue bonus = dictKey.getorPut(attribute, new BonusValue());
        bonus.addMultiplicativeBonus(value);
    }

}
