using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // PlayerのCharaStatusを読み込む
    [SerializeField] private CharaStatus playerdata;

    private void OnTriggerEnter(Collider other)
    {
        // otherのゲームオブジェクトのインターフェースを呼び出す
        IDamageable damageable = other.GetComponent<IDamageable>();

        // damageableにnull値が入っていないかチェック
        if (damageable != null)
        {
            // damageableのdamage処理メソッドを呼び出す。引数としてPlayerのATKを指定
            damageable.Damage(playerdata.ATK);
        }
    }
}
