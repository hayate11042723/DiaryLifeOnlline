using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStatus charadata; // プレイヤーのステータスデータ
    [SerializeField] public Slider Slider; // HPを表示するスライダー
    [SerializeField] GameObject deathEffectPrefab; // 死亡時に表示するエフェクトのPrefab
    [SerializeField] private Animator PlayerAnimator; // プレイヤーのアニメーター
    public int HP; // 現在のHP
    private int playerDamage; // 算出されたダメージ量

    void Start()
    {
        // プレイヤーのステータスが設定されている場合、HPを初期化
        if (charadata != null)
        {
            Slider.value = 1; // HPスライダーの初期値
            HP = charadata.MAXHP; // 最大HPを現在のHPに設定
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
            HP -= playerDamage; // 現在のHPからダメージを引く
            Slider.value = (float)HP / (float)charadata.MAXHP; // HPスライダーを更新
        }

        if (HP <= 0) // HPが0以下になった場合
        {
            Debug.Log("死"); // デバッグログを出力
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
        HP = charadata.MAXHP; // HPを最大値にリセット
        Slider.value = 1; // HPスライダーを最大値にリセット
        PlayerAnimator.SetBool("death", false); // Deathアニメーションのフラグを解除
        Debug.Log("プレイヤーが蘇生しました。"); // デバッグログを出力
    }

    public void Death()
    {
        Debug.Log("Player has died."); // デバッグログを出力
        Destroy(gameObject); // ゲームオブジェクトを破壊
    }
}
