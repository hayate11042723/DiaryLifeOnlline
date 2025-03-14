using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSell : MonoBehaviour
{
    // アイテム管理スクリプト
    [SerializeField] private ItemKanri itemManager;
    // プレイヤーステータス
    [SerializeField] private PlayerStatus playerStatus;
    // インベントリ
    [SerializeField] private GameObject inventory;
    // 特定のCanvas
    [SerializeField] private Canvas targetCanvas;
    // 元の親オブジェクト
    private Transform originalParent;

    private void Start()
    {
        // インベントリの元の親オブジェクトを保存
        originalParent = inventory.transform.parent;
    }

    // アイテムを売るメソッド
    public void SellItem(int itemIndex)
    {
        // アイテムインデックスが有効かどうかをチェック
        if (itemIndex < 0 || itemIndex >= itemManager.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
            return;
        }

        // 売却するアイテムを取得
        ItemData item = itemManager.GetItemList()[itemIndex];
        // アイテムの売却価格を取得
        int itemPrice = item.GetItemSellingPrice();

        // プレイヤーがアイテムを所持しているかどうかをチェック
        if (itemManager.HasItem(item))
        {
            // アイテムを削除
            itemManager.RemoveItem(item);
            // 所持金を増やす
            playerStatus.HAVEGOLD += itemPrice;
            Debug.Log($"Sold {item.GetItemName()} for {itemPrice} gold. Total gold: {playerStatus.HAVEGOLD}");
        }
        else
        {
            // アイテムを所持していない場合のメッセージ
            Debug.Log("You don't have this item to sell");
        }
    }

    // インベントリを特定のCanvasの子要素にするメソッド
    public void MoveInventoryToCanvas()
    {
        inventory.transform.SetParent(targetCanvas.transform, false);
    }

    // インベントリを元の親オブジェクトに戻すメソッド
    public void ResetInventoryParent()
    {
        inventory.transform.SetParent(originalParent, false);
    }
}