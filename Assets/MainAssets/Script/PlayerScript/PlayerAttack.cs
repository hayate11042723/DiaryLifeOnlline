using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Collider WeaponCollider;
    public SowrdEffect swordEffect; // SowrdEffectの参照を追加

    // 攻撃中かどうかのフラグ
    public bool isAttacking = false; // フラグをpublicに変更

    // アニメーションが変わらない時間を計測するタイマー
    private float animationTimer = 0f;
    private float animationThreshold = 3f; // 4秒の閾値
    private string currentAnimation = "idle";

    private void Update()
    {
        CheckAnimationState();
    }

    // 武器の当たり判定をオンにする
    void AttackFlagON()
    {
        WeaponCollider.enabled = true;

        // SowrdEffectのAttackFlagONメソッドを呼び出す
        if (swordEffect != null)
        {
            swordEffect.AttackFlagON();
        }
    }

    // 武器の当たり判定をオフにする
    void AttackFlagOFF()
    {
        WeaponCollider.enabled = false;

        // SowrdEffectのAttackFlagOFFメソッドを呼び出す
        if (swordEffect != null)
        {
            swordEffect.AttackFlagOFF();
        }

        // 攻撃アニメーション終了後にフラグをリセット
        isAttacking = false;

        // アニメーションフラグをリセット
        PlayerAnimator.SetBool("attack_I", false);
        PlayerAnimator.SetBool("attack_K", false);
        PlayerAnimator.SetBool("attack_R", false);

        // アニメーションをidleに戻す
        SetAnimation("idle");
    }

    // 攻撃（一撃）の入力処理
    public void OnAttack_I(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            SetAnimation("attack_I");
        }
    }

    // 攻撃（回転撃）の入力処理
    public void OnAttack_K(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            SetAnimation("attack_K");
        }
    }

    // 攻撃（連撃）の入力処理
    public void OnAttack_R(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            SetAnimation("attack_R");
        }
    }

    private void SetAnimation(string animationName)
    {
        if (currentAnimation != animationName)
        {
            PlayerAnimator.SetBool(currentAnimation, false);
            PlayerAnimator.SetBool(animationName, true);
            currentAnimation = animationName;
            animationTimer = 0f; // タイマーをリセット
        }
    }

    private void CheckAnimationState()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationThreshold)
        {
            // 一瞬だけアニメーションフラグをfalseにして元に戻す
            PlayerAnimator.SetBool(currentAnimation, false);
            StartCoroutine(ResetAnimationFlag());
            animationTimer = 0f; // タイマーをリセット
        }
    }

    private IEnumerator ResetAnimationFlag()
    {
        yield return null; // 1フレーム待つ
        PlayerAnimator.SetBool(currentAnimation, true);
    }
}
