using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemData")]
public class ItemData : ScriptableObject
{
    public enum itemtype
    {
        Sword, Armor, Portion
    }

    [SerializeField]
    private string ItemName; // アイテムの名前
    [SerializeField]
    private itemtype ItemType; // アイテムのタイプ
    [SerializeField]
    private Sprite ItemIcon; // アイテムのアイコン
    [SerializeField]
    private string ItemExplanation; // アイテムの説明
    [SerializeField]
    private int ItemLimit; // アイテムの持てる最大個数
    [SerializeField]
    private int ItemBuyingPrice; // アイテムの購入価格
    [SerializeField]
    private int ItemSellingPrice; // アイテムの販売価格
    [SerializeField]
    private int ATK; // 攻撃力 (Swordの場合)
    [SerializeField]
    private int DFE; // 防御力 (Armorの場合)

    public string GetItemName()
    {
        return ItemName;
    }

    public itemtype GetItemType()
    {
        return ItemType;
    }

    public Sprite GetItemIcon()
    {
        return ItemIcon;
    }

    public string GetItemExplanation()
    {
        return ItemExplanation;
    }

    public int GetItemLimit()
    {
        return ItemLimit;
    }

    public int GetItemBuyingPrice()
    {
        return ItemBuyingPrice;
    }

    public int GetItemSellingPrice()
    {
        return ItemSellingPrice;
    }

    public int GetATK()
    {
        if (ItemType == itemtype.Sword)
        {
            return ATK;
        }
        return 0;
    }

    public int GetDFE()
    {
        if (ItemType == itemtype.Armor)
        {
            return DFE;
        }
        return 0;
    }
}
