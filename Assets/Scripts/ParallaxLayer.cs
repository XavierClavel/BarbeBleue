using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxLayer : MonoBehaviour
{
    
    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public RectTransform parent;
    private bool initialized = false;
    [HideInInspector] public float sceneOffset = 0f;
    private Vector2 startPos;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent.GetComponent<RectTransform>();
        while (parent.gameObject.name == "Mask" || 
               parent.gameObject.name.StartsWith("Offset") ||
               parent.gameObject.name == "Wrapper"
               )
        {
            sceneOffset += parent.anchoredPosition.x;
            parent = parent.parent.GetComponent<RectTransform>();
        }

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

}
