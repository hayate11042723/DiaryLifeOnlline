using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IDamagebleのインターフェースの継承を行う
public class EnemyDamage : MonoBehaviour, IDamageable
{
    //シリアル化している。charadataのMazokusoldierを指定。
    [SerializeField] private CharaStatus charadata;
    //シリアル化。SliderのHPゲージ指定
    [SerializeField] Slider Slider;
    int HP;
    int enemyDamage;
    public Animator EnemyAnimator;

    void Start()
    {
        //charadataがnullでないことを確認
        if (charadata != null)
        {
            // valueのHPゲージのスライダーの最大の1に
            Slider.value = 10;

            //charadataの最大HPを代入。
            HP = charadata.MAXHP;
        }
    }

    // ダメージ処理のメソッド　valueにはPlayer1のATKの値が入ってる
    public void Damage(int value)
    {

        // charadataがnullでないかをチェック
        if (charadata != null)
        {
            // PlayerのATKからEnemyのDEFを引いた値からダメージを算出
            enemyDamage = value - charadata.DEF;
            // ダメージ量が0以下になったら1ダメージにする
            if(enemyDamage <= 0)
            {
                enemyDamage = 1;
            }
            // HPから算出されたダメージを引く
            HP -= enemyDamage;
        }


        // HPが0以下ならDeath()メソッドを呼び出す。
        if (HP <= 0)
        {
            EnemyAnimator.SetBool("death", true);
            Death();
        }
    }
    // 死亡処理のメソッド
    public void Death()
    {
        // ゲームオブジェクトを破壊
        Destroy(gameObject);
    }
}
