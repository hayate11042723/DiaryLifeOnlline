using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMuki : MonoBehaviour
{
    // インスペクターで設定するカメラ
    public Camera targetCamera;

    // Update is called once per frame
    void Update()
    {
        if (targetCamera != null)
        {
            // カメラの向きを取得
            Vector3 cameraForward = targetCamera.transform.forward;
            cameraForward.y = 0; // 上下の向きを無視
            cameraForward.Normalize(); // 正規化

            // カメラの向きに合わせてオブジェクトの向きを設定
            transform.rotation = Quaternion.LookRotation(cameraForward);
        }
    }
}