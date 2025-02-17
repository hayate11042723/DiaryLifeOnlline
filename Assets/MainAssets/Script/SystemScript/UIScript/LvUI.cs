using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] PlayerStatus charadata; // プレイヤーのステータスデータ
    [SerializeField] Text NAME; // プレイヤーの名前を表示するテキスト
    [SerializeField] Text LV; // プレイヤーのレベルを表示するテキスト

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの名前とレベルを取得
        string charaname = charadata.NAME;
        int lv = charadata.LV;

        // テキスト要素に名前とレベルを表示
        NAME.text = charaname;
        string lvtext = ($"LV{lv}");
        LV.text = lvtext;
    }
}