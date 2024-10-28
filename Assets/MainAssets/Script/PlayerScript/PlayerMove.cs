using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private bool isRun = false;
    private bool isIdle = true;

    private Rigidbody rb;
    public float movementSpeed = 5f;
    private bool moving;
    private float horizontal;
    private float vertical;
    public Vector3 worldPoint;

    public Animator PlayerAnimator;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    { 
        if (moving)
        {
            var localPonint = transform.InverseTransformPoint(worldPoint);
            Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * movementSpeed;
            rb.AddForce(movement, ForceMode.VelocityChange);

            // �ړ����x���A�j���[�^�[�� "Speed" �p�����[�^�[�ɓn��
            float currentSpeed = Mathf.Clamp01(movement.magnitude / movementSpeed); // 0����1�͈̔͂ɃN�����v

            // �v���C���[�̌������ړ������ɍ��킹��
            if (movement.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 1000f);
            }
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