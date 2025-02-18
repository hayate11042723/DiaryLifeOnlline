using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSelector0 : MonoBehaviour, IPointerEnterHandler
{
    public Button[] buttons; // ボタンの配列
    public RawImage cursor; // カーソルオブジェクト
    public Vector3 cursorOffset; // カーソルのオフセット
    private int currentIndex = 0; // 現在選択中のボタンのインデックス

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

        // マウス左クリック
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            buttons[currentIndex].onClick.Invoke();
        }
    }

    private void MoveSelection(int direction)
    {
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

        // カーソルを選択中のボタンの横に移動
        if (cursor != null)
        {
            cursor.rectTransform.position = buttons[index].transform.position + cursorOffset;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (eventData.pointerEnter == buttons[i].gameObject)
            {
                currentIndex = i;
                SelectButton(currentIndex);
                break;
            }
        }
    }
}
