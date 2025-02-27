using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossAI : MonoBehaviour
{
    // プレイヤー感知エリアの四点の座標
    [SerializeField] private Vector3 areaPoint1;
    [SerializeField] private Vector3 areaPoint2;
    [SerializeField] private Vector3 areaPoint3;
    [SerializeField] private Vector3 areaPoint4;
    // basic攻撃コライダー
    [SerializeField] private Collider basicCollider;
    // tail攻撃コライダー
    [SerializeField] private Collider tailCollider;
    // プレイヤーの移動速度
    [SerializeField] private float moveSpeed;
    // プレイヤーに近づく停止距離
    [SerializeField] private float StopDistance;
    // プレイヤーがエリア外に出てからwalkアニメーションを再生するまでの時間
    [SerializeField] private float timeToStartWalking;
    // 攻撃間隔（秒）
    [SerializeField] private float attackInterval;
    // 向き直す時間（秒）
    [SerializeField] private float reorientTime;
    // 攻撃後に向きを固定する時間（秒）
    [SerializeField] private float postAttackIdleTime;
    // ターゲットのタグ
    [SerializeField] private string TargetTag = "Player";
    // 無視するレイヤー
    [SerializeField] private string ignoreLayerName = "markar";
    // スライダーCanvas
    [SerializeField] private Canvas SliderCanvas;
    [SerializeField] private Animator EnemyAnimator;
    // エリア内のプレイヤーTransform
    private Transform playerTransform;
    private bool isPlayerInArea = false;
    private bool nearPlayer = false;
    private float timeSincePlayerLeft = 0f;
    private bool isReturningToInitialPosition = false;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool isIdle = false;
    private Quaternion fixedRotation;

    // 敵の初期位置を保存
    private Vector3 enemyInitialPosition;
    private Quaternion enemyInitialRotation;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーのTransformを取得
        GameObject player = GameObject.FindGameObjectWithTag(TargetTag);
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // 敵の初期位置と回転を保存
        enemyInitialPosition = transform.position;
        enemyInitialRotation = transform.rotation;

        // スライダーを非表示にする
        SliderCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーがエリア内にいるかどうかを判定
        isPlayerInArea = IsPlayerInArea();

        // プレイヤーがエリア内にいる場合
        if (isPlayerInArea && playerTransform != null)
        {
            // プレイヤーがエリア内にいるときにスライダーを表示
            SliderCanvas.gameObject.SetActive(true);

            // プレイヤーとの距離を計算
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // 攻撃中またはアイドル状態でない場合のみプレイヤーの方向を向く
            if (!isAttacking && !isIdle)
            {
                transform.LookAt(playerTransform);
            }

            // 攻撃中またはアイドル状態でない場合のみ移動
            if (!isAttacking && !isIdle)
            {
                // プレイヤーが停止距離より遠い場合
                if (distanceToPlayer > StopDistance)
                {
                    // プレイヤーに向かって進む
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                    nearPlayer = false;
                    EnemyAnimator.SetBool("getUp", true); // アニメーションを再生
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
            }

            // プレイヤーがエリア内にいる間はタイマーをリセット
            timeSincePlayerLeft = 0f;
            isReturningToInitialPosition = false;

            // 攻撃を行う
            if (canAttack)
            {
                PerformAttack();
                StartCoroutine(AttackCooldown());
            }
        }
        else
        {
            // プレイヤーがエリア外にいる場合
            timeSincePlayerLeft += Time.deltaTime;

            // プレイヤーがエリア外にいたらスライダーを非表示
            SliderCanvas.gameObject.SetActive(false);

            // プレイヤーがエリア外に出てから一定時間経過したらwalkアニメーションを再生
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
                    transform.rotation = enemyInitialRotation; // 初期回転に戻す
                    EnemyAnimator.SetBool("walk", false); // walkアニメーションを停止
                    EnemyAnimator.SetBool("getUp", false); // getUpアニメーションを停止
                    EnemyAnimator.SetBool("sleep", true); // Sleepアニメーションを再生
                    EnemyAnimator.SetBool("isSleep", true); // isSleepフラグをセット
                    isReturningToInitialPosition = false; // リセット
                }
            }
        }
    }

    // プレイヤーがエリアに入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag) && other.gameObject.layer != LayerMask.NameToLayer(ignoreLayerName))
        {
            isPlayerInArea = true;
            EnemyAnimator.SetBool("getUp", true); // アニメーションを再生
            EnemyAnimator.SetBool("sleep", false); // Sleepアニメーションを停止
            Debug.Log("Player entered the area");
        }
    }

    // プレイヤーがエリアから出たとき
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag) && other.gameObject.layer != LayerMask.NameToLayer(ignoreLayerName))
        {
            isPlayerInArea = false;
            nearPlayer = false;
            EnemyAnimator.SetBool("run", false); // アニメーションを停止
            timeSincePlayerLeft = 0f; // タイマーをリセット
            isReturningToInitialPosition = false; // フラグをリセット
            Debug.Log("Player exited the area");
        }
    }

    // プレイヤーがエリア内にいるかどうかを判定するメソッド
    private bool IsPlayerInArea()
    {
        if (playerTransform == null) return false;

        Vector3 playerPos = playerTransform.position;

        // 四点の座標で囲まれた領域内にプレイヤーがいるかどうかを判定
        bool isInside = IsPointInPolygon(playerPos, new Vector3[] { areaPoint1, areaPoint2, areaPoint3, areaPoint4 });
        return isInside;
    }

    // 点が多角形内にあるかどうかを判定するメソッド
    private bool IsPointInPolygon(Vector3 point, Vector3[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        float pointX = point.x, pointZ = point.z;
        float startX, startZ, endX, endZ;
        Vector3 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endZ = endPoint.z;
        while (i < polygonLength)
        {
            startX = endX; startZ = endZ;
            endPoint = polygon[i++];
            endX = endPoint.x; endZ = endPoint.z;
            inside ^= (endZ > pointZ ^ startZ > pointZ) && ((pointX - startX) < (endX - startX) * (pointZ - startZ) / (endZ - startZ));
        }
        return inside;
    }

    // 攻撃を行うメソッド
    private void PerformAttack()
    {
        bool isInBasicCollider = basicCollider.bounds.Contains(playerTransform.position);
        bool isInTailCollider = tailCollider.bounds.Contains(playerTransform.position);

        if (isInBasicCollider && isInTailCollider)
        {
            // 両方のコライダーに重なっている場合はランダムで攻撃
            int attackType = Random.Range(0, 2); // 0 or 1
            if (attackType == 0)
            {
                // basic攻撃
                EnemyAnimator.SetBool("basic", true);
                StartCoroutine(ResetAttack("basic"));
            }
            else
            {
                // tail攻撃
                EnemyAnimator.SetBool("tail", true);
                StartCoroutine(ResetAttack("tail"));
            }
        }
        else if (isInBasicCollider)
        {
            // basic攻撃
            EnemyAnimator.SetBool("basic", true);
            StartCoroutine(ResetAttack("basic"));
        }
        else if (isInTailCollider)
        {
            // tail攻撃
            EnemyAnimator.SetBool("tail", true);
            StartCoroutine(ResetAttack("tail"));
        }
    }

    // 攻撃をリセットするメソッド
    private IEnumerator ResetAttack(string attackType)
    {
        isAttacking = true;
        yield return new WaitForSeconds(1.0f); // アニメーションの長さに応じて調整
        EnemyAnimator.SetBool(attackType, false);
        isAttacking = false;

        // 向きとポジションを固定し、アイドル状態にする
        isIdle = true;
        fixedRotation = transform.rotation; // 現在の回転を保存
        EnemyAnimator.SetBool("idle", true); // アイドルアニメーションを再生
        EnemyAnimator.SetBool("run", false);
        EnemyAnimator.SetBool("walk", false);
        EnemyAnimator.SetBool("getUp", false);
        EnemyAnimator.SetBool("sleep", false);
        yield return new WaitForSeconds(postAttackIdleTime); // アイドル状態で待機
        isIdle = false;
        EnemyAnimator.SetBool("idle", false); // アイドルアニメーションを停止
    }

    // 攻撃のクールダウンを管理するメソッド
    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
    }
}