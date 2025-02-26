using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// IDamagebleのインターフェースの継承を行う
public class EnemyDamageBoss : MonoBehaviour, IDamageable
{
    //シリアル化している。charadataの各エネミーのStatusを指定。
    [SerializeField] private CharaStatus charadata;
    //シリアル化している。playerdataの各エネミーのStatusを指定。
    [SerializeField] private PlayerStatus playerdata;
    // シリアル化している。Lvdataの参照を指定。
    [SerializeField] private Lvdata lvdata;
    //シリアル化。SliderのHPゲージ指定
    [SerializeField] Slider Slider;
    [SerializeField] int HP;
    [SerializeField] int DestroyTime;
    int enemyDamage;
    [SerializeField] Animator EnemyAnimator;
    [SerializeField] GameObject Effect;

    //シリアル化。ステータスポイントの指定
    [SerializeField] private int statusPoint;
    //シリアル化。経験値の増量値の指定
    [SerializeField] private int maxexp;

    // シリアル化。hitアニメーションをトリガーするダメージ量の指定
    [SerializeField] private int hitTriggerDamage;

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

    // HPを全回復するメソッド
    public void isSleep()
    {
        HP = charadata.MAXHP;
        Slider.value = 1;
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

            // 特定のダメージ量に達した場合にアニメーションフラグをTRUEにする
            if (enemyDamage >= hitTriggerDamage)
            {
                EnemyAnimator.SetBool("hit", true);
                // アニメーションが再生し終わったらフラグをFALSEに戻す
                StartCoroutine(ResetHitFlag());
                enemyDamage = 0;
            }
        }

        // HPが0以下ならhitアニメーションを再生し終わった後にDeathフラグをセット
        if (HP <= 0)
        {
            // hitアニメーションが再生し終わったらDeathフラグをセット
            StartCoroutine(ResetHitFlag(true));
        }
    }

    // アニメーション終了後にhitフラグをFALSEに戻し、必要に応じてdeathフラグをTRUEにするコルーチン
    private IEnumerator ResetHitFlag(bool setDeathFlag = false)
    {
        // アニメーションが終了するまで待つ
        float animationLength = EnemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // hitフラグをFALSEに戻す
        EnemyAnimator.SetBool("hit", false);

        // deathフラグをTRUEにする
        if (setDeathFlag)
        {
            EnemyAnimator.SetBool("death", true);
            // コルーチンを開始してアニメーション終了後にDeath()を実行
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // アニメーションが終了するまで待つ
        float animationLength = EnemyAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // さらに1秒待つ
        yield return new WaitForSeconds(1f);

        // 死亡処理
        Death();
    }

    // 死亡処理のメソッド
    public void Death()
    {
        // 消滅エフェクト
        var effect = Instantiate(Effect);
        effect.transform.position = gameObject.transform.position;

        // ゲームオブジェクトを破壊
        Destroy(gameObject);
        Destroy(effect, DestroyTime);

        // 獲得経験値があるなら経験値処理
        if (charadata.GETEXP > 0)
        {
            playerdata.EXP = playerdata.EXP + charadata.GETEXP;

            while (playerdata.LV < lvdata.playerExpTable.Count && playerdata.EXP >= lvdata.playerExpTable[playerdata.LV].exp)
            {
                playerdata.EXP -= lvdata.playerExpTable[playerdata.LV].exp;
                playerdata.LV += 1;
                playerdata.StatusPoint += statusPoint;
                playerdata.MAXEXP *= maxexp;
            }
        }

        // 獲得ゴールドがあるならゴールド処理
        if (charadata.GETGOLD > 0)
        {
            playerdata.HAVEGOLD = playerdata.HAVEGOLD + charadata.GETGOLD;
        }
    }
}

