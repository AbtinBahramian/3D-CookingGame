using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter 
{
    // [SerializeField] private KitchenObjectSO KitchenObjectSO;
    // [SerializeField] private Transform counterTopPoint;
    // //[SerializeField] private ClearCounter secendCounter;
    // //[SerializeField] private bool testing;

    //private KitchenObject kitchenObject;
    
    // private void Update() {
    //     if(testing && Input.GetKeyDown(KeyCode.T)){
    //         if(kitchenObject != null){
    //             kitchenObject.SetKitchenObjectParent(secendCounter);
    //             //Debug.Log(kitchenObject.GetClearCounter()); 
    //         }       
    //     }
    // }
    public override void Interact(Player player){
        if(!HasKitchenObject()){
            if(player.HasKitchenObject()){
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }else{
                //player has nothing
            }
        }else{
            //there is a kitchen object here
            if(player.HasKitchenObject()){
                // player has somethings
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)){ // we check the type 
                    //playe is holding a Plate

                    if(plateKitchenObject.TryAddIngeridient(GetKitchenObject().GetKitchenObjectSO())){
                        GetKitchenObject().DestroySelf();
                    }                    
                }else{
                    // player is not carryin plate but something else
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject)){
                        if(plateKitchenObject.TryAddIngeridient(player.GetKitchenObject().GetKitchenObjectSO())){
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }else{
                //player has not something
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        
    }
}
