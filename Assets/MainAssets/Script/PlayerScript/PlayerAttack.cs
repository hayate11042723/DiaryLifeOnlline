using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Collider WeaponCollider;
    public SowrdEffect swordEffect; // SowrdEffectの参照を追加
    public float detectionRadius = 5f; // 敵を検出する半径
    public Transform lockedOnEnemy; // ロックオンされたエネミーを保持する変数

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
            FaceTarget(); // ターゲットに向く
            SetAnimation("attack_I");
        }
    }

    // 攻撃（回転撃）の入力処理
    public void OnAttack_K(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            FaceTarget(); // ターゲットに向く
            SetAnimation("attack_K");
        }
    }

    // 攻撃（連撃）の入力処理
    public void OnAttack_R(InputAction.CallbackContext context)
    {
        if (context.started && !isAttacking) // 攻撃中でなければ実行
        {
            isAttacking = true; // 攻撃中フラグを設定
            FaceTarget(); // ターゲットに向く
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

    // ターゲットに向くメソッド
    private void FaceTarget()
    {
        if (lockedOnEnemy != null)
        {
            Vector3 direction = (lockedOnEnemy.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation; // 一気に向く
        }
        else
        {
            FaceNearestEnemy(); // ロックオンされていない場合は近くの敵に向く
        }
    }

    // 近くの敵に向くメソッド
    private void FaceNearestEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        Collider nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = hitCollider;
                }
            }
        }

        if (nearestEnemy != null)
        {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation; // 一気に向く
        }
    }
}
