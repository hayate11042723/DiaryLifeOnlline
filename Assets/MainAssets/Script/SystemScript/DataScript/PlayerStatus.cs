using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create PlayerStatusData")]
public class PlayerStatus : ScriptableObject
{
    public string NAME;     //キャラ名
    public int MAXHP;       //最大HP
    public int MAXMP;       //最大MP
    public int ATK;         //攻撃力
    public int DEF;         //防御力
    public int INT;         //魔力
    public int MDEF;        //魔法抵抗力
    public int AGI;         //移動速度
    public int LV;          //レベル
    public int EXP;         //経験値
    public int MAXEXP;      //次のレベルまでの経験値
    public int HAVEGOLD;    //所持金
    public int StatusPoint; //ステータスポイント

    // レベルアップ時に呼び出すメソッド
    public void LevelUp()
    {
        StatusPoint += 20; // レベルアップ時にステータスポイントを20増加させる
        EXP = 0; // 経験値をリセット
        MAXEXP *= 2; // 次のレベルまでの経験値を増加させる
    }
}