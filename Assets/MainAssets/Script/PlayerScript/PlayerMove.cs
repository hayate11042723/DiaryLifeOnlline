using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 moving, latestPos;
    private float speed;

    private bool isRun = false;
    private bool isIdle = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 20;
    }

    // Update is called once per frame
    void Update()
    {
        // W�L�[�i�O���ړ��j
        if (Input.GetKey(KeyCode.W))
        {
            rb.velocity = transform.forward * speed;
        }

        // S�L�[�i����ړ��j
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity = -transform.forward * speed;
        }

        // D�L�[�i�E�ړ��j
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = transform.right * speed;
        }

        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = -transform.right * speed;
        }
    }


}