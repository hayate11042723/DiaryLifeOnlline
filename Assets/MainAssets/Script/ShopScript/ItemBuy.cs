using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuy : MonoBehaviour
{
    // アイテムデータベース
    [SerializeField] private ItemDataBase itemDataBase;
    // アイテム管理スクリプト
    [SerializeField] private itemkanri itemManager;
    // プレイヤーステータス
    [SerializeField] private PlayerStatus playerStatus;

    // アイテムを購入するメソッド
    public void BuyItem(int itemIndex)
    {
        // アイテムインデックスが有効かどうかをチェック
        if (itemIndex < 0 || itemIndex >= itemDataBase.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
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
    }
}

