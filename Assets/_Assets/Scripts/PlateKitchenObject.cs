using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject  // be carefull with inheritence
{
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList; 
    
    public event EventHandler<OnIngridientAddedArgs> OnIngridientAdded;
    public class OnIngridientAddedArgs : EventArgs{
        public KitchenObjectSO kitchenObjectSO;
    } 
    private List<KitchenObjectSO> kitchenObjectSOList;
    

    private void Awake() {
        kitchenObjectSOList = new List<KitchenObjectSO>();

    } 

    public bool TryAddIngeridient(KitchenObjectSO kitchenObjectSO){ // we don't want duplicate ingridients
        if(!validKitchenObjectSOList.Contains(kitchenObjectSO)){
            // is not valid 
            return false;
        }
        if(kitchenObjectSOList.Contains(kitchenObjectSO)){
            return false;
        }else{
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngridientAdded?.Invoke(this, new OnIngridientAddedArgs{
                kitchenObjectSO = kitchenObjectSO
            });
            return true;
        }
        
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList(){
        return kitchenObjectSOList;
    }
}
