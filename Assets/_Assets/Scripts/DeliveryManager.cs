using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance {get; private set; }
    
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnDeliverySuccess;
    public event EventHandler OnDeliveryFailed;
   
    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float waitingRecipeTime;
    private float waitingRecipeTimeMax = 4f;
    private int waitingRecipeMax = 4;
    private int recipeDeliveredAmount = 0;

    private void Awake() {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update() {
        waitingRecipeTime -= Time.deltaTime;
        if(waitingRecipeTime <= 0f){
            waitingRecipeTime = waitingRecipeTimeMax;
            if(waitingRecipeSOList.Count < waitingRecipeMax){
                RecipeSO recipeSO = recipeListSO.recipeListSO[UnityEngine.Random.Range(0, recipeListSO.recipeListSO.Count)];
                
                waitingRecipeSOList.Add(recipeSO);
                
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject){
        for(int i=0; i < waitingRecipeSOList.Count; i++){
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count){ 
                //has the same number of ingridients as the recipe
                bool plateContetnMatchesRecipe = true;

                foreach(KitchenObjectSO recipeKitcheObjectSO in waitingRecipeSO.kitchenObjectSOList){
                    //Cycling through all the ingridents in the Recipe
                    bool ingridientFound = false;
                    foreach(KitchenObjectSO plateKitcheObjectSO in plateKitchenObject.GetKitchenObjectSOList()){
                        //Cycling through all the ingridents in the Plate
                        if(recipeKitcheObjectSO == plateKitcheObjectSO){
                            //Ingridients Matches
                            ingridientFound = true;
                            break;
                        }
                    }
                    if(!ingridientFound){
                        //this ingridient did not found
                        plateContetnMatchesRecipe = false;
                    }
                }
                if(plateContetnMatchesRecipe){
                    //player deliverd the correct recipe
                    recipeDeliveredAmount ++;
                    waitingRecipeSOList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }

            }
        }
        //no matches found
        Debug.Log("no matches found");
        OnDeliveryFailed?.Invoke(this,EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList(){
        return waitingRecipeSOList;
    }

    public int GetRecipeDeliveredAmount(){
        return recipeDeliveredAmount;
    }
}
