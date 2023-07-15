using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;


    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        resumeButton.onClick.AddListener(() => {
            GameHandler.Instance.TogglePause();
        });
        optionsButton.onClick.AddListener(() => {
            Hide();
            OptionsUI.Instance.Show(Show);
            
        });
    }

    private void Start() {
        GameHandler.Instance.onPaused += GameHandler_OnPaused;
        GameHandler.Instance.onUnpaused += GameHandler_OnUnpaused;

        gameObject.SetActive(false);
    }

    private void GameHandler_OnUnpaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void GameHandler_OnPaused(object sender, EventArgs e)
    {
        Show();
    }

    public void Show(){
        gameObject.SetActive(true);

        resumeButton.Select(); // for be selectable by gamepad
    }

    public void Hide(){
        gameObject.SetActive(false);
    }
}
