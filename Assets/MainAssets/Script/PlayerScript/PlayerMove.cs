using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rb;
    public float movementSpeed = 5f;
    private bool moving;
    private float horizontal;
    private float vertical;

    public Animator PlayerAnimator;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (moving)
        {
            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * movementSpeed;
            rb.AddForce(movement, ForceMode.VelocityChange);

            // 移動速度をアニメーターの "Speed" パラメーターに渡す
            float currentSpeed = Mathf.Clamp01(movement.magnitude / movementSpeed); // 0から1の範囲にクランプ

            //// プレイヤーの向きを移動方向に合わせる
            //if (movement.magnitude > 0)
            //{
            //    Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            //    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 1000f);
            //}
        }
    }

    void FixedUpdate()
    {
        // カメラの方向から、X-Z平面の単位ベクトルを取得
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // 方向キーの入力値とカメラの向きから、移動方向を決定
        Vector3 moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        rb.velocity = moveForward * movementSpeed + new Vector3(0, rb.velocity.y, 0);

        // キャラクターの向きを進行方向に
        if (moveForward != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForward);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 movementInput = context.ReadValue<Vector2>();
        horizontal = movementInput.x;
        vertical = movementInput.y;

        if (context.performed)
        {
            moving = true;
            PlayerAnimator.SetBool("run", true);
        }
        else if (context.canceled)
        {
            moving = false;
            PlayerAnimator.SetBool("run", false);
        }
    }
}