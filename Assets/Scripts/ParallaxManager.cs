using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxManager : MonoBehaviour, IParallax
{
    [SerializeField] RectTransform content;
    [SerializeField] RectTransform firstScene;
    public static ParallaxManager instance;
    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
    
    private const float parallaxCoeff = 0.005f;
    public const float offset = 0;//513f;
    
    // Start is called before the first frame update
    private void Awake()
    {
        EventManagers.parallax.registerListener(this);
        instance = this;
    }

    public void setup()
    {
        parallaxLayers.ForEach(it => it.setup());
    }

    public void onParallaxDeclaration(ParallaxLayer parallax)
    {
        parallaxLayers.Add(parallax);
    }

    public Vector2 getParallaxOffset(ParallaxLayer layer)
    {
        var deltaPos = content.anchoredPosition.x + layer.parent.anchoredPosition.x - firstScene.anchoredPosition.x + layer.sceneOffset;
        var parallaxIntensity = parallaxCoeff * layer.rectTransform.anchoredPosition3D.z;
        return deltaPos * parallaxIntensity * Vector2.right;
    }
        
}
