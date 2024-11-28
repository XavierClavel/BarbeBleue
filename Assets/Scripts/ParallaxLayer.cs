using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxLayer : MonoBehaviour
{
    private static float parallaxCoeff = 0.005f;
    private RectTransform rectTransform;
    private Vector2 initPos;
    public const float offset = -513f;
    
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initPos = rectTransform.anchoredPosition + offset * Vector2.right;
        updatePosition();
    }

    void Update()
    {
        updatePosition();
    }

    private void updatePosition()
    {
        rectTransform.anchoredPosition = initPos + (parallaxCoeff * (rectTransform.parent.position.x + 2 * offset) * transform.localPosition.z) * Vector2.right;
    }

}
