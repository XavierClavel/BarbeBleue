using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    private InputMaster controls;
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    private bool isPauseButtonDisplayed = false;
    private int i = 0;
    private float displayPosition;
    private float hidePosition;
    private Sequence sequence = null;
    private const float transitionDuration = 0.3f;
    private const float pauseDuration = 2f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new InputMaster();
        controls.Player.Pause.performed += _ => PauseUnpause();
        controls.Player.TriggerPauseButton.performed += _ => TriggerPauseButton();
        controls.Enable();
        pauseMenu.SetActive(false);
        displayPosition = pauseButton.transform.localPosition.y;
        Debug.Log(displayPosition);
        hidePosition = 250f + displayPosition;
        pauseButton.transform.localPosition = hidePosition * Vector3.up;
        sequence = DOTween.Sequence();
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
        
        sequence.Kill();
        if (isPauseButtonDisplayed)
        {
            sequence = DOTween.Sequence()
                    .AppendCallback(() => { isPauseButtonDisplayed = false; })
                    .Append(pauseButton.transform.DOLocalMoveY(hidePosition, transitionDuration).SetEase(Ease.InOutQuad))
                ;
        }
        else
        {
            sequence = DOTween.Sequence()
                    .AppendCallback(() => { isPauseButtonDisplayed = true; })
                    .Append(pauseButton.transform.DOLocalMoveY(displayPosition, transitionDuration).SetEase(Ease.InOutQuad))
                    .AppendInterval(pauseDuration)
                    .AppendCallback(() => { isPauseButtonDisplayed = false; })
                    .Append(pauseButton.transform.DOLocalMoveY(hidePosition, transitionDuration).SetEase(Ease.InOutQuad))
                ;
        }
        
        sequence.Play();

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
