using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSellExplanation : MonoBehaviour
{
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private ToggleGroup togglegroup;
    [SerializeField] private Text itemname;
    [SerializeField] private Text itemsetumei;
    [SerializeField] private Text itemBuyingPrice;
    [SerializeField] private Text itemSellingPrice;
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();
    private List<ItemData> MotimonoList = new List<ItemData>();
    private const int InitialWoodenSwordIndex = 0;
    private const int InitialClothArmorIndex = 3;

    void Start()
    {
        // 初期化処理
    }

    public void UpdateItemKazu(Dictionary<ItemData, int> newItemKazu)
    {
        itemkazu = newItemKazu;
    }
    public void Motimonokoushin()
    {
        // 持ち物リストをクリア
        MotimonoList.Clear();

        // 持っている個数が1個以上のアイテムを持ち物リストに追加する
        foreach (var item in itemkazu)
        {
            if (item.Value > 0)
            {
                MotimonoList.Add(item.Key);
            }
        }

        // 持ち物リストの内容をログに出力
        Debug.Log("Updated MotimonoList:");
        foreach (var item in MotimonoList)
        {
            Debug.Log($"Item: {item.GetItemName()}, Count: {itemkazu[item]}");
        }
    }


    public void slotkoushin()
    {
        // スロット更新処理
    }

    public void DisplayItemExplanation(int itemIndex)
    {
        // アイテムの説明を表示する処理
    }

    public List<ItemData> GetItemList()
    {
        return itemDataBase.GetItemList();
    }

    public ToggleGroup GetToggleGroup()
    {
        return togglegroup;
    }

    public void UpdateInventory()
    {
        // インベントリを更新する処理
    }
}
