using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSelector2 : MonoBehaviour
{
    [SerializeField] float waitForSeconds;
    [SerializeField] float iSeconds;
    [SerializeField] float enterKeyCooldown = 0.5f; // Enterキーのクールダウン時間（秒）
    [SerializeField] float mouseClickCooldown = 0.5f; // マウス左クリックのクールダウン時間（秒）

    public Button[] buttons; // ボタンの配列
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private Coroutine blinkingCoroutine; // 点滅コルーチン

    private float lastEnterKeyPressTime = -1f; // 最後にEnterキーが押された時間
    private float lastMouseClickTime = -1f; // 最後にマウス左クリックが押された時間

    private void Start()
    {
        if (buttons.Length > 0)
        {
            SelectButton(currentIndex);
        }
    }

    private void Update()
    {
        // 上方向キー
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.up.wasPressedThisFrame == true)
        {
            MoveSelection(-1);
        }

        // 下方向キー
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.down.wasPressedThisFrame == true)
        {
            MoveSelection(1);
        }

        // EnterキーまたはAボタン
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            buttons[currentIndex].onClick.Invoke();
            lastEnterKeyPressTime = Time.time; // 最後にEnterキーが押された時間を更新
        }

        // マウス左クリック
        if (Mouse.current.leftButton.wasReleasedThisFrame && Time.time - lastMouseClickTime > mouseClickCooldown)
        {
            buttons[currentIndex].onClick.Invoke();
            lastMouseClickTime = Time.time; // 最後にマウス左クリックが押された時間を更新
        }
    }

    private void MoveSelection(int direction)
    {
        // 点滅をリセット
        StopBlinkingEffect();

        // インデックスを更新
        currentIndex += direction;
        if (currentIndex < 0) currentIndex = buttons.Length - 1;
        if (currentIndex >= buttons.Length) currentIndex = 0;

        SelectButton(currentIndex);
    }

    private void SelectButton(int index)
    {
        // EventSystemでボタンを選択
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

        // 点滅を開始
        StartBlinkingEffect(buttons[index]);
    }

    private void StartBlinkingEffect(Button button)
    {
        blinkingCoroutine = StartCoroutine(BlinkEffect(button));
    }

    private void StopBlinkingEffect()
    {
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine);
            blinkingCoroutine = null;
        }

        // 全ボタンのテキストの明度をリセット
        foreach (var btn in buttons)
        {
            var text = btn.GetComponentInChildren<Text>();
            if (text != null)
            {
                var color = text.color;
                color.a = 1f; // 明度をリセット（完全に明るい状態）
                text.color = color;
            }
        }
    }

    private IEnumerator BlinkEffect(Button button)
    {
        var text = button.GetComponentInChildren<Text>();
        if (text == null) yield break;

        // 点滅ループ
        while (true)
        {
            // 明るくする
            for (float i = iSeconds; i <= 1f; i += waitForSeconds)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(waitForSeconds);
            }

            // 暗くする
            for (float i = 1f; i >= iSeconds; i -= waitForSeconds)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(waitForSeconds);
            }
        }
    }

    private void SetTextBrightness(Text text, float brightness)
    {
        var color = text.color;
        color.a = brightness; // アルファ値で明度を調整
        text.color = color;
    }
}

