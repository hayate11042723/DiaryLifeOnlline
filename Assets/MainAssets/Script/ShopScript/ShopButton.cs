using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButton : MonoBehaviour
{
    // ショップのウィンドウ
    [SerializeField] private Canvas shopCanvas;
    // プレイヤーの移動スクリプト
    [SerializeField] private MonoBehaviour playerMove;
    // プレイヤーの攻撃スクリプト
    [SerializeField] private MonoBehaviour playerAttack;
    // 非アクティブにするUIキャンバス
    [SerializeField] private List<Canvas> falseCanvases;
    // アクティブにするUIキャンバス
    [SerializeField] private List<Canvas> trueCanvases;
    // NPCのTransform
    [SerializeField] private Transform npcTransform;
    // プレイヤーのTransform
    [SerializeField] private Transform playerTransform;

    // ボタンがクリックされたときに呼び出されるメソッド
    public void OnButtonClick()
    {
        // Canvasをアクティブに設定
        if (shopCanvas != null)
        {
            shopCanvas.gameObject.SetActive(true);
            // 他のCanvasを非アクティブに設定
            foreach (var canvas in falseCanvases)
            {
                if (canvas != shopCanvas)
                {
                    canvas.gameObject.SetActive(false);
                }
            }
            // プレイヤーの移動と攻撃を無効にする
            if (playerMove != null)
            {
                playerMove.enabled = false;
            }
            if (playerAttack != null)
            {
                playerAttack.enabled = false;
            }
            // プレイヤーの向きをNPCの方へ向ける
            if (playerTransform != null && npcTransform != null)
            {
                Vector3 directionToNPC = (npcTransform.position - playerTransform.position).normalized;
                playerTransform.forward = directionToNPC;

                // NPCの向きをプレイヤーの方へ向ける
                Vector3 directionToPlayer = (playerTransform.position - npcTransform.position).normalized;
                npcTransform.forward = directionToPlayer;
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
            foreach (var canvas in trueCanvases)
            {
                canvas.gameObject.SetActive(true);
            }
            // プレイヤーの移動と攻撃を再有効化する
            if (playerMove != null)
            {
                playerMove.enabled = true;
            }
            if (playerAttack != null)
            {
                playerAttack.enabled = true;
            }
        }
    }
}
