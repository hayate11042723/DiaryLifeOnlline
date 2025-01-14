using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocusOnEnemy : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    [SerializeField]
    private Transform _playerTransform;

    [SerializeField]
    private InputActionReference _clickAction; // 左クリックアクション

    [SerializeField]
    private InputActionReference _rightClickDragAction; // 右ドラッグのInputAction

    [SerializeField]
    private CinemachineInputProvider _inputProvider; // CinemachineのInputProvider

    private Transform _currentLookTarget; // 現在のターゲット
    private bool _isFocusedOnEnemy = false; // カメラがEnemyを注視しているか
    private bool _isDragging = false; // 右ドラッグ中かどうか

    private void Start()
    {
        if (_cinemachineVirtualCamera == null)
        {
            _cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        }

        if (_inputProvider == null)
        {
            _inputProvider = GetComponent<CinemachineInputProvider>();
        }
    }

    private void OnEnable()
    {
        // アクションを有効化
        _clickAction.action.Enable();
        _clickAction.action.performed += OnLeftClickPerformed;

        _rightClickDragAction.action.Enable();
        _rightClickDragAction.action.started += OnRightClickDragStart;
        _rightClickDragAction.action.canceled += OnRightClickDragEnd;

        if (_inputProvider != null)
        {
            _inputProvider.enabled = true;
        }
    }

    private void OnDisable()
    {
        // アクションを無効化
        _clickAction.action.performed -= OnLeftClickPerformed;
        _clickAction.action.Disable();

        _rightClickDragAction.action.started -= OnRightClickDragStart;
        _rightClickDragAction.action.canceled -= OnRightClickDragEnd;
        _rightClickDragAction.action.Disable();

        if (_inputProvider != null)
        {
            _inputProvider.enabled = false;
        }
    }

    private void OnLeftClickPerformed(InputAction.CallbackContext context)
    {
        if (context.control.name == "leftButton")
        {
            if (_isFocusedOnEnemy)
            {
                ResetCamera();
            }
            else
            {
                TryFocusOnClickedEnemy();
            }
        }
    }

    private void OnRightClickDragStart(InputAction.CallbackContext context)
    {
        if (context.control.name == "rightButton")
        {
            _isDragging = true;
            if (_inputProvider != null)
            {
                _inputProvider.enabled = true; // CinemachineInputProviderを有効化
                Debug.Log("右クリック"); // シーン切り替え前は『正』、シーン切り替え後は『負』
            }
        }
    }

    private void OnRightClickDragEnd(InputAction.CallbackContext context)
    {
        if (context.control.name == "rightButton")
        {
            _isDragging = false;
            if (!_isFocusedOnEnemy && _inputProvider != null)
            {
                _inputProvider.enabled = true; // ターゲットがない場合は引き続き有効
            }
        }
    }

    private void TryFocusOnClickedEnemy()
    {
        if (_isDragging) return; // 右ドラッグ中はクリック無効

        // Raycastでクリックしたオブジェクトを検出
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Enemyタグが付いているオブジェクトを判定
            if (hit.transform.CompareTag("Enemy"))
            {
                FocusCameraOnEnemy(hit.transform);
            }
        }
    }

    private void FocusCameraOnEnemy(Transform enemyTransform)
    {
        if (_cinemachineVirtualCamera != null && enemyTransform != null)
        {
            // 現在のCinemachineComponentBaseを取得
            var composer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();

            // Aimモードを変更する例 (POVを削除してDoNothingに変更)
            if (composer != null)
            {
                _cinemachineVirtualCamera.DestroyCinemachineComponent<CinemachinePOV>();
            }

            // 新たにAimを指定 (例えば、GroupComposerを設定)
            _cinemachineVirtualCamera.AddCinemachineComponent<CinemachineGroupComposer>();

            // LookAtをクリックしたEnemyに設定
            _cinemachineVirtualCamera.LookAt = enemyTransform;
            _currentLookTarget = enemyTransform;

            // CinemachineInputProviderを無効化（Enemyにフォーカス中は操作を禁止）
            if (_inputProvider != null)
            {
                _inputProvider.enabled = false;
            }

            _isFocusedOnEnemy = true;
        }
    }

    private void ResetCamera()
    {
        // 現在のCinemachineComponentBaseを取得
        var composer = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineGroupComposer>();

        // Aimモードを変更する例 (POVを削除してDoNothingに変更)
        if (composer != null)
        {
            _cinemachineVirtualCamera.DestroyCinemachineComponent<CinemachineGroupComposer>();
        }

        // 新たにAimを指定 (例えば、GroupComposerを設定)
        _cinemachineVirtualCamera.AddCinemachineComponent<CinemachinePOV>();

        // カメラをプレイヤーにリセット
        if (_cinemachineVirtualCamera != null)
        {
            _cinemachineVirtualCamera.LookAt = _playerTransform;
        }

        _currentLookTarget = null;
        _isFocusedOnEnemy = false;

        // CinemachineInputProviderを有効化
        if (_inputProvider != null)
        {
            _inputProvider.enabled = true;
        }
    }
}