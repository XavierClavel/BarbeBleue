using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    private InputMaster controls;
    private bool isGamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    private bool isPauseButtonDisplayed = false;
    private float displayPosition;
    private float hidePosition;
    private Sequence sequence = null;
    private const float transitionDuration = 0.3f;
    private const float pauseDuration = 2f;
    // Share of the screen's short side a tap is allowed to drift before it counts as a drag.
    private const float tapMoveTolerance = 0.02f;
    private Vector2 tapStartPosition;
    private bool hasTapStartPosition = false;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controls = new InputMaster();
        controls.Player.Pause.performed += _ => PauseUnpause();
        controls.Player.TriggerPauseButton.started += _ => OnTapStarted();
        controls.Player.TriggerPauseButton.performed += _ => OnTapPerformed();
        controls.Enable();
        pauseMenu.SetActive(false);
        displayPosition = pauseButton.transform.localPosition.y;
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

    private static bool TryGetPointerPosition(out Vector2 position)
    {
        var pointer = Pointer.current;
        position = pointer == null ? Vector2.zero : pointer.position.ReadValue();
        return pointer != null;
    }

    private void OnTapStarted()
    {
        hasTapStartPosition = TryGetPointerPosition(out tapStartPosition);
    }

    private void OnTapPerformed()
    {
        // The tap interaction only checks how long the press lasted, so a quick scroll swipe
        // reaches this point too. Discard it when the pointer travelled while it was down.
        if (hasTapStartPosition && TryGetPointerPosition(out var tapEndPosition))
        {
            float maxMove = Mathf.Min(Screen.width, Screen.height) * tapMoveTolerance;
            if ((tapEndPosition - tapStartPosition).sqrMagnitude > maxMove * maxMove) return;
        }

        TriggerPauseButton();
    }

    private void TriggerPauseButton()
    {
        sequence?.Kill();
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(Vault.scene.TitleScreen);
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
