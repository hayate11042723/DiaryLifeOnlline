using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Collider WeaponCollider;

    void AttackFlagON()
    {
        WeaponCollider.enabled = true;
    }
    
    void AttackFlagOFF()
    {
        WeaponCollider.enabled = false;
        PlayerAnimator.SetBool("attack_I", false);
        PlayerAnimator.SetBool("attack_K", false);
        PlayerAnimator.SetBool("attack_R", false);
    }

    public void OnAttack_I(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerAnimator.SetBool("attack_I",true);
        }
    }

    public void OnAttack_K(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerAnimator.SetBool("attack_K", true);
        }
    }

    public void OnAttack_R(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerAnimator.SetBool("attack_R", true);
        }
    }


}
