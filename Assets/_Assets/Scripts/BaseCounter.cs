using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectparent
{
    [SerializeField] private Transform counterTopPoint;

    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData(){ // we should reset static events
        OnAnyObjectPlacedHere = null;
    }
    
    private KitchenObject kitchenObject;

    public virtual void Interact(Player player){
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternate(Player player){
       // Debug.LogError("BaseCounter.InteractAlternate()");
    }
    
    public Transform GetKitchenObjectFollowTransform(){
        return counterTopPoint;
    }
    public bool HasKitchenObject(){
        return kitchenObject != null;
    }
    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null){
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject(){
        return kitchenObject;
    }
    public void ClearKitchenObject(){
        kitchenObject = null;
    }
}
