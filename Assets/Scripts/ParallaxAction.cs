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

public class ParallaxAction : MonoBehaviour, IParallaxedObject
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
    [HideInInspector] public Vector3 basePosition;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actuator = GetComponent<RectTransform>();
        rt = GetComponent<RectTransform>();
        basePosition = rt.localPosition;
        
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


    public RectTransform getRectTransform()
    {
        return rt;
    }
}
