using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerBack : MonoBehaviour
{
    private Vector3 initialPosition;  // 初期位置
    private Quaternion initialRotation; // 初期回転

    // Input Systemのアクション
    private PlayerInput playerInput;

    private void Awake()
    {
        // 初期位置と回転を保存
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // PlayerInputを取得
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // ResetPlayerアクションを有効化
        playerInput.actions["ResetPlayer"].performed += OnResetPlayer;
    }

    private void OnDisable()
    {
        // ResetPlayerアクションを無効化
        playerInput.actions["ResetPlayer"].performed -= OnResetPlayer;
    }

    public void OnResetPlayer(InputAction.CallbackContext context)
    {
        // 初期位置に戻す
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}