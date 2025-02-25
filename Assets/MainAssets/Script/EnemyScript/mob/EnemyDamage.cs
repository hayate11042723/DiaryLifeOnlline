using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IDamagebleのインターフェースの継承を行う
public class EnemyDamage : MonoBehaviour, IDamageable
{
    //シリアル化している。charadataの各エネミーのStatusを指定。
    [SerializeField] private CharaStatus charadata;
    //シリアル化している。playerdataの各エネミーのStatusを指定。
    [SerializeField] private PlayerStatus playerdata;
    // シリアル化している。Lvdataの参照を指定。
    [SerializeField] private Lvdata lvdata;
    //シリアル化。SliderのHPゲージ指定
    [SerializeField] Slider Slider;
    public int HP;
    int enemyDamage;
    public Animator EnemyAnimator;
    public GameObject Effect;

    //シリアル化。ステータスポイントの指定
    [SerializeField] private int statusPoint;
    //シリアル化。経験値の増量値の指定
    [SerializeField] private int maxexp;

    void Start()
    {
        //charadataがnullでないことを確認
        if (charadata != null)
        {
            // valueのHPゲージのスライダーの最大の1に
            Slider.value = 1;

            //charadataの最大HPを代入。
            HP = charadata.MAXHP;

            // スライダーを非表示にする
            Slider.gameObject.SetActive(false);
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
            if (enemyDamage <= 0)
            {
                enemyDamage = 1;
            }
            // HPから算出されたダメージを引く
            HP -= enemyDamage;
            // HPゲージに反映
            Slider.value = (float)HP / (float)charadata.MAXHP;

            // HPが減ったらスライダーを表示する
            if (!Slider.gameObject.activeSelf)
            {
                Slider.gameObject.SetActive(true);
            }

            // HPが減った時にアニメーションフラグをTRUEにする
            EnemyAnimator.SetBool("hit", true);

            // アニメーションが再生し終わったらフラグをFALSEに戻す
            StartCoroutine(ResetHitFlag());
        }

        // HPが0以下ならDeathアニメーションを再生
        if (HP <= 0)
        {
            // Deathフラグをセットしてアニメーションを再生
            EnemyAnimator.SetBool("death", true);

            // コルーチンを開始してアニメーション終了後にDeath()を実行
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    // アニメーション終了後に死亡処理を行うコルーチン
    private IEnumerator WaitForDeathAnimation()
    {
        // アニメーションが終了するまで待つ
        yield return new WaitForSeconds(EnemyAnimator.GetCurrentAnimatorStateInfo(0).length);

        // 死亡処理
        Death();
    }

    // アニメーション終了後にhitフラグをFALSEに戻すコルーチン
    private IEnumerator ResetHitFlag()
    {
        // アニメーションが終了するまで待つ
        yield return new WaitForSeconds(EnemyAnimator.GetCurrentAnimatorStateInfo(0).length);

        // hitフラグをFALSEに戻す
        EnemyAnimator.SetBool("hit", false);
    }

    // 死亡処理のメソッド
    public void Death()
    {
        // 消滅エフェクト
        var effect = Instantiate(Effect);
        effect.transform.position = gameObject.transform.position;

        //獲得経験値があるなら経験値処理
        if (charadata.GETEXP > 0)
        {

            playerdata.EXP = playerdata.EXP + charadata.GETEXP;

            var a = lvdata.playerExpTable[playerdata.LV];

            if (playerdata.EXP >= a.exp)
            {
                playerdata.LV += 1;
                playerdata.StatusPoint += statusPoint;
                playerdata.EXP = 0;
                playerdata.MAXEXP *= maxexp;
            }
        }
        
        //獲得ゴールドがあるならゴールド処理
        if (charadata.GETGOLD > 0)
        {
            playerdata.HAVEGOLD = playerdata.HAVEGOLD + charadata.GETGOLD;
        }

        // ゲームオブジェクトを破壊
        Destroy(gameObject);
        Destroy(effect, 5);
    }
}

