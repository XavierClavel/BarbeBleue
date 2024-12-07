using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxManager : MonoBehaviour, IParallax
{
    [SerializeField] RectTransform content;
    [SerializeField] RectTransform firstScene;
    public static ParallaxManager instance;
    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
    private Dictionary<ParallaxLayer, bool> inProcess = new Dictionary<ParallaxLayer, bool>();
    public float offset = 0;
    
    private const float parallaxCoeff = 0.005f;
    private const float occlusionCullingBuffer = 1000f;
    
    // Start is called before the first frame update
    private void Awake()
    {
        EventManagers.parallax.registerListener(this);
        instance = this;
        StartCoroutine(nameof(occlusionCallingCoroutine));
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

    private void Update()
    {
        parallaxLayers.ForEach(it =>
        {
            manageOcclusionCulling(it);
            updatePosition(it);
        });
    }

    private void manageOcclusionCulling(ParallaxLayer layer)
    {
        var distanceToCamera = content.anchoredPosition.x + layer.parent.anchoredPosition.x + layer.sceneOffset - Screen.width * 0.5f;
        var distanceToFrustum = Mathf.Abs(distanceToCamera) - (layer.rectTransform.rect.width + Screen.width) * 0.5f;
        var shouldBeDisplayed = distanceToFrustum < occlusionCullingBuffer;
        
        //Fallback in case the coroutine takes too long
        if (inProcess.TryGetValue(layer, out bool value))
        {
            if (value == shouldBeDisplayed) return;
            inProcess.Remove(layer);
            layer.gameObject.SetActive(shouldBeDisplayed);
        }
        
        if (layer.gameObject.activeInHierarchy != shouldBeDisplayed)
        {
            inProcess[layer] = shouldBeDisplayed;
        }
    }
    
    private void updatePosition(ParallaxLayer layer)
    {
        if (!layer.gameObject.activeInHierarchy) return;
        layer.rectTransform.anchoredPosition = getParallaxOffset(layer);
    }

    private IEnumerator occlusionCallingCoroutine()
    {
        while (true)
        {
            var todo = inProcess.Keys.ToList();
            todo.ForEach(it =>
            {
                it.gameObject.SetActive(inProcess[it]);
                inProcess.Remove(it);
            });
            yield return null;
        }
    }
        
}
