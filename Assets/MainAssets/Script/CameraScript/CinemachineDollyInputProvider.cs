using Cinemachine;
using UnityEngine;

/// <summary>
/// CinemachineFramingTransposer のカメラ前後方向のコントロールを付与するクラス。
/// このスクリプトを使用することで、プレイヤーの入力に応じてカメラ距離を動的に調整可能。
/// </summary>
public class CinemachineDollyInputProvider : MonoBehaviour
{
    // Cinemachine の仮想カメラ。
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera = null;

    // ユーザー入力を受け取るためのプロバイダ。
    [SerializeField]
    private CinemachineInputProvider _cinemachineInputProvider = null;

    // カメラ距離の調整感度。
    [SerializeField]
    private float _sensitivity;

    // カメラ距離の最小値。
    [SerializeField]
    private float _minDistance;

    // カメラ距離の最大値。
    [SerializeField]
    private float _maxDistance;

    /// <summary>
    /// カメラ感度のプロパティ。外部からの取得と設定が可能。
    /// </summary>
    public float Sensitivity
    {
        get => _sensitivity;
        set => _sensitivity = value;
    }

    /// <summary>
    /// オブジェクトが有効化された際に呼び出されるメソッド。
    /// </summary>
    private void OnEnable()
    {
        _cinemachineInputProvider.ZAxis.action.Enable();
        Debug.Log("Enable");
    }

    /// <summary>
    /// スクリプトが初期化される際に呼び出されるメソッド。
    /// CinemachineFramingTransposer コンポーネントを取得し、カメラ距離の調整ロジックを設定。
    /// </summary>
    private void Awake()
    {
        // 仮想カメラから FramingTransposer コンポーネントを取得。
        var cinemachineComponent = _cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);
        if (cinemachineComponent is CinemachineFramingTransposer cinemachineFramingTransposer)
        {
            // Z 軸の入力アクションにリスナーを登録。
            _cinemachineInputProvider.ZAxis.action.performed += context =>
            {
                // 現在のカメラ距離を計算し、設定された範囲内にクランプ。
                float distance = Mathf.Clamp(
                    cinemachineFramingTransposer.m_CameraDistance + context.ReadValue<float>() * _sensitivity,
                    _minDistance,
                    _maxDistance
                );

                // 計算した距離をカメラに適用。
                cinemachineFramingTransposer.m_CameraDistance = distance;
            };
        }
        else
        {
            // FramingTransposer コンポーネントが見つからない場合は警告を表示。
            Debug.LogWarning($"{nameof(CinemachineFramingTransposer)} が存在しません。");
        }
    }
}
