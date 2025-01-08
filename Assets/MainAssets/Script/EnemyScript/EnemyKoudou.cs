using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyKoudou : MonoBehaviour, IEnemy
{
    //シリアル化している。CharaStatusの敵を指定。
    [SerializeField] CharaStatus charadata;
    private GameObject Player;
    Vector3 distance; //Playerとの距離
    int State;

    void Update()
    {
        FindClosestPlayer();
    }

    void FindClosestPlayer()
    {
        // タグ "Player" を持つ全てのオブジェクトを取得
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        float shortestDistance = Mathf.Infinity;
        GameObject nearestPlayer = null;

        // 各Playerとの距離を計算して最短距離を求める
        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPlayer = player;
            }
        }

        Player = nearestPlayer;
    }

    public int EnemyAIkoudou()
    {
        EnemyDamage enemyDamage = GetComponent<EnemyDamage>();

        //キャラ距離計算の処理
        //敵の位置からPlayerの位置を引いた後にMathf.Absで絶対値を出すことで距離がわかる。

        distance = transform.position - Player.transform.position;

        float distanceX = Mathf.Abs(distance.x);
        float distanceZ = Mathf.Abs(distance.z);

        // Debug.Log(Player.transform.position);
        // Debug.Log(distanceX);
        // Debug.Log(distanceZ);
        // Debug.Log(charadata.ShortAttackRange);

        //X座標とZ座標の距離のどちらが大きいか調べ、大きいほうの距離が敵のShortAttackRange以下であればStateを1として返す。攻撃を行う。
        if (charadata.MAXHP > enemyDamage.HP)
        {
        //    Debug.Log("減少");
            if (distanceX > distanceZ)
            {
                Debug.Log("通");
                if (charadata.ShortAttackRange >= distanceX)
                {
                    Debug.Log("通");
                    State = 1;
                    Debug.Log(State);
                    return State;
                }
            }
            else if(distanceX < distanceZ)
            {
                if (charadata.ShortAttackRange >= distanceZ)
                {
                    State = 1;
                    return State;
                }
            }
            else
            {
                State = 0;
                return State;
            }
        }

        //条件を満たさない場合はStateを0として返す。何もしない。
        State = 0;
        return State;
    }
}