using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // PlayerのCharaStatusを読み込む
    [SerializeField] private PlayerStatus playerdata;
    [SerializeField] private GameObject hitEffectPrefab; // ヒット時に表示するエフェクトのPrefab
    [SerializeField] private Vector3 hitEffectScale = Vector3.one; // ヒットエフェクトのスケール

    private void OnTriggerEnter(Collider other)
    {
        // otherのゲームオブジェクトのインターフェースを呼び出す
        IDamageable damageable = other.GetComponent<IDamageable>();

        // damageableにnull値が入っていないかチェック
        if (damageable != null)
        {
            // damageableのdamage処理メソッドを呼び出す。引数としてPlayerのATKを指定
            damageable.Damage(playerdata.ATK);

            // ヒットエフェクトを生成
            Vector3 contactPoint = other.ClosestPointOnBounds(transform.position);
            GameObject hitEffect = Instantiate(hitEffectPrefab, contactPoint, Quaternion.identity);
            hitEffect.transform.localScale = hitEffectScale; // エフェクトのスケールを設定
        }
    }
}
