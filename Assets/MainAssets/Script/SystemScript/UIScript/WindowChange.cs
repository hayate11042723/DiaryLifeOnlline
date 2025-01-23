using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject currentCanvas; // 現在表示されているキャンバス
    public GameObject targetCanvas;  // 切り替え先のキャンバス

    public void SwitchCanvas()
    {
        if (currentCanvas != null)
        {
            SetButtonsInteractable(currentCanvas, false); // 現在のキャンバス内のボタンを無効にする
            currentCanvas.SetActive(false); // 現在のキャンバスを非表示
        }
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true);  // 切り替え先のキャンバスを表示
            SetButtonsInteractable(targetCanvas, true); // 切り替え先のキャンバス内のボタンを有効にする
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
