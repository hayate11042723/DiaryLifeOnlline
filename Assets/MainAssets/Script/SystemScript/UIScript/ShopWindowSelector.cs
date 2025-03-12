using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ShopWindowSelector : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] float waitForSeconds; // 点滅の間隔
    [SerializeField] float iSeconds; // 点滅の明るさの変化量
    [SerializeField] float enterKeyCooldown = 0.5f; // Enterキーのクールダウン時間（秒）
    [SerializeField] float mouseClickCooldown = 0.5f; // マウス左クリックのクールダウン時間（秒）
    [SerializeField] float clickBrightnessDuration = 0.1f; // クリック時の彩度変更の持続時間（秒）

    public Button[] buttons; // ボタンの配列
    public RawImage cursor; // カーソルオブジェクト
    public Vector3 cursorOffset; // カーソルのオフセット
    private int currentIndex = 0; // 現在選択中のボタンのインデックス
    private Coroutine blinkingCoroutine; // 点滅コルーチン

    private float lastEnterKeyPressTime = -1f; // 最後にEnterキーが押された時間
    private float lastMouseClickTime = -1f; // 最後にマウス左クリックが押された時間

    private void Start()
    {
        if (buttons.Length > 0)
        {
            SelectButton(currentIndex); // 最初のボタンを選択
        }
    }

    private void Update()
    {
        // 上方向キー
        if (Keyboard.current.upArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.up.wasPressedThisFrame == true)
        {
            MoveSelection(-1, 0);
        }

        // 下方向キー
        if (Keyboard.current.downArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.down.wasPressedThisFrame == true)
        {
            MoveSelection(1, 0);
        }

        // 左方向キー
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.left.wasPressedThisFrame == true)
        {
            MoveSelection(0, -1);
        }

        // 右方向キー
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Gamepad.current?.dpad.right.wasPressedThisFrame == true)
        {
            MoveSelection(0, 1);
        }

        // EnterキーまたはAボタン
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            StartCoroutine(ClickEffect(buttons[currentIndex])); // クリックエフェクトを開始
            buttons[currentIndex].onClick.Invoke(); // ボタンのクリックイベントを発生させる
            lastEnterKeyPressTime = Time.time; // 最後にEnterキーが押された時間を更新
        }

        // マウス左クリック
        if (Mouse.current.leftButton.wasReleasedThisFrame && Time.time - lastMouseClickTime > mouseClickCooldown)
        {
            // マウスクリック位置にあるボタンを選択
            SelectButtonUnderMouse();
            lastMouseClickTime = Time.time; // 最後にマウス左クリックが押された時間を更新
        }
    }

    // ボタンの選択を移動するメソッド
    private void MoveSelection(int verticalDirection, int horizontalDirection)
    {
        // 点滅をリセット
        StopBlinkingEffect();

        // インデックスを更新
        int newIndex = currentIndex + verticalDirection * GetHorizontalCount() + horizontalDirection;

        // 範囲チェック
        if (newIndex < 0)
        {
            newIndex = buttons.Length - 1;
        }
        else if (newIndex >= buttons.Length)
        {
            newIndex = 0;
        }

        currentIndex = newIndex;
        SelectButton(currentIndex);
    }

    // ボタンを選択するメソッド
    private void SelectButton(int index)
    {
        // 現在の選択をクリア
        EventSystem.current.SetSelectedGameObject(null);

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

    // マウス位置にあるボタンを選択するメソッド
    private void SelectButtonUnderMouse()
    {
        // マウス位置にあるボタンを検出
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                // ボタンが見つかった場合、そのボタンを選択し、クリックイベントを発生させる
                currentIndex = System.Array.IndexOf(buttons, button);
                SelectButton(currentIndex);
                StartCoroutine(ClickEffect(button));
                button.onClick.Invoke();
                break;
            }
        }
    }

    // 点滅エフェクトを開始するメソッド
    private void StartBlinkingEffect(Button button)
    {
        blinkingCoroutine = StartCoroutine(BlinkEffect(button));
    }

    // 点滅エフェクトを停止するメソッド
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

    // 点滅エフェクトのコルーチン
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

    // テキストの明度を設定するメソッド
    private void SetTextBrightness(Text text, float brightness)
    {
        var color = text.color;
        color.a = brightness; // アルファ値で明度を調整
        text.color = color;
    }

    // クリックエフェクトのコルーチン
    private IEnumerator ClickEffect(Button button)
    {
        var text = button.GetComponentInChildren<Text>();
        if (text == null) yield break;

        // 彩度を一瞬落とす
        SetTextBrightness(text, 0.5f);
        yield return new WaitForSeconds(clickBrightnessDuration);
        // 彩度を元に戻す
        SetTextBrightness(text, 1f);
    }

    // 水平方向のボタン数を取得するメソッド
    private int GetHorizontalCount()
    {
        // ここでは仮に4としていますが、実際のボタン配置に合わせて変更してください
        return 4;
    }

    // マウスがボタンに重なったときに呼び出されるメソッド
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
}
