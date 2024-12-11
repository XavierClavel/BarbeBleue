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
    private const float fallbackBuffer = 800f;
    
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
        Debug.Log($"{parallaxLayers.Count} parallax layers");
    }


    private Vector2 getParallaxOffset(ParallaxLayer layer)
    {
        var deltaPos = content.anchoredPosition.x + layer.parent.anchoredPosition.x + layer.sceneOffset - layer.parent.rect.width * 0.5f;
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
            if (value != shouldBeDisplayed) return;
            if (distanceToFrustum > fallbackBuffer) return;
            Debug.Log("forced occlusion culling");
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
            yield return null;
            if (inProcess.Keys.Count == 0) continue;
            foreach(var it in new List<ParallaxLayer>(inProcess.Keys)) {
                it.gameObject.SetActive(inProcess[it]);
                inProcess.Remove(it);
            };
        }
    }
        
}
