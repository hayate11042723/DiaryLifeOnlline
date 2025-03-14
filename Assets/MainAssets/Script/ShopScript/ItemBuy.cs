using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBuy : MonoBehaviour
{
    // アイテムデータベース
    [SerializeField] private ItemDataBase itemDataBase;
    // アイテム管理スクリプト
    [SerializeField] private ItemKanri itemManager;
    // プレイヤーステータス
    [SerializeField] private PlayerStatus playerStatus;
    // 所持金を表示するテキスト
    [SerializeField] private Text playerMoneyText;
    // メッセージを表示するテキスト
    [SerializeField] private Text messageText;
    // クリックイベントのデバウンス用
    private bool isBuying = false;
    // 現在のフェードアウトコルーチンを管理するためのフィールド
    private Coroutine fadeOutCoroutine;

    void Start()
    {
        messageText.gameObject.SetActive(false); // メッセージを非表示にする
        UpdatePlayerMoneyText(); // 所持金の初期表示を設定
    }

    // アイテムを購入するメソッド
    public void BuyItem(int itemIndex)
    {
        // デバウンス処理
        if (isBuying) return;
        isBuying = true;

        // デバッグログ
        Debug.Log("BuyItem called");

        // アイテムインデックスが有効かどうかをチェック
        if (itemIndex < 0 || itemIndex >= itemDataBase.GetItemList().Count)
        {
            Debug.LogError("Invalid item index");
            isBuying = false;
            return;
        }

        // 購入するアイテムを取得
        ItemData item = itemDataBase.GetItemList()[itemIndex];
        // アイテムの購入価格を取得
        int itemPrice = item.GetItemBuyingPrice();

        // プレイヤーの所持金がアイテムの価格以上かどうかをチェック
        if (playerStatus.HAVEGOLD >= itemPrice)
        {
            // 所持金を減らす
            playerStatus.HAVEGOLD -= itemPrice;
            // アイテムを追加
            itemManager.AddItem(item);
            Debug.Log($"Bought {item.GetItemName()} for {itemPrice} gold. Remaining gold: {playerStatus.HAVEGOLD}");
            UpdatePlayerMoneyText(); // 所持金の表示を更新
        }
        else
        {
            // 所持金が足りない場合のメッセージ
            messageText.text = "所持金が足りません";
            if (fadeOutCoroutine != null)
            {
                StopCoroutine(fadeOutCoroutine); // 現在のフェードアウトコルーチンを停止
            }
            fadeOutCoroutine = StartCoroutine(FadeOutMessage()); // 新しいフェードアウトコルーチンを開始
        }

        // デバウンス解除
        StartCoroutine(ResetIsBuying());
    }

    // デバウンス解除用のコルーチン
    private IEnumerator ResetIsBuying()
    {
        yield return new WaitForSeconds(0.1f); // 0.1秒待機
        isBuying = false;
    }

    // 所持金の表示を更新するメソッド
    private void UpdatePlayerMoneyText()
    {
        playerMoneyText.text = $"所持金: {playerStatus.HAVEGOLD}G";
    }

    // メッセージをフェードアウトさせるコルーチン
    private IEnumerator FadeOutMessage()
    {
        messageText.gameObject.SetActive(true); // メッセージを表示

        Color originalColor = messageText.color;
        for (float t = 0; t < 1.0f; t += Time.deltaTime / 2.0f) // 2秒かけてフェードアウト
        {
            messageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        messageText.gameObject.SetActive(false); // メッセージを非表示にする
        messageText.color = originalColor; // 元の色に戻す
    }
}