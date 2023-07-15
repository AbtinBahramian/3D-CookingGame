using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredAmount;

    private void Start() {
        GameHandler.Instance.OnStateChanged += GameHandler_OnStateChanged;


        Hide();
    }

    private void Update() {
       
    }

    private void GameHandler_OnStateChanged(object sender, EventArgs e)
    {
        if(GameHandler.Instance.IsGameOver()){
            Show();
            recipesDeliveredAmount.text = DeliveryManager.Instance.GetRecipeDeliveredAmount().ToString();
        }else{
            Hide();
        }
    }

    private void Show(){
        gameObject.SetActive(true);
    }
    private void Hide(){
        gameObject.SetActive(false);
    }
}
