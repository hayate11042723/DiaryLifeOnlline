using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSelector : MonoBehaviour
{
    public Button[] buttons; // ボタンの配列
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private Coroutine blinkingCoroutine; // 点滅コルーチン

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
        if (Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true)
        {
            buttons[currentIndex].onClick.Invoke();
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

        // 全ボタンの明度をリセット
        foreach (var btn in buttons)
        {
            var image = btn.GetComponent<Image>();
            if (image != null)
            {
                var color = image.color;
                color.a = 1f; // 明度をリセット（完全に明るい状態）
                image.color = color;
            }
        }
    }

    private IEnumerator BlinkEffect(Button button)
    {
        var image = button.GetComponent<Image>();
        if (image == null) yield break;

        // 点滅ループ
        while (true)
        {
            // 明るくする
            for (float i = 0.3f; i <= 1f; i += 0.05f)
            {
                SetImageBrightness(image, i);
                yield return new WaitForSeconds(0.05f);
            }

            // 暗くする
            for (float i = 1f; i >= 0.3f; i -= 0.05f)
            {
                SetImageBrightness(image, i);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void SetImageBrightness(Image image, float brightness)
    {
        var color = image.color;
        color.a = brightness; // アルファ値で明度を調整
        image.color = color;
    }
}
