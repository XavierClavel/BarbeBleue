using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
   private Transform transform;
    
    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.transform;
    }

    public void onScroll(float x)
    {
        transform.position = x * Vector3.right;
    }

}
