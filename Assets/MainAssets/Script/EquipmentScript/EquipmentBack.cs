using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentBack : MonoBehaviour
{
    public GameObject currentCanvas; // 現在表示されているキャンバス
    public GameObject targetCanvas;  // 切り替え先のキャンバス
    public GameObject inventoryCanvas; // Inventoryを移す先のキャンバス
    public GameObject itemKanriUI;   // インベントリUI

    private Transform originalParent; // 元の親要素を保持する変数

    void Start()
    {
        // itemKanriUIが設定されているか確認
        if (itemKanriUI == null)
        {
            Debug.LogError("itemKanriUI is not assigned.");
            return;
        }

        // 元の親要素を保存
        originalParent = itemKanriUI.transform.parent;
    }

    public void SwitchCanvas()
    {
        // 現在のCanvasを非表示にし、ボタンを無効化
        if (currentCanvas != null)
        {
            SetButtonsInteractable(currentCanvas, false);
            currentCanvas.SetActive(false);
        }

        // 切り替え先のCanvasを表示し、ボタンを有効化
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true);
            SetButtonsInteractable(targetCanvas, true);
        }

        // Inventoryを移す先のCanvasに設定
        if (inventoryCanvas != null && itemKanriUI != null)
        {
            itemKanriUI.transform.SetParent(inventoryCanvas.transform, false);
            Debug.Log($"Inventory moved to the inventory Canvas: {inventoryCanvas.name}");
        }
    }

    public void ResetInventoryParent()
    {
        // Inventoryを元の親要素に戻す
        if (itemKanriUI != null && originalParent != null)
        {
            itemKanriUI.transform.SetParent(originalParent, false);
            Debug.Log("Inventory returned to its original parent.");
        }
    }

    private void SetButtonsInteractable(GameObject canvas, bool interactable)
    {
        Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
        foreach (Button button in buttons)
        {
            button.interactable = interactable;
        }
    }
}
