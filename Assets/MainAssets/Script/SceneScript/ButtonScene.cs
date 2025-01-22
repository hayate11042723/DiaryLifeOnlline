using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScene : MonoBehaviour
{
    // 遷移先のシーン名を指定する
    public string nextSceneName;
    public Vector3 pos;
    private GameObject playerObject;
    private GameObject cameraObject;

    private void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera");

        if (playerObject == null || cameraObject == null)
        {
            Debug.LogError("Playerまたはカメラオブジェクトが見つかりません。");
            return;
        }
        DontDestroyOnLoad(playerObject);
        DontDestroyOnLoad(cameraObject);
    }

    // ボタンを押したときに呼び出されるメソッド
    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
        // Playerの遷移後の座標
        playerObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        // カメラの遷移後の座標
        cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 3f);
    }
}