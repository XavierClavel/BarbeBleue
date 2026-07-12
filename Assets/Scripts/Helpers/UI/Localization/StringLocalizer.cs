using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StringLocalizer : MonoBehaviour, ILocalized
{
    [SerializeField] string key = null;
    [SerializeField] private fontKey keyFont = fontKey.DEFAULT;
    [Header("Optional")]
    [Tooltip("When enabled, uses the size below instead of the default size for the selected font key.")]
    [SerializeField] private bool overrideFontSize = false;
    [SerializeField] private float fontSizeOverride = 36f;
    private TextMeshProUGUI textDisplay = null;
    private LocalizedString localizedString;

    public void setKey(string key)
    {
        this.key = key;
        Setup();
    }

    private void OnEnable()
    {
        if (!DataManager.isInitialized()) return;
        Setup();
    }

    private void Start()
    {
        if (!DataManager.isInitialized()) return;
        Setup();
    }
    
    

    public void Setup()
    {
        if (textDisplay == null)
        {
            textDisplay = GetComponent<TextMeshProUGUI>();
            if (textDisplay == null)
            {
                Debug.LogError($"GameObject {gameObject.name} does not have TextMeshProUGUI component");
                return;
            }
            LocalizationManager.registerStringLocalizer(this);
        }
        if (key.Trim() != "") UpdateKey();
        if (textDisplay.font != DataManager.dictKeyToFont[keyFont])
        {
            textDisplay.font = DataManager.dictKeyToFont[keyFont];
        }

        textDisplay.fontSize = overrideFontSize ? fontSizeOverride : DataManager.dictKeyToSize[keyFont];
    }

    private void UpdateKey()
    {
        if (!DataManager.dictLocalization.ContainsKey(key))
        {
            Debug.LogError($"{gameObject.name} is trying to call the \"{key}\" key which does not exist.");
            return;
        }
        localizedString = DataManager.dictLocalization[key];
        textDisplay.SetText(localizedString.getText());
    }

    private void OnDestroy()
    {
        LocalizationManager.unregisterStringLocalizer(this);
    }

    public void onLocaleChange(string locale)
    {
        Setup();
    }
}
