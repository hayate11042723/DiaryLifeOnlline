using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemSellExplanation : MonoBehaviour
{
    [SerializeField] private ItemDataBase itemDataBase;
    // トグルグループであるinventoryを指定。
    [SerializeField] private ToggleGroup togglegroup;
    // アイテム説明表示欄
    [SerializeField] private Text itemname;
    [SerializeField] private Text itemsetumei;
    [SerializeField] private Text itemBuyingPrice;
    [SerializeField] private Text itemSellingPrice;

    // アイテム数管理
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();
    // 持ち物管理
    private List<ItemData> MotimonoList = new List<ItemData>();

    // 定数
    private const int InitialWoodenSwordIndex = 0;
    private const int InitialClothArmorIndex = 3;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化アイテム処理
        foreach (var item in itemDataBase.GetItemList())
        {
            // アイテム数を全て0に
            itemkazu[item] = 0;
        }

        // 持っている初期アイテム設定
        itemkazu[itemDataBase.GetItemList()[InitialWoodenSwordIndex]] = 1; // 木の剣の数を1にする
        itemkazu[itemDataBase.GetItemList()[InitialClothArmorIndex]] = 1; // 布の服の数を1にする

        // 持ち物更新処理を呼び出す
        Motimonokoushin();
        slotkoushin(); // スロット更新処理を呼び出す
    }

    // アイテム数を更新するメソッド
    public void UpdateItemKazu(Dictionary<ItemData, int> newItemKazu)
    {
        itemkazu = newItemKazu;
    }

    // どこからでもアクセス可能。返り値なし。
    public void Motimonokoushin()
    {
        // 持ち物更新処理
        MotimonoList.Clear();

        // 持っている個数が1個以上のアイテムを持ち物リストに追加する
        foreach (var item in itemkazu)
        {
            if (item.Value > 0)
            {
                MotimonoList.Add(item.Key);
            }
        }
    }

    public void slotkoushin()
    {
        // スロット更新処理
        Toggle tgl = togglegroup.ActiveToggles().FirstOrDefault();
        if (tgl != null)
        {
            string x = tgl.name;
            if (int.TryParse(x, out int y))
            {
                if (MotimonoList.Count >= y)
                {
                    // 選択されたアイテムスロットのアイテム名と個数を表示
                    string z = MotimonoList[y - 1].GetItemName();
                    int k = itemkazu[MotimonoList[y - 1]];
                    itemname.text = $"{z}×{k}";
                    itemsetumei.text = MotimonoList[y - 1].GetItemExplanation();
                    itemBuyingPrice.text = $"購入価格: {MotimonoList[y - 1].GetItemBuyingPrice()}";
                    itemSellingPrice.text = $"販売価格: {MotimonoList[y - 1].GetItemSellingPrice()}";
                }
                else
                {
                    // アイテムスロットが空の場合
                    itemname.text = null;
                    itemsetumei.text = null;
                    itemBuyingPrice.text = null;
                    itemSellingPrice.text = null;
                }
            }
        }
    }

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
        itemBuyingPrice.text = $"購入価格: {item.GetItemBuyingPrice()}";
        itemSellingPrice.text = $"販売価格: {item.GetItemSellingPrice()}";
    }

    // アイテムリストを取得するメソッド
    public List<ItemData> GetItemList()
    {
        return itemDataBase.GetItemList();
    }

    // トグルグループのアクセサ
    public ToggleGroup GetToggleGroup()
    {
        return togglegroup;
    }

    // 売却画面に移ったときにインベントリを更新するメソッド
    public void UpdateInventory()
    {
        Motimonokoushin();
        slotkoushin();
    }
}

