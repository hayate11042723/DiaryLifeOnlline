using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class UIButtonSelector : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public Button[] buttons; // ボタンの配列
    public RawImage cursor; // カーソルオブジェクト
    public Vector3 cursorOffset; // カーソルのオフセット
    public GameObject canvas; // Canvasオブジェクト
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private Coroutine blinkingCoroutine; // 点滅コルーチン

    private float enterKeyCooldown = 0.5f; // Enterキーのクールダウン時間（秒）
    private float lastEnterKeyPressTime = -1f; // 最後にEnterキーが押された時間

    private void Start()
    {
        if (buttons.Length > 0)
        {
            SelectButton(currentIndex); // 最初のボタンを選択
        }
    }

    private void Update()
    {
        // Canvasが非表示になった場合、選択を最初のボタンに戻す
        if (canvas != null && !canvas.gameObject.activeInHierarchy)
        {
            currentIndex = 0;
            SelectButton(currentIndex);
            return; // Canvasが非表示の場合、他の入力処理を行わない
        }

        // 上方向キー
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.up.wasPressedThisFrame == true)
        {
            MoveSelection(-1); // 選択を上に移動
        }

        // 下方向キー
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.down.wasPressedThisFrame == true)
        {
            MoveSelection(1); // 選択を下に移動
        }

        // EnterキーまたはAボタン
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            buttons[currentIndex].onClick.Invoke(); // 現在選択中のボタンをクリック
            lastEnterKeyPressTime = Time.time; // 最後にEnterキーが押された時間を更新
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

        SelectButton(currentIndex); // 新しいボタンを選択
    }

    private void SelectButton(int index)
    {
        // EventSystemでボタンを選択
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);

        // カーソルを選択中のボタンの横に移動
        if (cursor != null)
        {
            cursor.rectTransform.position = buttons[index].transform.position + cursorOffset;
        }

        // 点滅を開始
        StartBlinkingEffect(buttons[index]);
    }

    private void StartBlinkingEffect(Button button)
    {
        blinkingCoroutine = StartCoroutine(BlinkEffect(button)); // 点滅コルーチンを開始
    }

    private void StopBlinkingEffect()
    {
        if (blinkingCoroutine != null)
        {
            StopCoroutine(blinkingCoroutine); // 点滅コルーチンを停止
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
            for (float i = 0.3f; i <= 1f; i += 0.05f)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(0.05f);
            }

            // 暗くする
            for (float i = 1f; i >= 0.3f; i -= 0.05f)
            {
                SetTextBrightness(text, i);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    private void SetTextBrightness(Text text, float brightness)
    {
        var color = text.color;
        color.a = brightness; // アルファ値で明度を調整
        text.color = color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (eventData.pointerEnter == buttons[i].gameObject)
            {
                currentIndex = i;
                SelectButton(currentIndex); // マウスがホバーしたボタンを選択

                // カーソルを選択中のボタンの横に移動
                if (cursor != null)
                {
                    cursor.rectTransform.position = buttons[i].transform.position + cursorOffset;
                }

                break;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (eventData.pointerPress == buttons[i].gameObject)
            {
                buttons[i].onClick.Invoke(); // マウスがクリックしたボタンをクリック
                break;
            }
        }
    }
}

