using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemExplanation : MonoBehaviour
{
    // アイテムデータベースの取得
    [SerializeField] private ItemDataBase itemDataBase;

    // アイテム説明表示欄
    [SerializeField] private Text itemname;
    [SerializeField] private Text itemsetumei;
    [SerializeField] private Text itemBuyPrice;
    [SerializeField] private Text itemSellPrice;

    // アイテムの説明を表示するメソッド
    public void DisplayItemExplanation(int itemIndex)
    {
        // アイテムインデックスが有効かどうかをチェック
        if (itemIndex < 0 || itemIndex >= itemDataBase.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
            return;
        }

        // アイテムデータを取得
        ItemData item = itemDataBase.GetItemList()[itemIndex];

        // アイテムの名称と説明をテキストに設定
        itemname.text = item.GetItemName();
        itemsetumei.text = item.GetItemExplanation();
        itemBuyPrice.text = "購入価格: " + item.GetItemBuyingPrice().ToString();
        itemSellPrice.text = "売却価格: " + item.GetItemSellingPrice().ToString();
    }
}
