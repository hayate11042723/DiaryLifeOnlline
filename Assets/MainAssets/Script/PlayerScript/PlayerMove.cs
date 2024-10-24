using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private bool isRun = false;
    private bool isIdle = true;

    // �ړ����x
    public float Speed;
    // �_�b�V�����x
    public float DashSpeed;

    float x, z;
    Rigidbody rb;
    Vector3 moving;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // WASD�ł̈ړ�����
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moving = new Vector3(x, 0, z);

        // ���x�̌v�Z
        moving = moving.normalized * Speed;

        // Shift�_�b�V���̏���
        if (Input.GetKey(KeyCode.LeftShift))
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            moving = new Vector3(x, 0, z);
            
            // �_�b�V�����̑��x�v�Z
            moving = moving.normalized * Speed * DashSpeed;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moving;
    }
}