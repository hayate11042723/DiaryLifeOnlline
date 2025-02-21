using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    float Timer;
    [SerializeField] private float ChangeTime;
    [SerializeField] private float EnemySpeed;

    [SerializeField] private float StopDistance; // プレイヤーに近づく停止距離

    [SerializeField] private string TargetTag = "Player"; // ターゲットのタグ
    private GameObject targetObject; // ターゲットのGameObject

    [SerializeField] private Animator EnemyAnimator;

    // Update is called once per frame
    void Update()
    {
        var speed = Vector3.zero;
        var rot = transform.eulerAngles;

        if (targetObject) // プレイヤーを追いかける
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetObject.transform.position);

            transform.LookAt(targetObject.transform); // 常にプレイヤーの方向を向く
            rot = transform.eulerAngles;

            if (distanceToPlayer > StopDistance) // 停止距離より遠い場合のみ移動
            {
                speed.z = EnemySpeed; // プレイヤー方向に進む
            }
            else
            {
                speed = Vector3.zero; // 停止
            }
        }
        else // ランダム移動
        {
            Timer += Time.deltaTime;
            if (Timer >= ChangeTime)
            {
                float rand = Random.Range(0, 360); // ランダムな方向に回転
                rot.y = rand;
                Timer = 0;
            }

            speed.z = EnemySpeed * 0.5f; // ランダム移動時の速度（半分に調整）
        }

        rot.x = 0;
        rot.z = 0;
        transform.eulerAngles = rot;

        this.transform.Translate(speed * Time.deltaTime, Space.Self); // フレームごとの速度を反映
    }

    // コライダーにプレイヤーが入ったとき
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            targetObject = other.gameObject;
            EnemyAnimator.SetBool("run", true); // アニメーションを再生
        }
    }

    // コライダーからプレイヤーが出たとき
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            targetObject = null;
            EnemyAnimator.SetBool("run", false); // アニメーションを停止
        }
    }
}

