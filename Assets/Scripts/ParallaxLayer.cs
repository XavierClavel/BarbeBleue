using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private static float parallaxCoeff = 0.005f;
    private RectTransform rectTransform;
    private Vector3 initPos;
    
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        initPos = rectTransform.localPosition;
    }

    void Update()
    {
        rectTransform.localPosition = initPos + (parallaxCoeff * rectTransform.parent.position.x * transform.localPosition.z) * Vector3.right ;
        Debug.Log(transform.localPosition.z);
    }

}
