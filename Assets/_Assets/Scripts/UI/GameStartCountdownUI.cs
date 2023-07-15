using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;

    private void Start() {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;

        Hide();
    }

    private void Update() {
        countDownText.text = Mathf.Ceil(GameHandler.Instance.GetCountDownTimer()).ToString();
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if(GameHandler.Instance.IsCountDownToStartActive()){
            Show();
        }else{
            Hide();
        }
    }

    private void Show(){
        countDownText.gameObject.SetActive(true);
    }
    private void Hide(){
        countDownText.gameObject.SetActive(false);
    }
}
