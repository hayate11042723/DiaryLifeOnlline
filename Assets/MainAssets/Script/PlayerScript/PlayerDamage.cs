using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour, IDamageable
{
    [SerializeField] public PlayerStatus charadata; // プレイヤーのステータスデータ
    [SerializeField] public Slider Slider; // HPを表示するスライダー
    [SerializeField] private string hpTextTag = "HPText"; // HPを表示するテキストのタグ
    [SerializeField] GameObject deathEffectPrefab; // 死亡時に表示するエフェクトのPrefab
    [SerializeField] private Animator PlayerAnimator; // プレイヤーのアニメーター
    private int playerDamage; // 算出されたダメージ量
    private Text hpText; // HPを表示するテキスト

    void Start()
    {
        // タグを使用してHPテキストを取得
        GameObject hpTextObject = GameObject.FindWithTag(hpTextTag);
        if (hpTextObject != null)
        {
            hpText = hpTextObject.GetComponent<Text>();
        }

        // プレイヤーのステータスが設定されている場合、HPを初期化
        if (charadata != null)
        {
            Slider.value = 1; // HPスライダーの初期値
            charadata.HP = charadata.MAXHP; // 最大HPを現在のHPに設定
            UpdateHPText(); // HPテキストを更新
        }
    }

    public void Damage(int value)
    {
        if (charadata != null)
        {
            // ダメージを計算（敵の攻撃値 - プレイヤーの防御値）
            playerDamage = value - charadata.DEF;
            if (playerDamage <= 0)
            {
                playerDamage = 1; // ダメージが0以下の場合は1に設定
            }
            charadata.HP -= playerDamage; // 現在のHPからダメージを引く
            Slider.value = (float)charadata.HP / (float)charadata.MAXHP; // HPスライダーを更新
            UpdateHPText(); // HPテキストを更新
        }

        if (charadata.HP <= 0) // HPが0以下になった場合
        {
            PlayerAnimator.SetBool("death", true); // Deathアニメーションを再生
            StartCoroutine(WaitForDeathAnimation()); // アニメーション終了後の処理をコルーチンで実行
        }
    }

    private IEnumerator WaitForDeathAnimation()
    {
        // Deathアニメーションが終了するまで待機
        yield return new WaitForSeconds(PlayerAnimator.GetCurrentAnimatorStateInfo(0).length);
        Instantiate(deathEffectPrefab, transform.position, Quaternion.identity); // 死亡エフェクトを生成
        yield return new WaitForSeconds(2.0f); // エフェクト生成後に1秒待機
        HandleRespawn(); // リスポーン処理を実行
    }

    private void HandleRespawn()
    {
        // 現在のシーン名を取得
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != "CityScene") // 現在のシーンがCitySceneでない場合
        {
            SceneManager.LoadScene("CityScene", LoadSceneMode.Single); // CitySceneに遷移
            StartCoroutine(RespawnInCity()); // CitySceneに遷移後、リスポーン処理を実行
        }
        else
        {
            RespawnAtPosition(new Vector3(0, 0.2f, -8)); // CitySceneの場合、指定位置にリスポーン
        }
    }

    private IEnumerator RespawnInCity()
    {
        // CitySceneが読み込まれるまで待機
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "CityScene");
        RespawnAtPosition(new Vector3(0, 0.2f, -8)); // 指定位置にリスポーン
    }

    private void RespawnAtPosition(Vector3 position)
    {
        // プレイヤーを指定位置に移動
        transform.position = position;
        charadata.HP = charadata.MAXHP; // HPを最大値にリセット
        Slider.value = 1; // HPスライダーを最大値にリセット
        UpdateHPText(); // HPテキストを更新
        PlayerAnimator.SetBool("death", false); // Deathアニメーションのフラグを解除
    }

    public void Death()
    {
        Destroy(gameObject); // ゲームオブジェクトを破壊
    }

    public void UpdateHPText()
    {
        if (hpText != null && charadata != null)
        {
            hpText.text = $"{charadata.HP}/{charadata.MAXHP}"; // HPテキストを更新
        }
    }
}

