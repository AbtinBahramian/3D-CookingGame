using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    public event EventHandler<IHasProgress.OnProgressChangedArgs> OnProgressChanged;
   
    public event EventHandler<OnStateChangedArgs> OnStateChanged;
    public class OnStateChangedArgs : EventArgs{
        public State state;
    }

    public enum State{
        Idle,
        Frying,
        Fried,
        Burned,
    }

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {  // State Machin
        if(HasKitchenObject()){
            switch (state)
            {
            case State.Idle:
            break;

            case State.Frying:
                fryingTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs{
                    progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                });

                if(fryingTimer > fryingRecipeSO.fryingTimerMax){
                    //fried
                    
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                    
                    state = State.Fried;
                    burningRecipeSO = GetBurningRepipeSO(GetKitchenObject().GetKitchenObjectSO());
                    burningTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedArgs{
                        state = state
                    });
                }
            break;

            case State.Fried:
                burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs{
                    progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                });

                if(burningTimer > burningRecipeSO.burningTimerMax){
                    //burned

                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
                    state = State.Burned;

                    OnStateChanged?.Invoke(this, new OnStateChangedArgs{ //when states chenging
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs{
                        progressNormalized = 0f
                    });
                }
            break;

            case State.Burned:
            break;
            
            }
        }
        
    }

    public override void Interact(Player player)
    {
        if(!HasKitchenObject()){
            if(player.HasKitchenObject()){
                if(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())){

                    //player carry an object that has recipe
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRepipeSO(GetKitchenObject().GetKitchenObjectSO());

                    fryingTimer = 0f;
                    state = State.Frying;

                    OnStateChanged?.Invoke(this, new OnStateChangedArgs{
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs{
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
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
                    state = State.Idle;

                    OnStateChanged?.Invoke(this, new OnStateChangedArgs{
                            state = state
                        });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs{
                        progressNormalized = 0f
                    });       
                }
            }else{
                //player has not something
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedArgs{
                        state = state
                    });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedArgs{
                    progressNormalized = 0f
                });
            }
        }
        
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRepipeSO(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO){
        FryingRecipeSO fryingRecipeSO = GetFryingRepipeSO(inputKitchenObjectSO);
        if(fryingRecipeSO != null){
            return fryingRecipeSO.output;
        }else{
            return null;
        }
    }
    private FryingRecipeSO GetFryingRepipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if(fryingRecipeSO.input == inputKitchenObjectSO){
                return fryingRecipeSO;
            }
        }
        return null;
    }
    private BurningRecipeSO GetBurningRepipeSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if(burningRecipeSO.input == inputKitchenObjectSO){
                return burningRecipeSO;
            }
        }
        return null;
    }
}
