using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{
    [SerializeField] private GameObject itemKanriObject;
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerDamage playerDamage; // PlayerDamageを参照
    [SerializeField] private Button usePotionButton;

    private Itemkanri itemKanriScript;

    void Start()
    {
        itemKanriScript = itemKanriObject.GetComponent<Itemkanri>();
        usePotionButton.onClick.AddListener(OnUsePotionButtonClick);
    }

    void OnUsePotionButtonClick()
    {
        // インベントリ内でポーションを選択しているか確認
        Toggle activeToggle = itemKanriScript.GetToggleGroup().ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            string toggleName = activeToggle.name;
            if (int.TryParse(toggleName, out int index))
            {
                if (itemKanriScript.GetMotimonoList().Count >= index)
                {
                    ItemData selectedItem = itemKanriScript.GetMotimonoList()[index - 1];
                    if (selectedItem.GetItemType() == ItemData.itemtype.Portion)
                    {
                        // ポーションを1個消費
                        itemKanriScript.DecreaseItem(selectedItem, 1);
                        itemKanriScript.Motimonokoushin();
                        itemKanriScript.slotkoushin();

                        // プレイヤーのHPを6割回復
                        int healAmount = Mathf.FloorToInt(playerStatus.MAXHP * 0.6f);
                        playerStatus.HP = Mathf.Min(playerStatus.HP + healAmount, playerStatus.MAXHP);

                        // HPスライダーを更新
                        playerDamage.Slider.value = (float)playerStatus.HP / playerStatus.MAXHP;

                        Debug.Log($"Used {selectedItem.GetItemName()}. Player HP: {playerStatus.HP}/{playerStatus.MAXHP}");
                    }
                }
            }
        }
    }
}
