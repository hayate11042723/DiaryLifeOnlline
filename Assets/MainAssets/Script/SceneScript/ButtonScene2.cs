using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScene2 : MonoBehaviour
{
    // 遷移先のシーン名を指定する
    public string nextSceneName;
    public Vector3 pos;
    public GameObject cameraObject;
    GameObject[] tagObjects;

    private void Awake()
    {
        tagObjects = GameObject.FindGameObjectsWithTag("Player");

        if (tagObjects.Length >= 2)
        {
            Destroy(gameObject);
            Destroy(cameraObject);
        }
        else
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(cameraObject);
        }

    }

    // ボタンを押したときに呼び出されるメソッド
    public void ChangeScene()
    {
        SceneManager.LoadScene(nextSceneName);
        // Playerの遷移後の座標
        this.transform.position = new Vector3(pos.x, pos.y, pos.z);
        // カメラの遷移後の座標
        cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 3f);
    }
}