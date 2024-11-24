using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public static class LocalizationManager
{
    private static string selectedLanguage = "FR";

    public static string getLanguage() => selectedLanguage;

    public static void registerStringLocalizer(StringLocalizer s) => EventManagers.localization.registerListener(s);
    public static void unregisterStringLocalizer(StringLocalizer s) => EventManagers.localization.unregisterListener(s);

    public static void setLanguage(string value)
    {
        selectedLanguage = value;
        EventManagers.localization.dispatchEvent(it => it.onLocaleChange(selectedLanguage));
    }
    
    /**
     * Adds localized text to a text field using the given key
     */
    public static void LocalizeTextField(string key, TextMeshProUGUI field)
    {
        Debug.Log(key);
        if (key.Trim() == "") return;
        field.gameObject.AddComponent<StringLocalizer>().setKey(key);
    }

}
