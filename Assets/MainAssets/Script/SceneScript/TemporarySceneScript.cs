using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TemporarySceneScript : MonoBehaviour
{
    public string tagname; // シーン遷移のトリガーとなるオブジェクトのタグ名
    public string sceneName; // 遷移先のシーン名

    // 遷移先のシーンで調整するオブジェクトの名前
    public string targetObjectName;

    // オブジェクトの新しい位置
    public Vector3 newPosition;

    // オブジェクトの新しい向き
    public Vector3 newRotation;

    private void OnTriggerStay(Collider other)
    {
        // シーン遷移の条件
        if (other.CompareTag(tagname))
        {
            // オブジェクトの位置情報を保存
            PlayerPrefs.SetString("TargetObjectName", targetObjectName);
            PlayerPrefs.SetFloat("NewPositionX", newPosition.x);
            PlayerPrefs.SetFloat("NewPositionY", newPosition.y);
            PlayerPrefs.SetFloat("NewPositionZ", newPosition.z);

            // オブジェクトの向き情報を保存
            PlayerPrefs.SetFloat("NewRotationX", newRotation.x);
            PlayerPrefs.SetFloat("NewRotationY", newRotation.y);
            PlayerPrefs.SetFloat("NewRotationZ", newRotation.z);

            // シーンの読み込み
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneName);
        }
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

                float newRotationX = PlayerPrefs.GetFloat("NewRotationX");
                float newRotationY = PlayerPrefs.GetFloat("NewRotationY");
                float newRotationZ = PlayerPrefs.GetFloat("NewRotationZ");
                Quaternion newRotation = Quaternion.Euler(newRotationX, newRotationY, newRotationZ);

                // オブジェクトの位置と向きを調整
                targetObject.transform.position = newPosition;
                targetObject.transform.rotation = newRotation;
            }
        }

        // イベントの解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
