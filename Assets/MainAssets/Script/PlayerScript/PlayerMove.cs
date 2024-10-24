using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private bool isRun = false;
    private bool isIdle = true;

    // 移動速度
    public float Speed;
    // ダッシュ速度
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
        // WASDでの移動処理
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moving = new Vector3(x, 0, z);

        // 速度の計算
        moving = moving.normalized * Speed;

        // Shiftダッシュの処理
        if (Input.GetKey(KeyCode.LeftShift))
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            moving = new Vector3(x, 0, z);
            
            // ダッシュ時の速度計算
            moving = moving.normalized * Speed * DashSpeed;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moving;
    }
}