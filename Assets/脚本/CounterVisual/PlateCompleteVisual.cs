using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KictchenObjectSO_GameObject
    {
        public KictchenObjectSO kictchenObjectSO;
        public GameObject gameObject;

    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KictchenObjectSO_GameObject> kictchenObjectSOGameObjectsList;

    void Start()
    {
        plateKitchenObject.OnIngreientAdded += PlateKitchenObject_OnIngreientAdded;
        
        foreach (KictchenObjectSO_GameObject kictchenObjectSOGameObject in kictchenObjectSOGameObjectsList)
        {
            kictchenObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngreientAdded(object sender, PlateKitchenObject.OnIngreientAddedEventArgs e)
    {
        foreach (KictchenObjectSO_GameObject kictchenObjectSOGameObject in kictchenObjectSOGameObjectsList)
        {
            if (kictchenObjectSOGameObject.kictchenObjectSO == e.kictchenObjectSO)
            {
                kictchenObjectSOGameObject.gameObject.SetActive(true);
            }
        }
        
    }
}
