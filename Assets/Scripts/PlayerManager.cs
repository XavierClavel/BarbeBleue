
using System;
using UnityEngine;

public class PlayerManager: MonoBehaviour
{

    private void Awake()
    {
        DataManager.LoadData();
    }
}
