using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField]
    private List<ItemData> itemList = new List<ItemData>();

    // アイテムリストを返す
    public List<ItemData> GetItemList()
    {
        return itemList;
    }
}