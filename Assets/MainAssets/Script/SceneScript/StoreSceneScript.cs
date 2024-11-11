using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreSceneScript : MonoBehaviour
{
    public Vector3 pos,rot;
    public GameObject cameraObject;

    void Start()
    {
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(cameraObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GeneralStore"))
        {
            SceneManager.LoadSceneAsync("GeneralStoreScene");
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
            cameraObject.transform.position = new Vector3(pos.x, pos.y + 1f, pos.z - 18f);
        }
    }
}
