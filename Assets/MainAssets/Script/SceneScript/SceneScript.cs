using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public Vector3 pos; // シーン遷移後のプレイヤーの位置
    public GameObject cameraObject; // カメラオブジェクト
    GameObject[] tagObjects; // "Player"タグを持つオブジェクトの配列
    public string tagname; // シーン遷移のトリガーとなるオブジェクトのタグ名
    public string sceneName; // 遷移先のシーン名

    private void Awake()
    {
        // "Player"タグを持つオブジェクトをすべて取得
        tagObjects = GameObject.FindGameObjectsWithTag("Player");

        if (tagObjects.Length >= 2)
        {
            // 2つ以上の"Player"タグを持つオブジェクトが存在する場合
            // 削除前にリスナーを解除
            var existingScript = tagObjects[1].GetComponent<FocusOnEnemy>();
            if (existingScript != null)
            {
                existingScript.enabled = false;
                existingScript.OnDisable();
            }

            // 2番目の"Player"タグを持つオブジェクトを削除
            Destroy(tagObjects[1].gameObject);
            // カメラオブジェクトを削除
            Destroy(cameraObject);
        }
        else
        {
            // オブジェクトをシーン遷移後も破棄しないように設定
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(cameraObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // シーン遷移の条件
        if (other.CompareTag(tagname))
        {
            // シーンの読み込み
            SceneManager.LoadScene(sceneName);
            // Playerの遷移後の座標を設定
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
            // カメラの遷移後の座標を設定
            cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 3f);
        }
    }
}