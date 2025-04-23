using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create PlayerStatusData")]
public class PlayerStatus : ScriptableObject
{
    public string NAME;
    public int MAXHP;
    public int HP;
    public int MAXMP;
    public int ATK;
    public int DEF;
    public int INT;
    public int MDEF;
    public int AGI;
    public int LV;
    public int EXP;
    public int MAXEXP;
    public int HAVEGOLD;
    public int StatusPoint;

    // 基本値を保持するプロパティ
    public int BaseATK { get; set; }
    public int BaseDEF { get; set; }
    public int BaseINT { get; set; }
    public int BaseMDEF { get; set; }
    public int BaseAGI { get; set; }

    // 初期化メソッド
    public void InitializeBaseStats()
    {
        BaseATK = ATK;
        BaseDEF = DEF;
        BaseINT = INT;
        BaseMDEF = MDEF;
        BaseAGI = AGI;
    }
}
