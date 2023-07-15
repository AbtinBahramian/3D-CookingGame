using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Player player;
    const string IS_Walking = "IsWalking";
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void Update() {
        animator.SetBool(IS_Walking, player.Is_Walking());
    }
}
