using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler Instance {get; private set;}

    public event EventHandler OnStateChanged;
    public event EventHandler onPaused;
    public event EventHandler onUnpaused;

    private enum State{ // states of game
        WaitingToStart,
        CountDownToStart, 
        GamePlaying,
        GameOver,
    }

    private State state;
    private float waitingToStartTimer = 1f;
    private float countDownTimer = 3f;
    private float playingTimer;
    private float playingTimerMax = 300f;
    private bool isPaused = false;


    private void Awake() {
        Instance = this;

        state = State.WaitingToStart;
    }

    private void Start() {
        GameInput.Instance.onPause += GameInput_OnPause;
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        TogglePause();
    }

    private void Update() {
        switch(state){
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if(waitingToStartTimer <= 0f){
                    state = State.CountDownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
            break;

            case State.CountDownToStart:
                countDownTimer -= Time.deltaTime;
                if(countDownTimer <= 0f){
                    state = State.GamePlaying;
                    playingTimer = playingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
            break;

            case State.GamePlaying:
                playingTimer -= Time.deltaTime;
                if(playingTimer <= 0f){
                    state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
            break;

            case State.GameOver:
            break;
        }
        //Debug.Log(state);
    }

    public bool gameIsPlaying(){
        return state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive(){
        return state == State.CountDownToStart;
    }

    public float GetCountDownTimer(){
        return countDownTimer;
    }

    public bool IsGameOver(){
        return state == State.GameOver;
    }

    public float getGamePlayingTimerNormalized(){
        return 1 - playingTimer / playingTimerMax;
    }

    public void TogglePause(){
        isPaused = !isPaused;
        if(isPaused){
            Time.timeScale = 0f;
            onPaused?.Invoke(this, EventArgs.Empty);
        }else{
            Time.timeScale = 1f;
            onUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
