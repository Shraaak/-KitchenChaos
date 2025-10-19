using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Player player;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
    }
    void Update()
    {
        animator.SetBool("IsWalking", player.IsWalking());
    }
}
