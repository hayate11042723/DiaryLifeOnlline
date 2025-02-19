using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWindow : MonoBehaviour
{
    // ショップのウィンドウ
    [SerializeField] private Canvas shopCanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Canvasを非アクティブに設定
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーがコライダー内に入った時
        if(other.CompareTag("Player"))
        {
            // Canvasをアクティブに設定
            if (shopCanvas != null)
            {
                shopCanvas.gameObject.SetActive(true);
            }
        }
    }
}
