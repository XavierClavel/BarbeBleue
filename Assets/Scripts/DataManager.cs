using UnityEngine;
using System.Collections.Generic;

public static class DataManager
{

    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
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
        initialized = true;
    }

    public static bool isInitialized() => initialized;

    private static void getSaveData()
    {
        SaveManager.Load();
    }

}

