using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class itemkanri : MonoBehaviour
{
    [SerializeField] private ItemDataBase itemDataBase;

    // 作った各アイテムスロットのiconを指定(スロットの番号順に)
    [SerializeField] private GameObject[] icons = new GameObject[24];

    // トグルグループであるinventoryを指定。
    [SerializeField] private ToggleGroup togglegroup;

    // アイテム説明表示欄
    [SerializeField] private Text itemname;
    [SerializeField] private Text itemsetumei;

    // アイテム数管理
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();

    // 持ち物管理
    private List<ItemData> MotimonoList = new List<ItemData>();

    // アイコン管理の配列
    private Image[] Icons = new Image[24];

    // 空スロットのカラーをインスペクターで設定できるようにする
    [SerializeField] private Color emptySlotColor = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
    [SerializeField] private Color filledSlotColor = new Color(1, 1, 1, 1);

    // 定数
    private const int InitialWoodenSwordIndex = 0;
    private const int InitialClothArmorIndex = 3;
    private const int IconArraySize = 24;

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

        // アイテムスロットのアイコンimageをGetComponentしてアイコン配列に代入
        for (int i = 0; i < IconArraySize; i++)
        {
            if (icons[i] != null)
            {
                Icons[i] = icons[i].GetComponent<Image>();
                if (Icons[i] != null)
                {
                    // アイコンのスプライトをnullに設定し、空スロットのカラーを設定
                    Icons[i].sprite = null;
                    Icons[i].color = emptySlotColor;
                }
            }
        }

        // 持ち物リストの要素数だけ繰り返す
        for (int i = 0; i < MotimonoList.Count; i++)
        {
            var item = MotimonoList[i];
            if (Icons[i] != null)
            {
                // アイコンのスプライトとカラーを設定
                Icons[i].sprite = item.GetItemIcon();
                Icons[i].color = filledSlotColor;
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
    }

    // アイテムを削除するメソッド
    public void RemoveItem(ItemData item)
    {
        if (itemkazu.ContainsKey(item) && itemkazu[item] > 0)
        {
            itemkazu[item]--;
            if (itemkazu[item] == 0)
            {
                itemkazu.Remove(item);
            }
            Motimonokoushin();
        }
    }

    // アイテムを所持しているか確認するメソッド
    public bool HasItem(ItemData item)
    {
        return itemkazu.ContainsKey(item) && itemkazu[item] > 0;
    }
}