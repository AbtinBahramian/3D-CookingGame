using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance {get; private set;}
    
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button interactAltButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button gamepadInteractButton;
    [SerializeField] private Button gamepadinteractAltButton;
    [SerializeField] private Button gamepadPauseButton;

    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAltText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private TextMeshProUGUI gamepadInteractText;
    [SerializeField] private TextMeshProUGUI gamepadInteractAltText;
    [SerializeField] private TextMeshProUGUI gamepadPauseText;

    [SerializeField] private Transform pressToRebindTransform;

    private Action onCloseButton;


    private void Awake() {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() => {
            SoundManager.Instance.changeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.changeVolume();
            UpdateVisual();
        });

        closeButton.onClick.AddListener(() => {
            onCloseButton();
            Hide();
        });

        moveUpButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.move_Up);
        });
        moveDownButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.move_Down);
        });
        moveLeftButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.move_Left);
        });
        moveRightButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.move_Right);
        });
        interactButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.interact);
        });
        interactAltButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.interact_Alt);
        });
        pauseButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.pause);
        });
        gamepadInteractButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.gamepad_Interact);
        });
        gamepadinteractAltButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.gamepad_Interact_Alt);
        });
        gamepadPauseButton.onClick.AddListener(() => {
           RebindBinding(GameInput.Binding.gamepad_Pause);
        });
    }


    private void Start() {
        GameHandler.Instance.onUnpaused += GameHandler_onUnPaused;
        pressToRebindTransform.gameObject.SetActive(false);

        UpdateVisual();

        Hide();
    }

    private void GameHandler_onUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual(){
        soundEffectsText.text = "Sound Effects:" + Mathf.Round((SoundManager.Instance.GetVolume() * 10f));
        musicText.text = "Music:" + Mathf.Round((MusicManager.Instance.GetVolume() * 10f));

        moveUpText.text = GameInput.Instance.GetBindings(GameInput.Binding.move_Up);
        moveDownText.text = GameInput.Instance.GetBindings(GameInput.Binding.move_Down);
        moveLeftText.text = GameInput.Instance.GetBindings(GameInput.Binding.move_Left);
        moveRightText.text = GameInput.Instance.GetBindings(GameInput.Binding.move_Right);
        interactText.text = GameInput.Instance.GetBindings(GameInput.Binding.interact);
        interactAltText.text = GameInput.Instance.GetBindings(GameInput.Binding.interact_Alt);
        pauseText.text = GameInput.Instance.GetBindings(GameInput.Binding.pause);
        gamepadInteractText.text = GameInput.Instance.GetBindings(GameInput.Binding.gamepad_Interact);
        gamepadInteractAltText.text = GameInput.Instance.GetBindings(GameInput.Binding.gamepad_Interact_Alt);
        gamepadPauseText.text = GameInput.Instance.GetBindings(GameInput.Binding.gamepad_Pause);


    }

    public void Show(Action onCloseButton){
        this.onCloseButton = onCloseButton;
        gameObject.SetActive(true);

        soundEffectsButton.Select(); // to select the button
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    public void ShowPressToRebindKey(){
        pressToRebindTransform.gameObject.SetActive(true);
    }

    public void HidePressToRebindKey(){
        pressToRebindTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () => {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

}
