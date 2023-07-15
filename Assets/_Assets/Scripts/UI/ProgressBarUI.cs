using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectHasProgress;
    [SerializeField] private Image barImage;
    
    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = gameObjectHasProgress.GetComponent<IHasProgress>(); // we did this because in UI we cant see the interface
        if(hasProgress == null){
            Debug.LogError("game object" + gameObjectHasProgress + "does not have a component that impliment IHasProgress");
        }
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f;
        hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if(e.progressNormalized == 1f || e.progressNormalized == 0f){
            hide();
        }else{
            show();
        }
    }

    private void show(){
        gameObject.SetActive(true);
    }
    private void hide(){
        gameObject.SetActive(false);
    }
}
