using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnObjectTrashed;

    new public static void ResetStaticData(){ // we should reset static events
        OnObjectTrashed = null;
    }

    public override void Interact(Player player){
        if(player.HasKitchenObject()){
            player.GetKitchenObject().DestroySelf();
            

            OnObjectTrashed?.Invoke(this, EventArgs.Empty);
        }
    }
}
