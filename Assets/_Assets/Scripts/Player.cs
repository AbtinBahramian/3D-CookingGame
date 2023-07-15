using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour , IKitchenObjectparent
{
    private static Player instance;
    public static Player Instance{ //this is a property
        get{
            return instance;                          // all of these can be done with {get; private set;}
        }
        private set{
            instance = value;
        }
    }
    
    
    public event EventHandler OnPickUpSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs{
        public BaseCounter selectedCounter;
    }

    [SerializeField] private int moveSpeed = 7;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayers;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private float rotateSpeed = 9f;
    private bool isWalking;
    private Vector3 lastInteractDir;

    private void Awake() {
        if(Instance != null){
            Debug.Log("There are more than one player instances");
        }
        Instance = this;    
    }
    
    private void Start() {
        gameInput.onInteractAction += GameInput_OnInteractAction;
        gameInput.onAlternateInteraction += GameInput_OnAlternateInteraction;
    }

    private void GameInput_OnAlternateInteraction(object sender, System.EventArgs e)
    {
        if(!GameHandler.Instance.gameIsPlaying()) return; // we don't want player to interact when not playing

        if(selectedCounter != null){
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e){
        if(!GameHandler.Instance.gameIsPlaying()) return;

        if(selectedCounter != null){
            selectedCounter.Interact(this);
        }
    }
    private void Update() {
        HandleMovement();
        HandleInteractions();     
       
    }

    public bool Is_Walking(){
        return isWalking;
    }

    private void HandleMovement(){
        Vector2 inputVector2 = gameInput.GameInputVector2Normalized();
        Vector3 moveDir = new Vector3(inputVector2.x, 0f, inputVector2.y);

        //collision detection
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove  = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if(!canMove){
            //move along X axis
            Vector3 moveDir_X = new Vector3(moveDir.x, 0, 0).normalized;
            canMove  = (moveDir.x < -.6f || moveDir.x > +.6f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir_X, moveDistance);
            if(canMove){
                moveDir = moveDir_X;
            } else{

                //move along z axis
                Vector3 moveDir_Z = new Vector3(0, 0, moveDir.z).normalized;
                canMove  = (moveDir.z < -.6f || moveDir.z > +.6f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir_Z, moveDistance);
                if(canMove){
                    moveDir = moveDir_Z;
                }
            }
        }
        
        if(canMove){
            transform.position += moveDir * moveDistance;
        }
        
        isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, -moveDir, (Time.deltaTime * rotateSpeed));
    }
    
    private void HandleInteractions(){
        Vector2 inputVector2 = gameInput.GameInputVector2Normalized();
        Vector3 moveDir = new Vector3(inputVector2.x, 0f, inputVector2.y);

        //saving last direction for raycast if we stop without this we will loose our raycast
        if(moveDir != Vector3.zero){
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit rayCastHit, interactDistance, counterLayers)){
            if (rayCastHit.transform.TryGetComponent(out BaseCounter baseCounter)){
                if(baseCounter != selectedCounter){
                    setselectedcounter(baseCounter);
                }
            }else{
                setselectedcounter(null);
            }
        }else{
            setselectedcounter(null);
        }   
        
    }

    private void setselectedcounter(BaseCounter selectedCounter){

        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
            selectedCounter = selectedCounter
         });
    }

    public Transform GetKitchenObjectFollowTransform(){
        return kitchenObjectHoldPoint;
    }
    public bool HasKitchenObject(){
        return kitchenObject != null;
    }
    public void SetKitchenObject(KitchenObject kitchenObject){
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null){
            OnPickUpSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject(){
        return kitchenObject;
    }
    public void ClearKitchenObject(){
        kitchenObject = null;
    }
}
