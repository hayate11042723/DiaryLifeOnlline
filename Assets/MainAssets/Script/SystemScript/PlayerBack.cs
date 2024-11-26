using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerBack : MonoBehaviour
{
    public GameObject Player;
    public GameObject Camera;
    public Vector3 pos, rot;

    public void OnBack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (SceneManager.GetActiveScene().name == "CityScene")
            {
                Player.transform.position = new Vector3(pos.x, pos.y, pos.z);
                Player.transform.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
            }
            else
            {
                SceneManager.LoadScene("CityScene");

                Player.transform.position = new Vector3(pos.x, pos.y, pos.z);
                Player.transform.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
                Camera.transform.position = new Vector3(pos.x, pos.y, pos.z);
                Camera.transform.eulerAngles = new Vector3(rot.x, rot.y, rot.z);
            }
        }
    }
}
