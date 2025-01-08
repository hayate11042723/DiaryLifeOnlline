using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IDamagebleのインターフェースの継承を行う
public class PlayerDamage : MonoBehaviour, IDamageable
{
    //シリアル化している。charadataのMazokusoldierを指定。
    [SerializeField] private CharaStatus charadata;
    //シリアル化。SliderのHPゲージ指定
    [SerializeField] Slider Slider;
    int HP;
    int playerDamage;
    public Animator PlayerAnimator;

    void Start()
    {
        //charadataがnullでないことを確認
        if (charadata != null)
        {
            // valueのHPゲージのスライダーの最大の1に
            Slider.value = 1;

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
            playerDamage = value - charadata.DEF;
            // ダメージ量が0以下になったら1ダメージにする
            if (playerDamage <= 0)
            {
                playerDamage = 1;
            }
            // HPから算出されたダメージを引く
            HP -= playerDamage;
            // HPゲージに反映
            Slider.value = (float)HP / (float)charadata.MAXHP;
        }


        // HPが0以下ならDeathアニメーションを再生
        if (HP <= 0)
        {
            Debug.Log("死");
            // Deathフラグをセットしてアニメーションを再生
            PlayerAnimator.SetBool("death", true);

            // コルーチンを開始してアニメーション終了後にDeath()を実行
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    // アニメーション終了後に死亡処理を行うコルーチン
    private IEnumerator WaitForDeathAnimation()
    {
        // アニメーションが終了するまで待つ
        yield return new WaitForSeconds(PlayerAnimator.GetCurrentAnimatorStateInfo(0).length);

        // 死亡処理
        Death();
    }

    // 死亡処理のメソッド
    public void Death()
    {
        // ゲームオブジェクトを破壊
        Destroy(gameObject);
    }
}
