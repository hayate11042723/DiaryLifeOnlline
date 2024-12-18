using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyKoudou : MonoBehaviour, IEnemy
{
    //シリアル化している。CharaStatusの敵を指定。
    [SerializeField] CharaStatus charadata;
    [SerializeField] GameObject Player;
    Vector3 distance; //Playerとの距離
    int State;

    public int EnemyAIkoudou()
    {
        EnemyDamage enemyDamage = GetComponent<EnemyDamage>();

        //キャラ距離計算の処理
        //敵の位置からPlayerの位置を引いた後にMathf.Absで絶対値を出すことで距離がわかる。

        distance = transform.position - Player.transform.position;

        float distanceX = Mathf.Abs(distance.x);
        float distanceZ = Mathf.Abs(distance.z);



        //X座標とZ座標の距離のどちらが大きいか調べ、大きいほうの距離が敵のShortAttackRange以下であればStateを1として返す。攻撃を行う。
        if(charadata.MAXHP > enemyDamage.HP)
        {
            if (distanceX > distanceZ)
            {
                if (charadata.ShortAttackRange >= distanceX)
                {

                    State = 1;

                    return State;
                }
            }
            else
            {
                if (charadata.ShortAttackRange >= distanceZ)
                {
                    State = 1;
                    return State;

                }
            }

        }

        //条件を満たさない場合はStateを0として返す。何もしない。
        State = 0;
        return State;

    }
}