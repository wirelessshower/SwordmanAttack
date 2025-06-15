using System;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviour
{
    [SerializeField] private string targetTag = "Targget"; 
    [SerializeField] private float attackCooldown = 0.5f; 

    private bool canAttack = true;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(targetTag))
            TryAttack();        
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(targetTag))
            TryAttack();
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(targetTag))
            animator.SetLayerWeight(1, 0);
    }

    private void TryAttack() {
        if (canAttack) {
            animator.SetLayerWeight(1, 1);
            canAttack = false;
            Invoke(nameof(ResetAttackCooldown), attackCooldown);
        }
    }


    private void ResetAttackCooldown() { 
        canAttack = true;
       
    }
}
