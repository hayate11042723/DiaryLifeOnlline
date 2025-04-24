using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private GameObject titleLogo; // タイトルロゴのGameObject
    [SerializeField] private Button showButton;   // タイトルロゴを表示するボタン
    [SerializeField] private Button hideButton;   // タイトルロゴを非表示にするボタン

    void Start()
    {
        // ボタンのクリックイベントにメソッドを登録
        if (showButton != null)
        {
            showButton.onClick.AddListener(ShowTitleLogo);
        }

        if (hideButton != null)
        {
            hideButton.onClick.AddListener(HideTitleLogo);
        }
    }

    // タイトルロゴを表示するメソッド
    public void ShowTitleLogo()
    {
        if (titleLogo != null)
        {
            titleLogo.SetActive(true);
        }
    }

    // タイトルロゴを非表示にするメソッド
    public void HideTitleLogo()
    {
        if (titleLogo != null)
        {
            titleLogo.SetActive(false);
        }
    }
}
