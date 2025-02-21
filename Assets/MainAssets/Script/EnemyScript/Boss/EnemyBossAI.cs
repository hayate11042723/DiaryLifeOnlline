using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAI : MonoBehaviour
{
    // プレイヤー感知エリア
    [SerializeField] public Collider GreenDragonArea;
    // プレイヤーの移動速度
    [SerializeField] private float moveSpeed;
    // プレイヤーに近づく停止距離
    [SerializeField] private float StopDistance;
    // プレイヤーがエリア外に出てからwalkアニメーションを再生するまでの時間
    [SerializeField] private float timeToStartWalking = 3f;
    // エリア内のプレイヤーTransform
    private Transform playerTransform;
    // ターゲットのタグ
    [SerializeField] private string TargetTag = "Player";

    [SerializeField] private Animator EnemyAnimator;
    private bool isPlayerInArea = false;
    private bool nearPlayer = false;
    private float timeSincePlayerLeft = 0f;
    private bool isReturningToInitialPosition = false;

    // GreenDragonAreaの初期位置を保存
    private Vector3 initialPosition;
    // 敵の初期位置を保存
    private Vector3 enemyInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのTransformを取得
        GameObject player = GameObject.FindGameObjectWithTag(TargetTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // GreenDragonAreaの設定を確認
        if (GreenDragonArea != null && GreenDragonArea.isTrigger)
        {
            // GreenDragonAreaの初期位置を保存
            initialPosition = GreenDragonArea.transform.position;
        }

        // 敵の初期位置を保存
        enemyInitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // GreenDragonAreaの位置を固定
        if (GreenDragonArea != null)
        {
            GreenDragonArea.transform.position = initialPosition;
        }

        // プレイヤーがエリア内にいる場合
        if (isPlayerInArea && playerTransform != null)
        {
            // プレイヤーとの距離を計算
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // プレイヤーの方向を向く
            transform.LookAt(playerTransform);

            // プレイヤーが停止距離より遠い場合
            if (distanceToPlayer > StopDistance)
            {
                // プレイヤーに向かって進む
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
                nearPlayer = false;
                EnemyAnimator.SetBool("run", true); // アニメーションを再生
                EnemyAnimator.SetBool("walk", false); // walkアニメーションを停止
            }
            else
            {
                // プレイヤーが停止距離内にいる場合
                if (!nearPlayer)
                {
                    nearPlayer = true;
                    EnemyAnimator.SetBool("run", false); // アニメーションを停止
                }
            }
            // プレイヤーがエリア内にいる間はタイマーをリセット
            timeSincePlayerLeft = 0f;
            isReturningToInitialPosition = false;
        }
        else
        {
            // プレイヤーがエリア外にいる場合
            timeSincePlayerLeft += Time.deltaTime;

            if (timeSincePlayerLeft >= timeToStartWalking && !isReturningToInitialPosition)
            {
                // walkアニメーションを再生し、元の位置に戻り始める
                EnemyAnimator.SetBool("walk", true); // walkアニメーションを再生
                isReturningToInitialPosition = true;
            }

            if (isReturningToInitialPosition)
            {
                // 元の位置に戻る
                float distanceToInitial = Vector3.Distance(transform.position, enemyInitialPosition);
                if (distanceToInitial > 0.1f)
                {
                    // 元の位置に向かって進む
                    Vector3 directionToInitial = (enemyInitialPosition - transform.position).normalized;
                    transform.position += directionToInitial * moveSpeed * Time.deltaTime;

                    // 進行方向を向く
                    transform.rotation = Quaternion.LookRotation(directionToInitial);

                    EnemyAnimator.SetBool("run", false); // runアニメーションを停止
                }
                else
                {
                    // 元の位置に到着したらSleepアニメーションを再生
                    EnemyAnimator.SetBool("walk", false); // walkアニメーションを停止
                    EnemyAnimator.SetBool("getUp", false); // getUpアニメーションを停止
                    EnemyAnimator.SetBool("sleep", true); // Sleepアニメーションを再生
                    isReturningToInitialPosition = false; // リセット
                }
            }
        }
    }

    // プレイヤーがエリアに入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            isPlayerInArea = true;
            EnemyAnimator.SetBool("getUp", true); // アニメーションを再生
            EnemyAnimator.SetBool("sleep", false); // Sleepアニメーションを停止
        }
    }

    // プレイヤーがエリアから出たとき
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            isPlayerInArea = false;
            nearPlayer = false;
            EnemyAnimator.SetBool("run", false); // アニメーションを停止
        }
    }
}


