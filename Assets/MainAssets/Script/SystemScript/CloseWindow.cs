using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloseWindow : MonoBehaviour
{
    public void OnCloseWindow(InputAction.CallbackContext context)
    {
        Application.Quit();
    }
}
