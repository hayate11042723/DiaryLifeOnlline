using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveKeyboard : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�v���C���[�̑��x����")]
    private float speed = 0;

    private Rigidbody rb;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[�ɃA�^�b�`����Ă���Rigidbody���擾
        rb = gameObject.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// �ړ�����i�㉺���E�L�[�Ȃǁj���擾
    /// </summary>
    /// <param name="movementValue"></param>
    private void OnMove(InputValue movementValue)
    {
        // Move�A�N�V�����̓��͒l���擾
        Vector2 movementVector = movementValue.Get<Vector2>();

        // x,y�������̓��͒l��ϐ��ɑ��
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        // ���͒l������3���x�N�g�����쐬
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        // rigidbody��AddForce���g�p���ăv���C���[�𓮂����B
        rb.AddForce(movement * speed);
    }

}