using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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
            Debug.Log("true");
        }
    }

    public void OnAttack_K(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerAnimator.SetBool("attack_K", true);
            Debug.Log("true");
        }
    }

    public void OnAttack_R(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerAnimator.SetBool("attack_R", true);
            Debug.Log("true");
        }
    }


}
