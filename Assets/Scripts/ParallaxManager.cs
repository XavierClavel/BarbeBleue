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
    [SerializeField] private Canvas canvas;
    public static ParallaxManager instance;
    private List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();
    private List<ParallaxAction> parallaxActions = new List<ParallaxAction>();
    private Dictionary<ParallaxLayer, bool> inProcess = new Dictionary<ParallaxLayer, bool>();
    public float offset = 0;
    public Checkpoint startAt;
    
    private const float parallaxCoeff = 0.005f;
    private const float fallbackBuffer = 800f;
    private RectTransform canvasRT;
    
    // Start is called before the first frame update
    private void Awake()
    {
        EventManagers.parallax.registerListener(this);
        EventManagers.parallaxActions.registerListener(this);
        instance = this;
        StartCoroutine(nameof(occlusionCallingCoroutine));
        canvasRT = canvas.GetComponent<RectTransform>();
        if (startAt != null)
        {
            content.anchoredPosition = -startAt.rectTransform.anchoredPosition;
        }
    }

    public void onParallaxDeclaration(ParallaxLayer parallax)
    {
        parallaxLayers.Add(parallax);
    }

    public void onParallaxActionDeclaration(ParallaxAction parallax)
    {
        parallaxActions.Add(parallax);
        Debug.Log($"{parallaxActions.Count} parallax actions");
    }


    private Vector2 getParallaxOffset(ParallaxLayer layer)
    {
        var deltaPos = getPositionRelativeToCamera(layer);
        var parallaxIntensity = parallaxCoeff * layer.rectTransform.anchoredPosition3D.z;
        
        return deltaPos * parallaxIntensity * Vector2.right;
    }

    private void Update()
    {
        parallaxLayers.ForEach(it =>
        {
            var layerQueuedForDeactivation = manageOcclusionCulling(it);
            if (!layerQueuedForDeactivation)
            {
                updatePosition(it);
            }
        });
        
        parallaxActions.ForEach(it =>
        {
            doParallaxActions(it);
        });
    }

    private void doParallaxActions(ParallaxAction action)
    {
        if (!action.gameObject.activeInHierarchy) return;
        var distanceToCamera = content.anchoredPosition.x + action.rt.anchoredPosition.x + action.parent.anchoredPosition.x + action.sceneOffset - Screen.width * 0.5f;
        var distanceToCameraPix = 0.5f + distanceToCamera / Screen.width;
        var objectSize = action.rt.rect.width / canvasRT.rect.width;
        var ratio = (distanceToCameraPix + 0.5f * objectSize) / (1f + objectSize);

        var value = action.startValue + Mathf.Clamp01(ratio) * (action.endValue - action.startValue);

        switch (action.type)
        {
            case actionType.LocalRotate:
                action.actuator.localEulerAngles = value * Vector3.forward;
                break;
            
            case actionType.LocalYSinusoid:
                action.actuator.localPosition = action.basePosition + (float)Math.Sin(ratio * action.magnitude + action.offset) * action.amplitude * Vector3.up;
                break;
            
            case actionType.LocalYTranslate:
                action.actuator.localPosition = action.basePosition + value * Vector3.up;
                break;
        }
    }

    private float getPositionRelativeToCamera(ParallaxLayer layer)
    {
        return content.anchoredPosition.x
               + layer.parent.anchoredPosition.x
               + layer.rectTransform.anchoredPosition.x
               + layer.sceneOffset
               - Screen.width * 0.5f;
    }

    private bool manageOcclusionCulling(ParallaxLayer layer)
    {
        var occlusionCullingBuffer = UnityEngine.Device.Screen.width;
        var distanceToCamera = getPositionRelativeToCamera(layer);
        var distanceToFrustum = Mathf.Abs(distanceToCamera) - (layer.rectTransform.rect.width + Screen.width) * 0.5f;
        var shouldBeDisplayed = distanceToFrustum < occlusionCullingBuffer;
        
        //Fallback in case the coroutine takes too long
        if (inProcess.TryGetValue(layer, out bool value))
        {
            if (value != shouldBeDisplayed) return false;
            if (distanceToFrustum > fallbackBuffer) return false;
            Debug.Log("forced occlusion culling");
            inProcess.Remove(layer);
            layer.gameObject.SetActive(shouldBeDisplayed);
        }
        
        if (layer.gameObject.activeInHierarchy != shouldBeDisplayed)
        {
            inProcess[layer] = shouldBeDisplayed;
        }

        return !shouldBeDisplayed;
    }
    
    private void updatePosition(ParallaxLayer layer)
    {
        if (!layer.gameObject.activeInHierarchy) return;
        layer.applyParallaxOffset(getParallaxOffset(layer));
    }

    private IEnumerator occlusionCallingCoroutine()
    {
        while (true)
        {
            yield return null;
            if (inProcess.Keys.Count == 0) continue;
            foreach(var it in new List<ParallaxLayer>(inProcess.Keys))
            {
                var status = inProcess[it];
                it.gameObject.SetActive(status);
                if (status)
                {
                    updatePosition(it);
                }
                inProcess.Remove(it);
            };
        }
    }
        
}
