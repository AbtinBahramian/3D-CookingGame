using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int spawnedPlateMaount;
    private int spawnedPlateMaountMax = 4;

    private void Update() {
        spawnPlateTimer += Time.deltaTime;

        if(spawnPlateTimer > spawnPlateTimerMax){
            spawnPlateTimer = 0f;
            if(spawnedPlateMaount < spawnedPlateMaountMax){
                spawnedPlateMaount ++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject()){
            //player does not have anything
            if(spawnedPlateMaount > 0){
                //we have plates oon the counter

                spawnedPlateMaount --;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
