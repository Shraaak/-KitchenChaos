using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float MoveSpeed = 7f;
    [SerializeField] private float RotationSpeed = 10f;
    [SerializeField] private InputGame inputGame;
    [SerializeField] private LayerMask counterslayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private float move_distance;
    private float player_height = 1.5f;
    private float player_radius = 0.7f;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start()
    {
        inputGame.OnInteractAction += GameInput_OnInteractAction;
        inputGame.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        selectedCounter?.Interact(this);
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        selectedCounter?.InteractAlternate(this);
    }

    void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleMovement()
    {
        Vector3 moveDir = inputGame.GetMovementVectorNormalized();
        move_distance = Time.deltaTime * MoveSpeed;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player_height, player_radius, moveDir, move_distance);
        if (!canMove)
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0f);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player_height, player_radius, moveDirX, move_distance);
            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(moveDir.x, 0f, 0f);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * player_height, player_radius, moveDirZ, move_distance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * move_distance;
        }

        isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * RotationSpeed);

    }

    private void HandleInteractions()
    {
        Vector3 moveDir = inputGame.GetMovementVectorNormalized();

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDietance = 2f;

        Debug.DrawRay(transform.position, lastInteractDir * interactDietance, Color.red, 1f);
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDietance, counterslayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //HasClearCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }

    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if(kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);  
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
