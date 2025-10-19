using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEvenArgs> OnProgressChanged;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningReciprSO[] burningRecipeSOArray;


    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;
    private float burnTimer;
    private BurningReciprSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingtimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingtimerMax)
                    {
                        //Fried
                        fryingTimer = 0f;

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);


                        state = State.Fried;
                        burnTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOwithInput(GetKitchenObject().GetKictchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                    }
                    break;
                case State.Fried:
                    burnTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
                    {
                        progressNormalized = burnTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burnTimer > burningRecipeSO.burningTimerMax)
                    {
                        //Fried
                        burnTimer = 0f;

                        GetKitchenObject().DestroySelf();

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state = state
                        });

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
                        {
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
        if (!HasKitchenObject())
        {
            // There is no KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKictchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOwithInput(GetKitchenObject().GetKictchenObjectSO());

                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingtimerMax
                    });
                }

            }
            else
            {
                //Player has nothing
            }
        }
        else
        {
            // There is a KitchenObject here
            if (player.HasKitchenObject())
            {
                // Player is carrying something 
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKictchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                    state = State.Idle;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state = state
                    });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
                    {
                        progressNormalized = 0f
                    });

                }
            }
            else
            {
                //Player has nothing
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state = state
                });

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEvenArgs
                {
                    progressNormalized = 0f
                });
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        print("YES");
    }
    private bool HasRecipeWithInput(KictchenObjectSO inputKictchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOwithInput(inputKictchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KictchenObjectSO GetOutputForInput(KictchenObjectSO inputKictchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOwithInput(inputKictchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOwithInput(KictchenObjectSO inputkictchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputkictchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningReciprSO GetBurningRecipeSOwithInput(KictchenObjectSO inputkictchenObjectSO)
    {
        foreach (BurningReciprSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputkictchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return state == State.Fried;
    }
}
