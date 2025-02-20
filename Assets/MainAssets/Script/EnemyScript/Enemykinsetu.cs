using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemykinsetu : MonoBehaviour, ICharaAttack
{
    [SerializeField] Animator EnemyController;
    [SerializeField] Collider AttackCollider;
    int Hcount;   //攻撃ヒット回数
    bool Attacktime;   //攻撃中

    public int HitCount()
    {
        //現在の残りヒット数を返す。
        return Hcount;
    }

    public void HitCountdown()
    {
        //ダメージが入ることが確定した際に残りヒット数を減らす。
        --Hcount;
    }
    public bool Attacktimekanshi()
    {
        //攻撃中か攻撃中でないかを返す。
        return Attacktime;
    }

    void Start()
    {
        EnemyController = GetComponent<Animator>();
    }

    void AttackStart()
    {
        AttackCollider.enabled = true;
        Hcount = 1;
        Attacktime = true;
    }

    void Hit()
    {
        Hcount = 0;
        Attacktime = false;
    }

    void AttackEnd()
    {
        AttackCollider.enabled = false;
        //アニメーション終了 Attackパラメータを0にする。
        EnemyController.SetInteger("attack", 0);
    }
}
