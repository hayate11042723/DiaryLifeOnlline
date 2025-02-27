using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBossAtack : MonoBehaviour
{
    [SerializeField] Animator EnemyController;
    // 噛みつき攻撃コライダー
    [SerializeField] Collider AttackBasicCollider;
    // しっぽ攻撃コライダー
    [SerializeField] Collider AttackTailCollider;
    [SerializeField] private string TargetTag = "Player";
    [SerializeField] private float attackInterval = 2.0f; // 攻撃の間隔（秒）

    private bool canAttack = true;

    private void Start()
    {
        AttackBasicCollider.isTrigger = true;
        AttackTailCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag) && canAttack)
        {
            PerformRandomAttack();
        }
    }

    private void PerformRandomAttack()
    {
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
        StartCoroutine(AttackCooldown());
    }

    private IEnumerator ResetAttack(string attackType)
    {
        yield return new WaitForSeconds(1.0f); // アニメーションの長さに応じて調整
        EnemyController.SetBool(attackType, false);
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }
}
