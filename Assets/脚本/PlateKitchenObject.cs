using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngreientAddedEventArgs> OnIngreientAdded;
    public class OnIngreientAddedEventArgs : EventArgs
    {
        public KictchenObjectSO kictchenObjectSO;
    }
    [SerializeField] private List<KictchenObjectSO> validKitchenObjectList;
    private List<KictchenObjectSO> kictchenObjectSOList;

    private void Awake()
    {
        kictchenObjectSOList = new List<KictchenObjectSO>();
    }

    public bool TryAddIngredient(KictchenObjectSO kictchenObjectSO)
    {
        if (!validKitchenObjectList.Contains(kictchenObjectSO))
        {
            return false;
        }
        if (kictchenObjectSOList.Contains(kictchenObjectSO))
        {
            //Already has this type
            return false;
        }
        else
        {
            kictchenObjectSOList.Add(kictchenObjectSO);

            OnIngreientAdded?.Invoke(this, new OnIngreientAddedEventArgs
            {
                kictchenObjectSO = kictchenObjectSO
            });

            return true;
        }
    }
    
    public List<KictchenObjectSO> GetKictchenObjectList()
    {
        return kictchenObjectSOList;
    }
}
