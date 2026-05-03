using UnityEngine;

public class ParallaxLayer : MonoBehaviour, IParallaxedObject
{
    
    [HideInInspector] public RectTransform rectTransform;
    private Vector2 startPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    public void applyParallaxOffset(Vector2 delta)
    {
        rectTransform.anchoredPosition = startPos + delta;
    }
    

    // Start is called before the first frame update
    private void Start()
    {
        EventManagers.parallax.dispatchEvent(it => it.onParallaxDeclaration(this));
    }

    public RectTransform getRectTransform()
    {
        return rectTransform;
    }
}
