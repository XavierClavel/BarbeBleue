using UnityEngine;

public class PauseManager : MonoBehaviour
{

    private InputMaster controls;
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new InputMaster();
        controls.Player.Pause.performed += _ => PauseUnpause();
        controls.Enable();
        pauseMenu.SetActive(false);
    }

    private void PauseUnpause()
    {
        if (isGamePaused) Unpause();
        else Pause();
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

    public void setLocale(string locale)
    {
        LocalizationManager.setLanguage(locale);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
