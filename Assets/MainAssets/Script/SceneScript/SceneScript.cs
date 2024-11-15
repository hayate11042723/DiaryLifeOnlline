using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    public Vector3 pos,rot;
    public GameObject cameraObject;
    GameObject[] tagObjects;
    public string tagname;
    public string sceneName;

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

    private void OnTriggerStay(Collider other)
    {
        // シーン遷移の条件
        if (other.CompareTag(tagname))
        {
            // シーンの読み込み
            SceneManager.LoadSceneAsync(sceneName);
            // Playerの遷移後の座標
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
            this.transform.Rotate (new Vector3(rot.x, rot.y, rot.z));
            // カメラの遷移後の座標
            cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 3f);
            cameraObject.transform.Rotate(new Vector3(rot.x, rot.y, rot.z));
        }
    }
}
