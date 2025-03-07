using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton2 : MonoBehaviour
{
    // ショップのウィンドウ
    [SerializeField] private Canvas shopCanvas;
    // 他のUIキャンバス
    [SerializeField] private List<Canvas> otherCanvases;

    // ボタンがクリックされたときに呼び出されるメソッド
    public void OnButtonClick()
    {
        // Canvasをアクティブに設定
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(true);
            // 他のCanvasを非アクティブに設定
            foreach (var canvas in otherCanvases)
            {
                if (canvas != shopCanvas)
                {
                    canvas.gameObject.SetActive(false);
                }
            }
        }
    }

    // Canvasを閉じるメソッド
    public void CloseShop()
    {
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(false);
            // 他のCanvasを再アクティブに設定
            foreach (var canvas in otherCanvases)
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }
}
