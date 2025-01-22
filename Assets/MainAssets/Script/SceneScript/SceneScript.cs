using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public Vector3 pos;
    public GameObject cameraObject;
    GameObject[] tagObjects;
    public string tagname;
    public string sceneName;

    private void Awake()
    {
        tagObjects = GameObject.FindGameObjectsWithTag("Player");

        if (tagObjects.Length >= 2)
        {
            // 削除前にリスナーを解除
            var existingScript = tagObjects[1].GetComponent<FocusOnEnemy>();
            if (existingScript != null)
            {
                existingScript.enabled = false;
                existingScript.OnDisable();
            }

            Destroy(tagObjects[1].gameObject); // オブジェクトを削除
            Destroy(cameraObject);            // カメラを削除
        }
        else
        {
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
            // Playerの遷移後の座標
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
            // カメラの遷移後の座標
            cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 3f);
        }
    }
}
