using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelector : ItemSelector
{
    protected override int getStartSelectedItem()
    {
        int currentIndex = selectableItems.FindIndex(it => it.key == LocalizationManager.getLocale());
        if (currentIndex == -1)
        {
            Debug.LogError($"Language {LocalizationManager.getLocale()} was not defined in LanguageSelector");
            return 0;
        }

        return currentIndex;
    }

    public override void onSelected(string key)
    {
        LocalizationManager.setLocale(key);
    }
}
