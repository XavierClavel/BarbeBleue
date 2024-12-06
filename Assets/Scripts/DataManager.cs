using UnityEngine;
using System.Collections.Generic;
using TMPro;

public static class DataManager
{

    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<fontKey, TMP_FontAsset> dictKeyToFont = new Dictionary<fontKey, TMP_FontAsset>();
    private static bool initialized = false;

    
    public static void LoadData()
    {
        if (initialized) return;
        
        getSaveData();
        LocalizedStringBuilder localizedStringBuilder = new LocalizedStringBuilder();
        foreach (TextAsset data in Resources.LoadAll<TextAsset>("Localization/"))
        {
            localizedStringBuilder.loadText(data, ref dictLocalization, $"Localization/{data.name}");
        }

        foreach (var data in Resources.LoadAll<FontGroup>("FontGroups/"))
        {
            dictKeyToFont[data.getKey()] = data.getFont();
        }
        initialized = true;
    }

    public static bool isInitialized() => initialized;

    private static void getSaveData()
    {
        SaveManager.load();
    }

}

