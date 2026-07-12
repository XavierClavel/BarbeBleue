using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public enum actionType
{
    GlobalRotate,
    LocalRotate,
    Sinusoid,
    LocalYSinusoid,
    LocalYTranslate,
    Blink,
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

    [CanBeNull] private Image image;
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        actuator = GetComponent<RectTransform>();
        rt = GetComponent<RectTransform>();
        TryGetComponent(out Image objectImage);
        image = objectImage;
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

    public void HideImage()
    {
        if (image == null)
        {
            return;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
    
    public void ShowImage()
    {
        if (image == null)
        {
            return;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    public void setAlpha(float alpha)
    {
        if (image == null)
        {
            return;
        }
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }


    public RectTransform getRectTransform()
    {
        return rt;
    }
}
