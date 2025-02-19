using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class ItemSyoki : MonoBehaviour
{
    [SerializeField] private ItemDataBase ItemDataBase; // アイテムデータベース
    [SerializeField] private List<ItemData> initialItems; // 初期アイテムのリスト
    // アイテム数管理
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();

    // Start is called before the first frame update
    void Start()
    {
        // アイテムデータベースからアイテムリストを取得し、全てのアイテム数を0に初期化
        for (int i = 0; i < ItemDataBase.GetItemList().Count; i++)
        {
            itemkazu.Add(ItemDataBase.GetItemList()[i], 0);
        }

        // 初期アイテムを所持
        foreach (var item in initialItems)
        {
            if (itemkazu.ContainsKey(item))
            {
                itemkazu[item] = 1; // 初期アイテムの数を1に設定
            }
        }
    }
}