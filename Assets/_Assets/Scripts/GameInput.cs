using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{   
    public static GameInput Instance {get; private set;}

    private const string PLAYER_PREFS_INPUT_BINDING = "PlayerInputBinding";
    
    private PlayerInputActions playerInputActions;
    public event EventHandler onInteractAction; // we build an event for using in player
    public event EventHandler onAlternateInteraction; 
    public event EventHandler onPause;

    public enum Binding{
        move_Up,
        move_Down,
        move_Right,
        move_Left,
        interact, 
        interact_Alt,
        pause,
        gamepad_Interact,
        gamepad_Interact_Alt,
        gamepad_Pause,
    }
    
    private void Awake() {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        if(PlayerPrefs.HasKey(PLAYER_PREFS_INPUT_BINDING)){
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_INPUT_BINDING)); 
        }
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed; // subscribing to performed event
        playerInputActions.Player.AlternateInteract.performed += AlternateInteract_performed;
        playerInputActions.Player.Pause.performed += Pause_Performed;

    }

    private void OnDestroy() {
        playerInputActions.Player.Interact.performed -= Interact_performed; 
        playerInputActions.Player.AlternateInteract.performed -= AlternateInteract_performed;
        playerInputActions.Player.Pause.performed -= Pause_Performed;

        playerInputActions.Dispose();
    }

    private void Pause_Performed(InputAction.CallbackContext context)
    {
        onPause?.Invoke(this, EventArgs.Empty);
    }

    private void AlternateInteract_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        onAlternateInteraction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj){
        onInteractAction?.Invoke(this, EventArgs.Empty);

        // just like the upper line
        // 
        // if(onInteractAction != null){
        //     onInteractAction(this, EventArgs.Empty);
        // }

    }
    public Vector2 GameInputVector2Normalized(){
        
        Vector2 inputVector2 = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector2 = inputVector2.normalized;
        return inputVector2;
    }

    public string GetBindings(Binding binding){
        switch (binding){
            default:
            case Binding.move_Up:
            return playerInputActions.Player.Move.bindings[1].ToDisplayString();

            case Binding.move_Down:
            return playerInputActions.Player.Move.bindings[2].ToDisplayString();

            case Binding.move_Left:
            return playerInputActions.Player.Move.bindings[3].ToDisplayString();

            case Binding.move_Right:
            return playerInputActions.Player.Move.bindings[4].ToDisplayString();

            case Binding.interact:
            return playerInputActions.Player.Interact.bindings[0].ToDisplayString();

            case Binding.interact_Alt:
            return playerInputActions.Player.AlternateInteract.bindings[0].ToDisplayString();

            case Binding.pause:
            return playerInputActions.Player.Pause.bindings[0].ToDisplayString();

            case Binding.gamepad_Interact:
            return playerInputActions.Player.Interact.bindings[1].ToDisplayString();

            case Binding.gamepad_Interact_Alt:
            return playerInputActions.Player.AlternateInteract.bindings[1].ToDisplayString();

            case Binding.gamepad_Pause:
            return playerInputActions.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onAction){
        playerInputActions.Player.Disable();
        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.move_Up:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 1;
                break;

            case Binding.move_Down:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 2;
                break;

            case Binding.move_Left:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 3;
                break;

            case Binding.move_Right:
                inputAction = playerInputActions.Player.Move;
                bindingIndex = 4;
                break;

            case Binding.interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 0;
                break;

            case Binding.interact_Alt:
                inputAction = playerInputActions.Player.AlternateInteract;
                bindingIndex = 0;
                break;

            case Binding.pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 0;
                break;

            case Binding.gamepad_Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingIndex = 1;
                break;

            case Binding.gamepad_Interact_Alt:
                inputAction = playerInputActions.Player.AlternateInteract;
                bindingIndex = 1;
                break;

            case Binding.gamepad_Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingIndex = 1;
                break;
                

        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
        .OnComplete(callback => {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onAction();

            PlayerPrefs.SetString(PLAYER_PREFS_INPUT_BINDING, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        })
        .Start();
    }
}
