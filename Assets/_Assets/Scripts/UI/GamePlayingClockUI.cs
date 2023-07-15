using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image gamePlayingTimer;

    private void Awake() {
        gamePlayingTimer.gameObject.SetActive(false);
    }

    private void Update() {
        if(GameHandler.Instance.gameIsPlaying()){
            gamePlayingTimer.gameObject.SetActive(true);
        }
        
        gamePlayingTimer.fillAmount = GameHandler.Instance.getGamePlayingTimerNormalized();
    }
}
