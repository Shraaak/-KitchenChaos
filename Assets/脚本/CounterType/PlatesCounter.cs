using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KictchenObjectSO plateKictchenObjectSO;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawneAmount;
    private int platesSpawneAmountMax = 4;

    private void Update()
    {
        if (platesSpawneAmount < platesSpawneAmountMax)
        {
            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateTimer >= spawnPlateTimerMax)
            {
                spawnPlateTimer = 0f;
                
                platesSpawneAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesSpawneAmount > 0)
            {
                platesSpawneAmount--;

                KitchenObject.SpawnKitchenObject(plateKictchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);

            }
        }
    }
}
