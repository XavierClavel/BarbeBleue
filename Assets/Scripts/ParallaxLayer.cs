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

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent.GetComponent<RectTransform>();
        if (parent.gameObject.name == "Mask")
        {
            sceneOffset += parent.anchoredPosition.x;
            parent = parent.parent.GetComponent<RectTransform>();
        }
        if (parent.gameObject.name.StartsWith("Offset"))
        {
            sceneOffset += parent.anchoredPosition.x;
            parent = parent.parent.GetComponent<RectTransform>();
        }
    }
    

    // Start is called before the first frame update
    private void Start()
    {
        EventManagers.parallax.dispatchEvent(it => it.onParallaxDeclaration(this));
    }

}
