using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CharaAttackBoss : MonoBehaviour
{
    // シリアル化している。charadataを指定。
    [SerializeField] private CharaStatus charadata;
    // 胴体を持っているゲームオブジェクト(親)を指定。
    [SerializeField] GameObject AttackChara;
    // ヒット時に表示するエフェクトのPrefab
    [SerializeField] private GameObject hitEffectPrefab;
    // ヒットエフェクトの発生位置のオフセット
    [SerializeField] private Vector3 hitEffectOffset;
    // ヒットエフェクトのスケール
    [SerializeField] private Vector3 hitEffectScale = Vector3.one;

    int HHcount; // 残りヒット数
    int ATK; // 攻撃力
    ICharaAttack Hcount; // ICharaAttackインターフェース

    void Start()
    {
        if (charadata != null)
        {
            // charadataのATKを代入。
            ATK = charadata.ATK;
        }

        // ICharaAttackのインターフェースが定義されたコンポーネント(スクリプト)をHcountに代入。
        Hcount = AttackChara.GetComponent<ICharaAttack>();
    }

    // 体がゲームオブジェクトに侵入した瞬間に呼び出し
    void OnTriggerEnter(Collider other)
    {
        // ICharaAttackのインターフェースが定義されたスクリプトのHitCount()を呼び出し、返り値(残りヒット数)をHHcountに代入する。
        HHcount = Hcount.HitCount();

        // HHcountが0以下ならもう既にヒットしている。return;で処理を終わらせる。
        if (HHcount <= 0)
        {
            return;
        }

        if (other.tag == "Player")
        {
            // コライダーのあるゲームオブジェクトのインターフェースを呼び出す
            IDamageable damageable = other.GetComponent<IDamageable>();
            // damageableにnull値が入っていないかチェック
            if (damageable != null)
            {
                // ダメージが入るのが確定したのでヒット数-1
                Hcount.HitCountdown();
                // damageableのダメージ処理メソッドを呼び出す。引数としてcharadataのATKを指定
                damageable.Damage(ATK);

                // ヒットエフェクトを生成
                Vector3 hitPoint = other.ClosestPoint(transform.position) + hitEffectOffset;
                GameObject hitEffect = Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
                hitEffect.transform.localScale = hitEffectScale; // エフェクトのスケールを設定
            }
        }
    }
}

