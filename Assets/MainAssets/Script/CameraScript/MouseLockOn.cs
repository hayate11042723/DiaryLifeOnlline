using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Linq;

public class MouseLockOn : MonoBehaviour
{
    public Transform player;                        // プレイヤーのTransform
    public CinemachineVirtualCamera virtualCamera; // Cinemachineの仮想カメラ
    public float lockOnRange = 15f;                // ロックオン可能な範囲
    public LayerMask targetLayer;                  // ロックオン対象のレイヤー
    public GameObject lockOnUI;                    // ロックオン中に表示するUI

    private Transform currentTarget;               // 現在のロックオン対象
    private Collider[] potentialTargets;           // ロックオン可能なターゲットリスト

    void Update()
    {
        // ロックオン切り替え（例: Tabキーで最も近いターゲットに切り替え）
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchTarget();
        }

        // ロックオンのオン/オフ
        if (Input.GetKeyDown(KeyCode.L))
        {
            ToggleLockOn();
        }

        // UI更新
        if (currentTarget != null && lockOnUI != null)
        {
            lockOnUI.SetActive(true);
            lockOnUI.transform.position = Camera.main.WorldToScreenPoint(currentTarget.position);
        }
        else if (lockOnUI != null)
        {
            lockOnUI.SetActive(false);
        }
    }

    void SwitchTarget()
    {
        potentialTargets = Physics.OverlapSphere(player.position, lockOnRange, targetLayer);

        if (potentialTargets.Length == 0)
        {
            currentTarget = null;
            return;
        }

        // 最も近いターゲットを探す
        currentTarget = potentialTargets
            .OrderBy(t => Vector3.Distance(player.position, t.transform.position))
            .FirstOrDefault()?.transform;

        if (virtualCamera != null && currentTarget != null)
        {
            virtualCamera.LookAt = currentTarget;
        }
    }

    void ToggleLockOn()
    {
        if (currentTarget != null)
        {
            currentTarget = null; // ロックオン解除
            if (virtualCamera != null)
                virtualCamera.LookAt = null;
        }
        else
        {
            SwitchTarget(); // 新しいターゲットにロックオン
        }
    }
}