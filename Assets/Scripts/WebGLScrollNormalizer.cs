using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WebGLScrollNormalizer : MonoBehaviour
{
    private float webGLScrollFactor = 0.0005f; // Adjust for desired sensitivity

    private void Start()
    {
        var scrollRect = GetComponent<ScrollRect>();
        Debug.Log(scrollRect.scrollSensitivity);
#if !UNITY_EDITOR && UNITY_WEBGL
        scrollRect.scrollSensitivity *= webGLScrollFactor;
#endif
    }

}