using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBossAtack : MonoBehaviour
{
    [SerializeField] Animator EnemyController; // エネミーのアニメーター
    // 噛みつき攻撃コライダー
    [SerializeField] Collider AttackBasicCollider;
    // しっぽ攻撃コライダー
    [SerializeField] Collider AttackTailCollider;
    [SerializeField] private string TargetTag = "Player"; // ターゲットのタグ
    [SerializeField] private float attackInterval; // 攻撃の間隔（秒）

    private bool canAttack = true; // 攻撃可能かどうかのフラグ

    private void Start()
    {
        // コライダーをトリガーに設定
        AttackBasicCollider.isTrigger = true;
        AttackTailCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ターゲットがエリアに入ったとき、攻撃可能であれば攻撃を実行
        if (other.CompareTag(TargetTag) && canAttack)
        {
            PerformRandomAttack();
        }
    }

    private void PerformRandomAttack()
    {
        // ランダムで攻撃タイプを選択
        int attackType = Random.Range(0, 2); // 0 or 1
        if (attackType == 0)
        {
            // 噛みつき攻撃
            EnemyController.SetBool("basic", true);
            StartCoroutine(ResetAttack("basic"));
        }
        else
        {
            // しっぽ攻撃
            EnemyController.SetBool("tail", true);
            StartCoroutine(ResetAttack("tail"));
        }
        // 攻撃クールダウンを開始
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator ResetAttack(string attackType)
    {
        // 一定時間待機してから攻撃をリセット
        yield return new WaitForSeconds(1.0f); // アニメーションの長さに応じて調整
        EnemyController.SetBool(attackType, false);
    }

    private IEnumerator AttackCooldown()
    {
        // 攻撃クールダウンを設定
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }
}