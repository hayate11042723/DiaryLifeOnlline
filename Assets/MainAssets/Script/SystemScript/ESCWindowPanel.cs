using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ESCWindowPanel : MonoBehaviour
{
    // 表示/非表示を切り替える対象のパネル
    public GameObject panel;

    // トグル用の入力アクション
    public InputAction toggleAction;

    // ボタン
    public Button closeButton;

    private void OnEnable()
    {
        // 入力アクションを有効化
        toggleAction.Enable();
        toggleAction.performed += OnToggle;

        // ボタンのクリックイベントを登録
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(HidePanel);
        }
    }

    private void OnDisable()
    {
        // 入力アクションを無効化
        toggleAction.performed -= OnToggle;
        toggleAction.Disable();

        // ボタンのクリックイベントを解除
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(HidePanel);
        }
    }

    private void OnToggle(InputAction.CallbackContext context)
    {
        // パネルのアクティブ状態を切り替える
        if (panel != null)
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    public void HidePanel()
    {
        // パネルを非表示にする
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
