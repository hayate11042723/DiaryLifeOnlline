using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemData")]
public class ItemData : ScriptableObject
{
    public enum itemtype
    {
        Sword,Rod,Armor,Clothes,Portion,Material
    }

    [SerializeField]
    private string Itemname; //アイテムの名前
    [SerializeField]
    private itemtype Itemtype; //アイテムのタイプ
    [SerializeField]
    private Sprite Itemicon; //アイテムのアイコン
    [SerializeField]
    private string Itemexplanation; //アイテムの説明
    [SerializeField]
    private int Itemlimit; //アイテムの持てる最大個数

    public string GetItemname()
    {
        return Itemname;
    }

    public itemtype GetItemtype()
    {
        return Itemtype;
    }

    public Sprite GetItemicon()
    {
        return Itemicon;
    }

    public string GetItemexplanation()
    {
        return Itemexplanation;
    }

    public int GetItemlimit()
    {
        return Itemlimit;
    }

}