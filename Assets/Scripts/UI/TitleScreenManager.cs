using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(Vault.scene.MainScene);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
}
