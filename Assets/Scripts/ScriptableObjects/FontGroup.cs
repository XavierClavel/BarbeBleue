using TMPro;
using UnityEngine;

public enum fontKey
{
    DEFAULT,
    LE_PETIT_CHAPERON_ROUGE,
}

[CreateAssetMenu(menuName = "Custom/FontGroup", fileName = "FontGroup", order = 1)]
public class FontGroup : ScriptableObject
{
    [SerializeField] private fontKey key;
    [SerializeField] private TMP_FontAsset font;

    public fontKey getKey() => key;
    public TMP_FontAsset getFont() => font;

}
