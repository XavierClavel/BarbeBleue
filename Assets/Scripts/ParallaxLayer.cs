using UnityEngine;

public class ParallaxLayer : MonoBehaviour, IParallaxedObject
{
    
    [HideInInspector] public RectTransform rectTransform;
    // Horizontal extent (min/max X, in content-local units) of this layer and all its children.
    // Captured by ParallaxManager while the layer is still active, so it stays valid after the
    // layer is deactivated (an inactive subtree reports no bounds and a frozen transform).
    [HideInInspector] public Vector2? contentExtent;
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
