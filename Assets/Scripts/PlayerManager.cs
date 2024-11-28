
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
        StartCoroutine(nameof(LateStart));
    }

    private IEnumerator LateStart()
    {
        yield return null;
        Checkpoint.list.ForEach(it => it.setup());
        CheckpointManager.instance.setup();
        Checkpoint.list.ForEach(it => it.setInitialPosition());
    }
}
