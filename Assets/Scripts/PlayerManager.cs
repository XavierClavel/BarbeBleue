
using System;
using System.Collections;
using UnityEngine;

public class PlayerManager: MonoBehaviour
{

    private void Awake()
    {
        DataManager.LoadData();
    }

    private void Start()
    {
        Application.targetFrameRate = -1;
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
        StartCoroutine(nameof(LateStart));
    }

    private IEnumerator LateStart()
    {
        yield return null;
        CheckpointManager.instance.setup();
    }
}
