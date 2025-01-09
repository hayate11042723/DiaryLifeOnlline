using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public GameObject currentCanvas; // 現在表示されているキャンバス
    public GameObject targetCanvas;  // 切り替え先のキャンバス

    public void SwitchCanvas()
    {
        if (currentCanvas != null)
        {
            currentCanvas.SetActive(false); // 現在のキャンバスを非表示
        }
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true);  // 切り替え先のキャンバスを表示
        }
    }
}
