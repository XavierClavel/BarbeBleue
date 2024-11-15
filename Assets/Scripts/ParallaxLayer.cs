using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void onScroll(float x)
    {
        transform.position = x * Vector3.right;
    }

}
