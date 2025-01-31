using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemData;

public class ItemSyoki : MonoBehaviour
{
    [SerializeField] private ItemDataBase ItemDataBase;
    //　アイテム数管理
    private Dictionary<ItemData, int> itemkazu = new Dictionary<ItemData, int>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ItemDataBase.GetItemList().Count; i++)
        {
            //　アイテム数を全て0に
            itemkazu.Add(ItemDataBase.GetItemList()[i], 0);


        }

        //ポーションのみ数を2にする。
        itemkazu[ItemDataBase.GetItemList()[2]] = 2;

        var a = itemkazu[ItemDataBase.GetItemList()[0]];
        var b = itemkazu[ItemDataBase.GetItemList()[1]];
        var c = itemkazu[ItemDataBase.GetItemList()[2]];

        Debug.Log(a);
        Debug.Log(b);
        Debug.Log(c);



        var d = ItemDataBase.GetItemList()[1].GetItemtype();
        Debug.Log(d);


    }

}