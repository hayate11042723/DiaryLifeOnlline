using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePlayerMove : MonoBehaviour
{
    public float speed = 5.0f; // プレイヤーの移動速度
    public GameObject enemy1; // エネミー1のゲームオブジェクト
    public GameObject enemy2; // エネミー2のゲームオブジェクト
    public float enemySpeed1 = 4.0f; // エネミー1の移動速度
    public float enemySpeed2 = 3.5f; // エネミー2の移動速度
    public Vector3 respawnPosition = new Vector3(10.0f, 0.0f, 0.0f); // 再出現する位置
    public Vector3 despawnPosition = new Vector3(-10.0f, 0.0f, 0.0f); // 消える位置
    public Vector3 moveDirection = Vector3.left; // プレイヤーの移動する向き
    public Vector3 enemyMoveDirection1 = Vector3.left; // エネミー1の移動する向き
    public Vector3 enemyMoveDirection2 = Vector3.left; // エネミー2の移動する向き
    public Quaternion leftRotation = Quaternion.Euler(0, 0, 0); // 左向きの回転
    public Quaternion rightRotation = Quaternion.Euler(0, 180, 0); // 右向きの回転

    private Vector3 initialPlayerPosition; // プレイヤーの初期位置
    private Vector3 initialEnemyPosition1; // エネミー1の初期位置
    private Vector3 initialEnemyPosition2; // エネミー2の初期位置
    private Animator playerAnimator; // プレイヤーのアニメーター
    private Animator enemyAnimator1; // エネミー1のアニメーター
    private Animator enemyAnimator2; // エネミー2のアニメーター

    // Start is called before the first frame update
    void Start()
    {
        // エネミーが設定されていない場合はエラーメッセージを表示
        if (enemy1 == null || enemy2 == null)
        {
            Debug.LogError("Enemy GameObject is not assigned.");
            return;
        }

        // プレイヤーとエネミーの初期位置を保存
        initialPlayerPosition = transform.position;
        initialEnemyPosition1 = enemy1.transform.position;
        initialEnemyPosition2 = enemy2.transform.position;

        // プレイヤーとエネミーのアニメーターを取得
        playerAnimator = GetComponent<Animator>();
        enemyAnimator1 = enemy1.GetComponent<Animator>();
        enemyAnimator2 = enemy2.GetComponent<Animator>();

        // プレイヤーのアニメーターが設定されていない場合はエラーメッセージを表示
        if (playerAnimator == null)
        {
            Debug.LogError("Player Animator is not assigned.");
        }

        // エネミーのアニメーターが設定されていない場合はエラーメッセージを表示
        if (enemyAnimator1 == null)
        {
            Debug.LogError("Enemy1 Animator is not assigned.");
        }
        if (enemyAnimator2 == null)
        {
            Debug.LogError("Enemy2 Animator is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // プレイヤーとエネミーを移動させる
        MovePlayer();
        MoveEnemy1();
        MoveEnemy2();
        // プレイヤーとエネミーの折り返しをチェック
        CheckBoundaries();
    }

    // プレイヤーを移動させる
    void MovePlayer()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        // プレイヤーが移動しているときにアニメーションを再生
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("run", true);
        }
        // プレイヤーが移動する方向を向く
        if (moveDirection.x > 0)
        {
            transform.rotation = rightRotation;
        }
        else if (moveDirection.x < 0)
        {
            transform.rotation = leftRotation;
        }
    }

    // エネミー1を移動させる
    void MoveEnemy1()
    {
        enemy1.transform.Translate(enemyMoveDirection1 * enemySpeed1 * Time.deltaTime, Space.World);
        // エネミー1が移動しているときにアニメーションを再生
        if (enemyAnimator1 != null)
        {
            enemyAnimator1.SetBool("run", true);
        }
        // エネミー1が移動する方向を向く
        if (enemyMoveDirection1.x > 0)
        {
            enemy1.transform.rotation = rightRotation;
        }
        else if (enemyMoveDirection1.x < 0)
        {
            enemy1.transform.rotation = leftRotation;
        }
    }

    // エネミー2を移動させる
    void MoveEnemy2()
    {
        enemy2.transform.Translate(enemyMoveDirection2 * enemySpeed2 * Time.deltaTime, Space.World);
        // エネミー2が移動しているときにアニメーションを再生
        if (enemyAnimator2 != null)
        {
            enemyAnimator2.SetBool("run", true);
        }
        // エネミー2が移動する方向を向く
        if (enemyMoveDirection2.x > 0)
        {
            enemy2.transform.rotation = rightRotation;
        }
        else if (enemyMoveDirection2.x < 0)
        {
            enemy2.transform.rotation = leftRotation;
        }
    }

    // プレイヤーとエネミーが境界に達した場合に折り返す
    void CheckBoundaries()
    {
        if (transform.position.x < despawnPosition.x || transform.position.x > respawnPosition.x)
        {
            moveDirection = -moveDirection; // プレイヤーの移動方向を反転
        }

        if (enemy1.transform.position.x < despawnPosition.x || enemy1.transform.position.x > respawnPosition.x)
        {
            enemyMoveDirection1 = -enemyMoveDirection1; // エネミー1の移動方向を反転
        }

        if (enemy2.transform.position.x < despawnPosition.x || enemy2.transform.position.x > respawnPosition.x)
        {
            enemyMoveDirection2 = -enemyMoveDirection2; // エネミー2の移動方向を反転
        }
    }
}

