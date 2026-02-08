using DG.Tweening;
using UnityEngine;

public enum actionType
{
    GlobalRotate,
    LocalRotate,
    Sinusoid,
    LocalYSinusoid,
    LocalYTranslate,
}

public class ParallaxAction : MonoBehaviour
{
    [SerializeField] public actionType type;
    [HideInInspector] public RectTransform rt;
    [HideInInspector] public RectTransform actuator;
    [SerializeField] private float duration;
    [SerializeField] public float startValue;
    [SerializeField] public float endValue;
    [SerializeField] public float magnitude;
    [SerializeField] public float amplitude;
    [SerializeField] public float offset;
    [HideInInspector] public RectTransform parent;
    [HideInInspector] public float sceneOffset = 0f;
    [HideInInspector] public Vector3 basePosition;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actuator = GetComponent<RectTransform>();
        rt = GetComponent<RectTransform>();
        basePosition = rt.localPosition;
        parent = rt.parent.GetComponent<RectTransform>();
        while (parent.gameObject.name == "Mask" || 
               parent.gameObject.name.StartsWith("Offset") ||
               parent.gameObject.name == "Wrapper"
              )
        {
            sceneOffset += parent.anchoredPosition.x;
            parent = parent.parent.GetComponent<RectTransform>();
        }
        
        
        
        switch (type)
        {
            case actionType.GlobalRotate:
                rt.DOLocalRotate(360f * Vector3.forward, duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                return;
            
            case actionType.Sinusoid:
                rt.DOLocalRotate(360f * Vector3.forward, duration, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
                return;
        }
        
        EventManagers.parallaxActions.dispatchEvent(it => it.onParallaxActionDeclaration(this));
    }
    
    
}
