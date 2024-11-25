using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    private InputMaster controls;
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    private int i = 0;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new InputMaster();
        controls.Player.Pause.performed += _ => PauseUnpause();
        controls.Player.TriggerPauseButton.performed += _ => TriggerPauseButton();
        controls.Enable();
        pauseMenu.SetActive(false);
        pauseButton.SetActive(false);
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

    private void TriggerPauseButton()
    {
        i++;
        StopCoroutine(nameof(waitAndDeactivate));
        if (pauseButton.activeInHierarchy)
        {
            pauseButton.SetActive(false);
        }
        else
        {
            StartCoroutine(nameof(waitAndDeactivate));    
        }
        
    }
    
    public void ToTitleScreen()
    {
        SceneManager.LoadScene(Vault.scene.TitleScreen);
    }

    private IEnumerator waitAndDeactivate()
    {
        var index = i;
        Debug.Log($"coroutine {index} starts");
        pauseButton.SetActive(true);
        yield return Helpers.getWaitRealtime(2f);
        pauseButton.SetActive(false);
        Debug.Log($"coroutine {index} ends");
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
