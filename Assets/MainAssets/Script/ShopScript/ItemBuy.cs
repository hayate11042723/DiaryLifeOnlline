using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuy : MonoBehaviour
{
    // アイテムデータベース
    [SerializeField] private ItemDataBase itemDataBase;
    // アイテム管理スクリプト
    [SerializeField] private Itemkanri itemManager;
    // プレイヤーステータス
    [SerializeField] private PlayerStatus playerStatus;
    // クリックイベントのデバウンス用
    private bool isBuying = false;

    // アイテムを購入するメソッド
    public void BuyItem(int itemIndex)
    {
        // デバウンス処理
        if (isBuying) return;
        isBuying = true;

        // デバッグログ
        Debug.Log("BuyItem called");

        // アイテムインデックスが有効かどうかをチェック
        if (itemIndex < 0 || itemIndex >= itemDataBase.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
            isBuying = false;
            return;
        }

        // 購入するアイテムを取得
        ItemData item = itemDataBase.GetItemList()[itemIndex];
        // アイテムの購入価格を取得
        int itemPrice = item.GetItemBuyingPrice();

        // プレイヤーの所持金がアイテムの価格以上かどうかをチェック
        if (playerStatus.HAVEGOLD >= itemPrice)
        {
            // 所持金を減らす
            playerStatus.HAVEGOLD -= itemPrice;
            // アイテムを追加
            itemManager.AddItem(item);
            Debug.Log($"Bought {item.GetItemName()} for {itemPrice} gold. Remaining gold: {playerStatus.HAVEGOLD}");
        }
        else
        {
            // 所持金が足りない場合のメッセージ
            Debug.Log("Not enough gold to buy this item");
        }

        // デバウンス解除
        StartCoroutine(ResetIsBuying());
    }

    // デバウンス解除用のコルーチン
    private IEnumerator ResetIsBuying()
    {
        yield return new WaitForSeconds(0.1f); // 0.1秒待機
        isBuying = false;
    }
}