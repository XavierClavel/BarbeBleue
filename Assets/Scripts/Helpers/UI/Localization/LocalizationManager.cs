using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public static class LocalizationManager
{
    private static string selectedLocale = "FR";

    public static string getLocale() => selectedLocale;

    public static void registerStringLocalizer(StringLocalizer s) => EventManagers.localization.registerListener(s);
    public static void unregisterStringLocalizer(StringLocalizer s) => EventManagers.localization.unregisterListener(s);

    public static void setLocale(string value)
    {
        selectedLocale = value;
        EventManagers.localization.dispatchEvent(it => it.onLocaleChange(selectedLocale));
        SaveManager.updateLocale(selectedLocale);
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
