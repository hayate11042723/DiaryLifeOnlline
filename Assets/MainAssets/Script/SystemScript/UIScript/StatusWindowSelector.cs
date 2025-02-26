using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusWindowSelector : MonoBehaviour, IPointerClickHandler
{
    // ボタンのリスト
    public List<Button> buttons;
    // Canvasオブジェクト
    [SerializeField] Canvas canvas;
    // 時間
    [SerializeField] float time;
    // 現在選択中のボタンのインデックス
    private int currentIndex = 0;
    // ボタンが明滅中かどうかのフラグ
    private bool isBlinking = false;
    // 明滅の速度
    private float blinkSpeed = 1f;
    // Enterキーのクールダウン時間（秒）
    private float enterKeyCooldown = 0.5f;
    // 最後にEnterキーが押された時間
    private float lastEnterKeyPressTime = -1f;

    // Start is called before the first frame update
    void Start()
    {
        // ボタンが存在する場合、最初のボタンをハイライト
        if (buttons.Count > 0)
        {
            HighlightButton(currentIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Canvasが非表示になった場合、選択を最初のボタンに戻す
        if (canvas != null && !canvas.gameObject.activeInHierarchy)
        {
            currentIndex = 0;
            HighlightButton(currentIndex);
            return; // Canvasが非表示の場合、他の入力処理を行わない
        }

        // 右矢印キーが押された場合、次のボタンを選択
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeSelection(1);
        }
        // 左矢印キーが押された場合、前のボタンを選択
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeSelection(-1);
        }

        // EnterキーまたはAボタン
        if ((Keyboard.current.enterKey.wasPressedThisFrame || Gamepad.current?.aButton.wasPressedThisFrame == true) && Time.time - lastEnterKeyPressTime > enterKeyCooldown)
        {
            buttons[currentIndex].onClick.Invoke();
            lastEnterKeyPressTime = Time.time; // 最後にEnterキーが押された時間を更新
        }
    }

    // 選択を変更するメソッド
    private void ChangeSelection(int direction)
    {
        // 現在のボタンのハイライトを解除
        UnhighlightButton(currentIndex);
        // 新しいインデックスを計算
        currentIndex = (currentIndex + direction + buttons.Count) % buttons.Count;
        // 新しいボタンをハイライト
        HighlightButton(currentIndex);
    }

    // ボタンをハイライトするメソッド
    private void HighlightButton(int index)
    {
        // 明滅が開始されていない場合、明滅を開始
        if (!isBlinking)
        {
            StartCoroutine(BlinkButton(buttons[index]));
        }
    }

    // ボタンのハイライトを解除するメソッド
    private void UnhighlightButton(int index)
    {
        // 全てのコルーチンを停止
        StopAllCoroutines();
        isBlinking = false;
        // ボタンのテキストの透明度を元に戻す
        Text buttonText = buttons[index].GetComponentInChildren<Text>();
        Color color = buttonText.color;
        color.a = 1f;
        buttonText.color = color;
    }

    // ボタンを明滅させるコルーチン
    private IEnumerator BlinkButton(Button button)
    {
        isBlinking = true;
        Text buttonText = button.GetComponentInChildren<Text>();
        float alpha = 1f;
        bool fadingOut = true;

        while (isBlinking)
        {
            // 透明度をゆっくりと変化させる
            if (fadingOut)
            {
                alpha -= Time.deltaTime * blinkSpeed;
                if (alpha <= time)
                {
                    alpha = time;
                    fadingOut = false;
                }
            }
            else
            {
                alpha += Time.deltaTime * blinkSpeed;
                if (alpha >= 1f)
                {
                    alpha = 1f;
                    fadingOut = true;
                }
            }

            Color color = buttonText.color;
            color.a = alpha;
            buttonText.color = color;

            yield return null;
        }
    }

    // ボタンを決定するメソッド
    private void SelectButton()
    {
        // 現在選択中のボタンのクリックイベントを呼び出す
        buttons[currentIndex].onClick.Invoke();
    }

    // ボタンがクリックされたときの処理
    public void OnPointerClick(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (eventData.pointerPress == buttons[i].gameObject)
            {
                currentIndex = i;
                SelectButton();
                break;
            }
        }
    }
}
