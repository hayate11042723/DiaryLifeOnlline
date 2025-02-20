using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float movementSpeed;
    public float d_movementSpeed;
    private Rigidbody rb;
    private bool moving;
    private bool d_moving;
    private float horizontal;
    private float vertical;

    public Animator PlayerAnimator;
    private PlayerAttack playerAttack; // PlayerAttackのインスタンスを追加

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAttack = GetComponent<PlayerAttack>(); // PlayerAttackのコンポーネントを取得
    }

    public void Update()
    {
        if (playerAttack != null && playerAttack.isAttacking) // 攻撃中は入力を無視
        {
            horizontal = 0;
            vertical = 0;
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (moving)
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
        else if (d_moving)
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 moveForward = cameraForward * vertical + Camera.main.transform.right * horizontal;

            // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
            rb.velocity = moveForward * d_movementSpeed + new Vector3(0, rb.velocity.y, 0);

            // キャラクターの向きを進行方向に
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (playerAttack != null && playerAttack.isAttacking) // 攻撃中は入力を無視
        {
            return;
        }

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
