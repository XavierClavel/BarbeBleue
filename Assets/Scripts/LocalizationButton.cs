using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LocalizationButton : MonoBehaviour, ILocalized
{
    [SerializeField] private string key;
    [SerializeField] private TextMeshProUGUI text;
    private Button button;
    private static Color unselectedColor = Color.white;
    private static Color selectedColor = new Color32(250,125,15,156);

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            LocalizationManager.setLocale(key);
        });
    }

    private void Start()
    {
        EventManagers.localization.registerListener(this);
        onLocaleChange(LocalizationManager.getLocale());
    }

    public void onLocaleChange(string locale)
    {
        text.color = locale == key ? selectedColor : unselectedColor;
    }
}
