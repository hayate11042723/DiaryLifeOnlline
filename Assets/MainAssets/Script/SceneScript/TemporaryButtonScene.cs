using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporaryButtonScene : MonoBehaviour
{
    // 遷移先のシーン名を指定する
    public string nextSceneName;

    // 遷移先のシーンで調整するオブジェクトの名前
    public string targetObjectName;

    // オブジェクトの新しい位置
    public Vector3 newPosition;

    // ボタンを押したときに呼び出されるメソッド
    public void ChangeScene()
    {
        // オブジェクトの位置情報を保存
        PlayerPrefs.SetString("TargetObjectName", targetObjectName);
        PlayerPrefs.SetFloat("NewPositionX", newPosition.x);
        PlayerPrefs.SetFloat("NewPositionY", newPosition.y);
        PlayerPrefs.SetFloat("NewPositionZ", newPosition.z);

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(nextSceneName);
    }

    // シーンがロードされたときに呼び出されるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 保存されたオブジェクトの位置情報を取得
        string targetObjectName = PlayerPrefs.GetString("TargetObjectName", "");
        if (!string.IsNullOrEmpty(targetObjectName))
        {
            GameObject targetObject = GameObject.Find(targetObjectName);
            if (targetObject != null)
            {
                float newPositionX = PlayerPrefs.GetFloat("NewPositionX");
                float newPositionY = PlayerPrefs.GetFloat("NewPositionY");
                float newPositionZ = PlayerPrefs.GetFloat("NewPositionZ");
                Vector3 newPosition = new Vector3(newPositionX, newPositionY, newPositionZ);

                // オブジェクトの位置を調整
                targetObject.transform.position = newPosition;
            }
        }

        // イベントの解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}