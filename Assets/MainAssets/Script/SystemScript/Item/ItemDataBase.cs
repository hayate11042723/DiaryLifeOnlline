using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    // アイテムデータのリスト
    [SerializeField]
    private List<ItemData> itemList = new List<ItemData>();

    // アイテムリストを返す
    public List<ItemData> GetItemList()
    {
        return itemList;
    }

    // アイテム名からアイテムデータを取得するメソッド
    public ItemData GetItemByName(string itemName)
    {
        return itemList.Find(item => item.GetItemName() == itemName);
    }
}
