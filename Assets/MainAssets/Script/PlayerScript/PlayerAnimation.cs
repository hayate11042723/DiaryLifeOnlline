using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator PlayerAnimator;

    private void FixedUpdate()
    {
        OnMove();
    }

    public void OnMove()
    {
        if (Input.GetKey(KeyCode.W))
        {
            PlayerAnimator.SetBool("run", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            PlayerAnimator.SetBool("run", true);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            PlayerAnimator.SetBool("run", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            PlayerAnimator.SetBool("run", true);
        }
        else
        {
            PlayerAnimator.SetBool("run", false);
        }

    }

}
