using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    [SerializeField] private GameObject itemKanriObject;
    [SerializeField] private GameObject ShopManager;
    [SerializeField] private PlayerStatus playerStatus; // プレイヤーステータスを参照
    [SerializeField] private Text playerMoneyText; // 所持金を表示するテキスト
    [SerializeField] private Text messageText; // メッセージを表示するテキスト
    [SerializeField] private Button sellButton; // 売却ボタン

    private ItemKanri itemKanriScript;
    private ItemSellExplanation itemSellExplanationScript;

    void Start()
    {
        itemKanriScript = itemKanriObject.GetComponent<ItemKanri>();
        itemSellExplanationScript = ShopManager.GetComponent<ItemSellExplanation>();
        UpdatePlayerMoneyText();
        messageText.gameObject.SetActive(false); // メッセージを非表示にする
    }

    public void OnSellButtonClick()
    {
        Toggle activeToggle = itemSellExplanationScript.GetToggleGroup().ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            string toggleName = activeToggle.name;
            if (int.TryParse(toggleName, out int index))
            {
                // インデックスが範囲内か確認
                var motimonoList = itemKanriScript.GetMotimonoList();
                if (index >= 0 && index < motimonoList.Count)
                {
                    ItemData selectedItem = motimonoList[index];
                    if (!itemKanriScript.IsInitialItem(selectedItem))
                    {
                        int sellingPrice = selectedItem.GetItemSellingPrice();
                        playerStatus.HAVEGOLD += sellingPrice; // 所持金を増やす
                        itemKanriScript.DecreaseItem(selectedItem, 1);
                        itemKanriScript.Motimonokoushin();
                        itemSellExplanationScript.Motimonokoushin();
                        itemSellExplanationScript.slotkoushin();
                        UpdatePlayerMoneyText(); // 所持金の表示を更新

                        // アイテムの説明を更新
                        if (index < motimonoList.Count)
                        {
                            itemSellExplanationScript.DisplayItemExplanation(index);
                        }
                        else
                        {
                            // 売却後にアイテムがなくなった場合、説明をクリア
                            itemSellExplanationScript.DisplayItemExplanation(-1);
                        }

                        messageText.text = ""; // メッセージをクリア
                    }
                    else
                    {
                        messageText.text = "初期アイテムは売却できません";
                        StartCoroutine(FadeOutMessage());
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
    }


    private void UpdatePlayerMoneyText()
    {
        playerMoneyText.text = $"所持金: {playerStatus.HAVEGOLD}G";
    }

    private IEnumerator FadeOutMessage()
    {
        messageText.gameObject.SetActive(true); // メッセージを表示
        sellButton.interactable = false; // 売却ボタンを無効にする

        Color originalColor = messageText.color;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / 2.0f) // 2秒かけてフェードアウト
        {
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        messageText.gameObject.SetActive(false); // メッセージを非表示にする
        messageText.color = originalColor; // 元の色に戻す
        sellButton.interactable = true; // 売却ボタンを再度有効にする
    }

    // 売却画面に移ったときにインベントリを更新するメソッド
    public void OnEnterSellScreen()
    {
        itemSellExplanationScript.UpdateInventory();
    }
}

