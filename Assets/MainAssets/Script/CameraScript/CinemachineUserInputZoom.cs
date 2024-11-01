using Cinemachine;
using UnityEngine;

public class CinemachineUserInputZoom : CinemachineExtension
{
    // Input Manager�̓��͖�
    [SerializeField] private string _inputName = "Mouse ScrollWheel";

    // ���͒l�Ɋ|����l
    [SerializeField] private float _inputScale = 100;

    // FOV�̍ŏ��E�ő�
    [SerializeField, Range(1, 179)] private float _minFOV = 10;
    [SerializeField, Range(1, 179)] private float _maxFOV = 90;

    // �ω����邨���悻�̎���[s]
    [SerializeField] private float _smoothTime = 0.1f;

    // �ω��̍ő呬�x
    [SerializeField] private float _maxSpeed = Mathf.Infinity;

    // ���[�U�[���͂�K�v�Ƃ���
    public override bool RequiresUserInput => true;

    private float _scrollDelta;

    // ���炩�ɕω�����FOV�̌v�Z�p�ϐ�
    private float _targetAdjustFOV;
    private float _currentAdjustFOV;
    private float _currentAdjustFOVVelocity;

    // Input System�̓��͂��󂯎�邽�߂̃R���|�[�l���g
    private CinemachineInputProvider _inputProvider;

    protected override void Awake()
    {
        base.Awake();

        // Input System�̓��͎󂯎�茳
        _inputProvider = GetComponent<CinemachineInputProvider>();

        if (_inputProvider != null)
        {
            // �X�N���[���ʂ����Z(Input System)
            _inputProvider.ZAxis.action.performed +=
                context => _scrollDelta = context.ReadValue<float>();
        }
    }

    private void Update()
    {
        // Input System�g�p����Input Manager��API���Q�Ƃ��Ȃ�
        if (_inputProvider != null)
            return;

        // �X�N���[���ʂ����Z(Input Manager)
        _scrollDelta += Input.GetAxis(_inputName);
    }

    // �e�X�e�[�W���Ɏ��s�����R�[���o�b�N
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        // Aim�̒��ゾ�����������{
        if (stage != CinemachineCore.Stage.Aim)
            return;

        var lens = state.Lens;

        if (!Mathf.Approximately(_scrollDelta, 0))
        {
            // FOV�̕␳�ʂ��v�Z
            _targetAdjustFOV = Mathf.Clamp(
                _targetAdjustFOV - _scrollDelta * _inputScale,
                _minFOV - lens.FieldOfView,
                _maxFOV - lens.FieldOfView
            );

            _scrollDelta = 0;
        }

        // ���炩�ɕω�����FOV�l���v�Z
        _currentAdjustFOV = Mathf.SmoothDamp(
            _currentAdjustFOV,
            _targetAdjustFOV,
            ref _currentAdjustFOVVelocity,
            _smoothTime,
            _maxSpeed,
            deltaTime
        );

        // state�̓��e�͖��񃊃Z�b�g�����̂ŁA
        // ����␳����K�v������
        lens.FieldOfView += _currentAdjustFOV;

        state.Lens = lens;
    }
}