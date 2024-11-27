using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxLayer : MonoBehaviour
{
    private static float parallaxCoeff = 0.005f;
    private RectTransform rectTransform;
    private Vector3 initPos;
    private const float offset = -513f;
    
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initPos = rectTransform.localPosition + offset * Vector3.right;
        updatePosition();
    }

    void Update()
    {
        updatePosition();
    }

    private void updatePosition()
    {
        rectTransform.localPosition = initPos + (parallaxCoeff * (rectTransform.parent.position.x + 2 * offset) * transform.localPosition.z) * Vector3.right;
    }

}
