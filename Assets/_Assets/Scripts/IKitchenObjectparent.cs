using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectparent
{
    public Transform GetKitchenObjectFollowTransform();

    public bool HasKitchenObject();

    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();
}
