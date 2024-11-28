
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointData
{
    public string key;
    public float position;

    public CheckpointData(string key, float position)
    {
        this.key = key;
        this.position = position;
    }
}

public class Checkpoint: MonoBehaviour
{
    [SerializeField] private string key;
    private float lastPos;
    public CheckpointData data;
    private bool initialized = false;


    private void Start()
    {
        EventManagers.checkpoint.dispatchEvent(it => it.onCheckpointDeclaration(this));
    }


    public void setup()
    {
        var rt = GetComponent<RectTransform>();
        data = new CheckpointData(key, rt.anchoredPosition.x);
    }
    
    public void setInitialPosition()
    {
        lastPos = transform.position.x;
        initialized = true;
    }

    private void FixedUpdate()
    {
        if (!initialized) return;
        if (Mathf.Sign(transform.position.x) * Mathf.Sign(lastPos) < 0)
        {
            EventManagers.checkpoint.dispatchEvent(it => it.onCheckpointReached(data));
        }
        lastPos = transform.position.x;
    }

}
