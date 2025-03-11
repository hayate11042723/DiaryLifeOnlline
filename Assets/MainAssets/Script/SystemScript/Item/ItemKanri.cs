using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Itemkanri : MonoBehaviour
{
    // アイテムデータベース
    [SerializeField] private ItemDataBase itemDataBase;
    // アイテムアイコンの配列
    [SerializeField] private GameObject[] icons = new GameObject[IconArraySize];
    // トグルグループ
    [SerializeField] private ToggleGroup togglegroup;
    // アイテム名表示欄
    [SerializeField] private Text itemname;
    // アイテム説明表示欄
    [SerializeField] private Text itemsetumei;
    // アイテム数管理
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();
    // 持ち物リスト
    private List<ItemData> MotimonoList = new List<ItemData>();
    // 初期アイテムリスト
    private List<ItemData> initialItems = new List<ItemData>();
    // アイコンの配列
    private Image[] Icons = new Image[IconArraySize];
    // 空スロットの色
    [SerializeField] private Color emptySlotColor = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
    // 埋まっているスロットの色
    [SerializeField] private Color filledSlotColor = new Color(1, 1, 1, 1);
    // 初期アイテムのインデックス
    private const int InitialWoodenSwordIndex = 0;
    private const int InitialClothArmorIndex = 3;
    // アイコン配列のサイズ
    private const int IconArraySize = 24;

    void Start()
    {
        // 初期化アイテム処理
        foreach (var item in itemDataBase.GetItemList())
        {
            // アイテム数を全て0に
            itemkazu[item] = 0;
        }

        // 持っている初期アイテム設定
        var initialWoodenSword = itemDataBase.GetItemList()[InitialWoodenSwordIndex];
        var initialClothArmor = itemDataBase.GetItemList()[InitialClothArmorIndex];
        itemkazu[initialWoodenSword] = 1; // 木の剣の数を1にする
        itemkazu[initialClothArmor] = 1; // 布の服の数を1にする

        // 初期アイテムリストに追加
        initialItems.Add(initialWoodenSword);
        initialItems.Add(initialClothArmor);

        // アイコンのImageコンポーネントを取得
        for (int i = 0; i < IconArraySize; i++)
        {
            Icons[i] = icons[i].GetComponent<Image>();
        }

        // 持ち物更新処理を呼び出す
        Motimonokoushin();
    }

    // 持ち物更新処理
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

        // アイコンを更新
        UpdateIcons();
    }

    // アイコンを更新するメソッド
    private void UpdateIcons()
    {
        for (int i = 0; i < IconArraySize; i++)
        {
            if (i < MotimonoList.Count)
            {
                Icons[i].sprite = MotimonoList[i].GetItemIcon();
                Icons[i].color = filledSlotColor;
            }
            else
            {
                Icons[i].sprite = null;
                Icons[i].color = emptySlotColor;
            }
        }
    }

    // スロット更新処理
    public void slotkoushin()
    {
        // アクティブなトグルを取得
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
                }
                else
                {
                    // アイテムスロットが空の場合
                    itemname.text = null;
                    itemsetumei.text = null;
                }
            }
        }
    }

    // アイテムリストを取得するメソッド
    public List<ItemData> GetItemList()
    {
        return itemDataBase.GetItemList();
    }

    // 持ち物リストのアクセサ
    public List<ItemData> GetMotimonoList()
    {
        return MotimonoList;
    }

    // アイテムを減らすメソッド
    public void DecreaseItem(ItemData item, int amount)
    {
        if (itemkazu.ContainsKey(item))
        {
            itemkazu[item] -= amount;
            if (itemkazu[item] <= 0)
            {
                itemkazu[item] = 0;
            }
        }
        Motimonokoushin();
    }

    // アイテムを所持しているかどうかをチェックするメソッド
    public bool HasItem(ItemData item)
    {
        return itemkazu.ContainsKey(item) && itemkazu[item] > 0;
    }

    // アイテムを削除するメソッド
    public void RemoveItem(ItemData item)
    {
        if (itemkazu.ContainsKey(item))
        {
            itemkazu[item]--;
            if (itemkazu[item] <= 0)
            {
                itemkazu[item] = 0;
            }
        }
        Motimonokoushin();
    }

    // アイテムを追加するメソッド
    public void AddItem(ItemData item)
    {
        if (itemkazu.ContainsKey(item))
        {
            itemkazu[item]++;
        }
        else
        {
            itemkazu[item] = 1;
        }
        Motimonokoushin();
        // アイテム説明を更新
        var itemSellExplanation = FindObjectOfType<ItemSellExplanation>();
        itemSellExplanation.UpdateItemKazu(itemkazu);
        itemSellExplanation.Motimonokoushin();
        itemSellExplanation.slotkoushin();
    }

    // 初期アイテムかどうかをチェックするメソッド
    public bool IsInitialItem(ItemData item)
    {
        return initialItems.Contains(item);
    }

    // トグルグループを取得するメソッド
    public ToggleGroup GetToggleGroup()
    {
        return togglegroup;
    }
}


