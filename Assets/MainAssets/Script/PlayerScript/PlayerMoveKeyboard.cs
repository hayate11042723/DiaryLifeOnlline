using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveKeyboard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("プレイヤーの速度調整")]
    private float speed = 0;

    private Rigidbody rb;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        // プレイヤーにアタッチされているRigidbodyを取得
        rb = gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// 移動操作（上下左右キーなど）を取得
    /// </summary>
    /// <param name="movementValue"></param>
    private void OnMove(InputValue movementValue)
    {
        // Moveアクションの入力値を取得
        Vector2 movementVector = movementValue.Get<Vector2>();

        // x,y軸方向の入力値を変数に代入
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        // 入力値を元に3軸ベクトルを作成
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // rigidbodyのAddForceを使用してプレイヤーを動かす。
        rb.AddForce(movement * speed);
    }

}