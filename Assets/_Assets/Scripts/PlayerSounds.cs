using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    
    private float footStepTimer;
    private float footStepTimerMax = .1f;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        footStepTimer -= Time.deltaTime;
        if(footStepTimer < 0f){
            footStepTimer = footStepTimerMax;

            if(player.Is_Walking()){
                float volume = 1f;
                SoundManager.Instance.PlayFootStepsSound(player.transform.position, volume);
            }
            
        }
    }
}

