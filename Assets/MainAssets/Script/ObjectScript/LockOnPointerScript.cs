using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnPointerScript : MonoBehaviour
{
    // 動きの設定
    public float amplitude = 0.5f; // 上下移動の振幅
    public float speed = 1.0f;     // 動く速さ

    // 初期位置
    private Vector3 startPosition;

    void Start()
    {
        // オブジェクトの初期位置を保存
        startPosition = transform.position;
    }

    void Update()
    {
        // サイン波を使ってY座標を変更
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;

        // 新しい位置を設定
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}