using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    private InputMaster controls;
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private bool disableOnStart = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new InputMaster();
        controls.Player.Pause.performed += _ => PauseUnpause();
        controls.Enable();
        if (disableOnStart)
        {
            pauseMenu.SetActive(false);   
        }
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void PauseUnpause()
    {
        if (isGamePaused) Unpause();
        else Pause();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(Vault.scene.MainScene);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        pauseMenu.SetActive(isGamePaused);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        pauseMenu.SetActive(isGamePaused);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
