using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    private void Awake()
    {
        DataManager.LoadData();
    }

    private void Start()
    {
#if UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Vault.scene.MainScene);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
}
