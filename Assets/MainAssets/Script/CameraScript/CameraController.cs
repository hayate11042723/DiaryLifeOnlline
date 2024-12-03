using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera playerFollowCamera;
    public CinemachineVirtualCamera lockOnCamera;
    public Transform player;
    public LayerMask enemyLayer;
    private Transform currentTarget;

    private bool isRightClicking = false;

    void Update()
    {
        HandleCameraControl();
        HandleLockOn();
    }

    private void HandleCameraControl()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            isRightClicking = true;
        }
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            isRightClicking = false;
        }

        if (isRightClicking)
        {
            // マウスの移動量を取得してカメラを回転
            float mouseX = Mouse.current.delta.x.ReadValue();
            float mouseY = Mouse.current.delta.y.ReadValue();

            // プレイヤーの周りでカメラを回転（適宜調整）
            playerFollowCamera.transform.RotateAround(player.position, Vector3.up, mouseX * 0.1f);
            playerFollowCamera.transform.RotateAround(player.position, transform.right, -mouseY * 0.1f);
        }
    }

    private void HandleLockOn()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 左クリック時にターゲットを取得
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, enemyLayer))
            {
                currentTarget = hit.transform;
                LockOnTarget();
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // ロックオン解除
            UnlockTarget();
        }
    }

    private void LockOnTarget()
    {
        if (currentTarget != null)
        {
            lockOnCamera.Follow = currentTarget;
            lockOnCamera.LookAt = currentTarget;

            // プレイヤーと敵の中間地点にカメラを配置
            Vector3 midPoint = (player.position + currentTarget.position) / 2;
            lockOnCamera.transform.position = midPoint;

            playerFollowCamera.gameObject.SetActive(false);
            lockOnCamera.gameObject.SetActive(true);
        }
    }

    private void UnlockTarget()
    {
        currentTarget = null;
        lockOnCamera.gameObject.SetActive(false);
        playerFollowCamera.gameObject.SetActive(true);
    }
}
