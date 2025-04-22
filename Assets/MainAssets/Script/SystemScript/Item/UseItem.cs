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
    [SerializeField] private Text messageText; // メッセージ表示用のText

    private ItemKanri itemKanriScript;
    private Slider hpSlider; // HPスライダー
    private Coroutine messageCoroutine; // メッセージ表示用のコルーチン

    private const float ButtonReenableDelay = 1f; // ボタンを再度有効にするまでの遅延時間

    void Start()
    {
        itemKanriScript = itemKanriObject.GetComponent<ItemKanri>();
        if (itemKanriScript == null)
        {
            return;
        }

        // タグを使用してHPスライダーを取得
        GameObject hpSliderObject = GameObject.FindWithTag("PlayerSlider");
        if (hpSliderObject == null)
        {
            return;
        }

        hpSlider = hpSliderObject.GetComponent<Slider>();
        if (hpSlider == null)
        {
            return;
        }

        // 既存のリスナーをクリアしてから新しいリスナーを追加
        usePotionButton.onClick.RemoveAllListeners();
        usePotionButton.onClick.AddListener(OnUsePotionButtonClick);

        // Toggleの状態が変わるたびにボタンの表示を更新
        var toggleGroup = itemKanriScript.GetToggleGroup();
        if (toggleGroup == null)
        {
            return;
        }

        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(delegate { UpdateUsePotionButtonVisibility(); });
        }

        // 初期状態のボタン表示を更新
        UpdateUsePotionButtonVisibility();
        // メッセージを非表示にする
        messageText.gameObject.SetActive(false);
    }

    void OnUsePotionButtonClick()
    {
        // ボタンを一時的に無効にする
        usePotionButton.interactable = false;

        // HPがMAXの場合はポーションを使用できない
        if (playerStatus.HP >= playerStatus.MAXHP)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            messageCoroutine = StartCoroutine(ShowMessage("これ以上回復しません"));
            StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
            return;
        }

        // インベントリ内でポーションを選択しているか確認
        var toggleGroup = itemKanriScript.GetToggleGroup();
        if (toggleGroup == null)
        {
            StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
            return;
        }

        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            string toggleName = activeToggle.name;
            if (int.TryParse(toggleName, out int index))
            {
                var motimonoList = itemKanriScript.GetMotimonoList();
                if (motimonoList == null)
                {
                    StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
                    return;
                }

                // インデックスが1から始まる場合の調整
                if (index > 0 && index <= motimonoList.Count)
                {
                    ItemData selectedItem = motimonoList[index - 1];
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
                        if (hpSlider != null)
                        {
                            hpSlider.value = (float)playerStatus.HP / playerStatus.MAXHP;
                        }

                        // HPテキストを更新
                        playerDamage.UpdateHPText();
                    }
                }
                else
                {
                    Debug.LogWarning($"Invalid index: {index}. MotimonoList size: {motimonoList.Count}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid toggle name: {toggleName}");
            }
        }

        // 一定時間後にボタンを再度有効にする
        StartCoroutine(ReenableButtonAfterDelay(ButtonReenableDelay));
    }

    void UpdateUsePotionButtonVisibility()
    {
        Debug.Log("Updating button visibility...");
        var toggleGroup = itemKanriScript.GetToggleGroup();
        if (toggleGroup == null)
        {
            Debug.LogWarning("ToggleGroup is null.");
            usePotionButton.gameObject.SetActive(false);
            return;
        }

        // アクティブなトグルを取得
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            string toggleName = activeToggle.name;
            Debug.Log($"Active toggle: {toggleName}");

            // トグル名をインデックスとして解釈
            if (int.TryParse(toggleName, out int index))
            {
                var motimonoList = itemKanriScript.GetMotimonoList();
                if (motimonoList == null)
                {
                    Debug.LogWarning("MotimonoList is null.");
                    usePotionButton.gameObject.SetActive(false);
                    return;
                }

                // インデックスが範囲内か確認
                if (index > 0 && index <= motimonoList.Count)
                {
                    ItemData selectedItem = motimonoList[index - 1];
                    if (selectedItem != null)
                    {
                        // 選択されたアイテムがポーションかどうかを確認
                        bool isPotion = selectedItem.GetItemType() == ItemData.itemtype.Portion;
                        Debug.Log($"Selected item: {selectedItem.GetItemName()}, IsPotion: {isPotion}");
                        usePotionButton.gameObject.SetActive(isPotion);
                        return;
                    }
                    else
                    {
                        Debug.LogWarning($"Selected item is null. Index: {index}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Index out of range: {index}. MotimonoList size: {motimonoList.Count}");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid toggle name: {toggleName}");
            }
        }
        else
        {
            Debug.LogWarning("No active toggle found.");
        }

        // ポーションが選択されていない場合はボタンを非表示にする
        usePotionButton.gameObject.SetActive(false);
    }


    IEnumerator ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        Color originalColor = messageText.color;
        messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);

        yield return new WaitForSeconds(2);

        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, t);
            yield return null;
        }

        messageText.gameObject.SetActive(false);
    }

    IEnumerator ReenableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        usePotionButton.interactable = true;
    }
}