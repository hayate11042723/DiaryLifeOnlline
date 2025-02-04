using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour
{
    // プレイヤーのステータス情報
    public PlayerStatus playerStatus;

    // UIテキスト要素
    public Text nameText;
    public Text maxHpText;
    public Text maxMpText;
    public Text atkText;
    public Text defText;
    public Text intText;
    public Text resText;
    public Text agiText;
    public Text lvText;
    public Text expText;
    public Text maxExpText;
    public Text haveGoldText;

    // Start is called before the first frame update
    void Start()
    {
        // UIを初期化
        UpdateUI();
    }

    // UIを更新するメソッド
    void UpdateUI()
    {
        // 各ステータスをUIに反映
        nameText.text = "名前: " + playerStatus.NAME;
        maxHpText.text = "HP: " + playerStatus.MAXHP.ToString();
        maxMpText.text = "MP: " + playerStatus.MAXMP.ToString();
        atkText.text = "ATK: " + playerStatus.ATK.ToString();
        defText.text = "DEF: " + playerStatus.DEF.ToString();
        intText.text = "INT: " + playerStatus.INT.ToString();
        resText.text = "MDEF: " + playerStatus.MDEF.ToString();
        // agiText.text = "AGI: " + playerStatus.AGI.ToString(); // コメントアウトされている
        lvText.text = "LV: " + playerStatus.LV.ToString();
        expText.text = "EXP: " + playerStatus.EXP.ToString();
        maxExpText.text = "次のレベルまでの必要EXP: " + playerStatus.MAXEXP.ToString();
        haveGoldText.text = "所持金: " + playerStatus.HAVEGOLD.ToString();
    }

    // プレイヤーのステータスを更新するメソッド
    public void UpdatePlayerStatus(string name, int maxHp, int maxMp, int atk, int def, int intStat, int res, int agi, int lv, int exp, int maxExp, int haveGold)
    {
        // ステータスを更新
        playerStatus.NAME = name;
        playerStatus.MAXHP = maxHp;
        playerStatus.MAXMP = maxMp;
        playerStatus.ATK = atk;
        playerStatus.DEF = def;
        playerStatus.INT = intStat;
        playerStatus.MDEF = res;
        playerStatus.AGI = agi;
        playerStatus.LV = lv;
        playerStatus.EXP = exp;
        playerStatus.MAXEXP = maxExp;
        playerStatus.HAVEGOLD = haveGold;

        // UIを更新
        UpdateUI();
    }
}