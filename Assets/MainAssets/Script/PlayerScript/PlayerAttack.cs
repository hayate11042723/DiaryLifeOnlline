using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Collider WeaponCollider;

    // 攻撃中かどうかのフラグ
    private bool isAttacking = false;

    // 武器の当たり判定をオンにする
    void AttackFlagON()
    {
        WeaponCollider.enabled = true;
        Debug.Log("WeaponON");
    }

    // 武器の当たり判定をオフにする
    void AttackFlagOFF()
    {
        WeaponCollider.enabled = false;
        Debug.Log("WeaponOFF");

        // 攻撃アニメーション終了後にフラグをリセット
        isAttacking = false;

        // アニメーションフラグをリセット
        PlayerAnimator.SetBool("attack_I", false);
        PlayerAnimator.SetBool("attack_K", false);
        PlayerAnimator.SetBool("attack_R", false);
    }

    // 攻撃（一撃）の入力処理
    public void OnAttack_I(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            PlayerAnimator.SetBool("attack_I", true);
        }
    }

    // 攻撃（回転撃）の入力処理
    public void OnAttack_K(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            PlayerAnimator.SetBool("attack_K", true);
        }
    }

    // 攻撃（連撃）の入力処理
    public void OnAttack_R(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            PlayerAnimator.SetBool("attack_R", true);
        }
    }
}
