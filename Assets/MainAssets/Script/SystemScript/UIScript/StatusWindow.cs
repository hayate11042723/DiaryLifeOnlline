using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour
{
    // プレイヤーのステータス情報
    public PlayerStatus playerStatus;
    public EnemyDamage LvUp;

    // UIテキスト要素
    public Text nameText;
    public Text hpText; // 現在のHPを表示するテキスト
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
    public Text statusPointText;

    // ステータス変更ボタン
    public Button increaseHpButton;
    public Button decreaseHpButton;
    public Button increaseMpButton;
    public Button decreaseMpButton;
    public Button increaseAtkButton;
    public Button decreaseAtkButton;
    public Button increaseDefButton;
    public Button decreaseDefButton;
    public Button increaseIntButton;
    public Button decreaseIntButton;
    public Button increaseMdefButton;
    public Button decreaseMdefButton;

    // Start is called before the first frame update
    void Start()
    {
        // UIを初期化
        UpdateUI();

        // ボタンにメソッドを登録
        increaseHpButton.onClick.AddListener(() => ChangeStatus("HP", 1));
        decreaseHpButton.onClick.AddListener(() => ChangeStatus("HP", -1));
        increaseMpButton.onClick.AddListener(() => ChangeStatus("MP", 1));
        decreaseMpButton.onClick.AddListener(() => ChangeStatus("MP", -1));
        increaseAtkButton.onClick.AddListener(() => ChangeStatus("ATK", 1));
        decreaseAtkButton.onClick.AddListener(() => ChangeStatus("ATK", -1));
        increaseDefButton.onClick.AddListener(() => ChangeStatus("DEF", 1));
        decreaseDefButton.onClick.AddListener(() => ChangeStatus("DEF", -1));
        increaseIntButton.onClick.AddListener(() => ChangeStatus("INT", 1));
        decreaseIntButton.onClick.AddListener(() => ChangeStatus("INT", -1));
        increaseMdefButton.onClick.AddListener(() => ChangeStatus("MDEF", 1));
        decreaseMdefButton.onClick.AddListener(() => ChangeStatus("MDEF", -1));
    }

    // UIを更新するメソッド
    public void UpdateUI()
    {
        // 各ステータスをUIに反映
        nameText.text = "名前: " + playerStatus.NAME;
        hpText.text = "HP: " + playerStatus.HP.ToString() + "/" + playerStatus.MAXHP.ToString(); // 現在のHPを表示
        maxHpText.text = "最大HP: " + playerStatus.MAXHP.ToString();
        maxMpText.text = "MP: " + playerStatus.MAXMP.ToString();
        atkText.text = "ATK: " + playerStatus.ATK.ToString();
        defText.text = "DEF: " + playerStatus.DEF.ToString();
        intText.text = "INT: " + playerStatus.INT.ToString();
        resText.text = "MDEF: " + playerStatus.MDEF.ToString();
        agiText.text = "AGI: " + playerStatus.AGI.ToString();
        lvText.text = "LV: " + playerStatus.LV.ToString();
        expText.text = "EXP: " + playerStatus.EXP.ToString();
        maxExpText.text = "次のレベルまでの必要EXP: " + playerStatus.MAXEXP.ToString();
        haveGoldText.text = "所持金: " + playerStatus.HAVEGOLD.ToString();
        statusPointText.text = "SP: " + playerStatus.StatusPoint.ToString();
    }

    // ステータスを変更するメソッド
    void ChangeStatus(string stat, int amount)
    {
        // ステータスポイントが不足している場合は警告を表示して処理を中断
        if (amount > 0 && playerStatus.StatusPoint < amount)
        {
            Debug.LogWarning("ステータスポイントが不足しています。");
            return;
        }

        // 指定されたステータスを変更
        switch (stat)
        {
            case "HP":
                if (amount < 0 && playerStatus.MAXHP <= 0) return;
                playerStatus.MAXHP = Mathf.Max(0, playerStatus.MAXHP + amount);
                break;
            case "MP":
                if (amount < 0 && playerStatus.MAXMP <= 0) return;
                playerStatus.MAXMP = Mathf.Max(0, playerStatus.MAXMP + amount);
                break;
            case "ATK":
                if (amount < 0 && playerStatus.ATK <= 0) return;
                playerStatus.ATK = Mathf.Max(0, playerStatus.ATK + amount);
                break;
            case "DEF":
                if (amount < 0 && playerStatus.DEF <= 0) return;
                playerStatus.DEF = Mathf.Max(0, playerStatus.DEF + amount);
                break;
            case "INT":
                if (amount < 0 && playerStatus.INT <= 0) return;
                playerStatus.INT = Mathf.Max(0, playerStatus.INT + amount);
                break;
            case "MDEF":
                if (amount < 0 && playerStatus.MDEF <= 0) return;
                playerStatus.MDEF = Mathf.Max(0, playerStatus.MDEF + amount);
                break;
        }

        // ステータスポイントを更新
        if (amount > 0)
        {
            playerStatus.StatusPoint -= amount;
        }
        else
        {
            playerStatus.StatusPoint += -amount;
        }

        // UIを更新
        UpdateUI();
    }

    // プレイヤーのステータスを更新するメソッド
    public void UpdatePlayerStatus(string name, int maxHp, int maxMp, int atk, int def, int intel, int mdef, int agi, int lv, int exp, int maxExp, int haveGold, int statusPoint)
    {
        // 各ステータスをUIに反映
        nameText.text = name;
        maxHpText.text = maxHp.ToString();
        maxMpText.text = maxMp.ToString();
        atkText.text = atk.ToString();
        defText.text = def.ToString();
        intText.text = intel.ToString();
        resText.text = mdef.ToString();
        agiText.text = agi.ToString();
        lvText.text = lv.ToString();
        expText.text = exp.ToString();
        maxExpText.text = maxExp.ToString();
        haveGoldText.text = haveGold.ToString();
        statusPointText.text = statusPoint.ToString();
    }
}
