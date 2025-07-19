// ------------------------------------------------------------------------------------------------
// �����: ��Él��
// �����: 2025/06/07
// �d�l�@: ���͂��e�f�o�C�X����󂯎��A�K�؂Ȕ�����s��
// ------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Input_Player : MonoBehaviour
{
	// �ϐ�

	// �V���O���g��	-------------------------------------------------------------------------------
	private InputSystemAction _input;
	public static Input_Player Instance;

	// ���͊J�n�����u��	---------------------------------------------------------------------------
	// �E�ړ�
	private bool _rightMove_started;
	public bool RightMove_Started => _rightMove_started;
	// ���ړ�
	private bool _leftMove_started;
	public bool LeftMove_Started => _leftMove_started;
	// �E�_�b�V��
	private bool _rightDash_started;
	public bool RightDash_Started => _rightDash_started;

	// ���͂��Ă����	-------------------------------------------------------------------------------
	// �E�ړ�
	private bool _rightMove_performed;
	public bool RightMove_performed => _rightMove_performed;
	// ���ړ�
	private bool _leftMove_performed;
	public bool LeftMove_Performed => _leftMove_performed;
	// �E�_�b�V��
	private bool _rightDash_performed;
	public bool RightDash_Performed => _rightDash_performed;

	// ���͉��������u��	---------------------------------------------------------------------------
	// �E�ړ�
	private bool _rightMove_canceled;
	public bool rightMove_Canceled => _rightMove_canceled;
	// ���ړ�
	private bool _leftMove_canceled;
	public bool LeftMove_Canceled => _leftMove_canceled;
	// �E�_�b�V��
	private bool _rightDash_canceled;
	public bool RightDash_Canceled => _rightDash_canceled;

	// �C�x���g�n���h��	---------------------------------------------------------------------------
	// �E�ړ�
	private void OnRightMove_started(InputAction.CallbackContext ctx) => _rightMove_started = true;
	private void OnRightMove_canceled(InputAction.CallbackContext ctx) => _rightMove_canceled = true;
	// ���ړ�
	private void OnLeftMove_started(InputAction.CallbackContext ctx) => _leftMove_started = true;
	private void OnLeftMove_canceled(InputAction.CallbackContext ctx) => _leftMove_canceled = true;
	// �E�_�b�V��
	private void OnRightDash_started(InputAction.CallbackContext ctx) => _rightDash_started = true;
	private void OnRightDash_canceled(InputAction.CallbackContext ctx) => _rightDash_canceled = true;

	// 1�����I����	-------------------------------------------------------------------------------
	// �R���g���[���[�́~�{�^��
	private bool _gamepadSouthButton = false;

	// 2,3�����I����	-------------------------------------------------------------------------------
	// �R���g���[���[�̍��X�e�B�b�N
	private Vector2 _gamepadLeftStick = Vector2.zero;
	// Joy-con�̍������x
	private Vector3 _joyconLeftAccel = Vector3.zero;

	// ����ϐ�	-----------------------------------------------------------------------------------
	// �E�ړ����̑���f�o�C�X
	bool _rightMove_vec2 = false;
	bool _rightMove_vec3 = false;
	bool _rightMove_button = false;
	// �E�ړ����̑���f�o�C�X
	bool _leftMove_vec2 = false;
	bool _leftMove_button = false;
	// �E�ړ����̑���f�o�C�X
	bool _rightDash_vec2 = false;
	bool _rightDash_vec3 = false;
	bool _rightDash_button = false;
	// ����p���̎c�莞��
	private float _accelTime = 0.0f;

	// �ړ�����̂ɕK�v�ȃX�e�B�b�N�̌X��
	private const float _RIGHTMOVE_STICKVALUE = 0.0f;
	// �ړ�����̂ɕK�v�ȉ����x
	private const float _RIGHTMOVE_ACCELVALUE = 0.33f;
	// �_�b�V������̂ɕK�v�ȉ����x
	private const float _RIGHTDASH_ACCELVALUE = 2.5f;
	// ����p������
	private const float _RIGHTMOVE_ACCELTIME = 0.08f;

	// Joy-Con	-----------------------------------------------------------------------------------
	// Joy-Con���X�g�i�����ڑ����ꂽJoy-Con�̏��j
	private List<Joycon> _joycons;
	// Joy-Con��
	private Joycon _joyconL;



	// �֐�

	/// <summary>
	/// �{�^�����͂ɂ�鑀����󂯕t����
	/// </summary>
	public void Action_Button()
	{
		// �E�ړ�	-------------------------------------------------------------------------------
		// ���͊J�n
		if (_rightMove_started)
		{
			_rightMove_started = false;
			_rightMove_button = true;
		}
		// ���͉���
		else if (_rightMove_canceled)
		{
			_rightMove_canceled = false;
			_rightMove_button = false;
		}

		// ���ړ�	-------------------------------------------------------------------------------
		// ���͊J�n
		if (_leftMove_started)
		{
			_leftMove_started = false;
			_leftMove_button = true;
		}
		// ���͉���
		if (_leftMove_canceled)
		{
			_leftMove_canceled = false;
			_leftMove_button = false;
		}

		// �E�_�b�V��	---------------------------------------------------------------------------
		// ���͊J�n
		if (_rightDash_started)
		{
			_rightDash_started = false;
			_rightDash_button = true;
		}
		// ���͉���
		else if (_rightDash_canceled)
		{
			_rightDash_canceled = false;
			_rightDash_button = false;
		}
	}

	/// <summary>
	/// 2�����I����(�X�e�B�b�N��)�ɂ�鑀����󂯕t����
	/// </summary>
	private void Action_Vec2()
	{
		// �E�ړ�	-------------------------------------------------------------------------------
		// ���l�܂ŃX�e�B�b�N���X���Ă���
		if (_gamepadLeftStick.x > _RIGHTMOVE_STICKVALUE && !_gamepadSouthButton)
		{
			_rightMove_vec2 = true;
		}
		else
		{
			_rightMove_vec2 = false;
		}

		// ���ړ�	-------------------------------------------------------------------------------
		// ���l�܂ŃX�e�B�b�N���X���Ă���
		if (_gamepadLeftStick.x < -_RIGHTMOVE_STICKVALUE)
		{
			_leftMove_vec2 = true;
		}
		else
		{
			_leftMove_vec2 = false;
		}

		// �E�_�b�V��	---------------------------------------------------------------------------
		// ���l�܂ŃX�e�B�b�N���X���Ă���
		if (_gamepadLeftStick.x > _RIGHTMOVE_STICKVALUE && _gamepadSouthButton)
		{
			_rightDash_vec2 = true;
		}
		else
		{
			_rightDash_vec2 = false;
		}
	}

	/// <summary>
	/// 2�����I����(Joycon�����x��)�ɂ�鑀����󂯕t����
	/// </summary>
	private void Action_Vec3()
	{
		// Joy-conL�̉����x���Βl��
		Vector3 joyconAccel = AbsoluteVec3(_joyconLeftAccel);

		// �E�ړ�	-------------------------------------------------------------------------------
		// �����ꂩ�̎��̉����x�����ȏォ�A�E�_�b�V�����łȂ��ꍇ
		if ((((joyconAccel.x > _RIGHTMOVE_ACCELVALUE && joyconAccel.x < _RIGHTDASH_ACCELVALUE) ||
			  (joyconAccel.y > _RIGHTMOVE_ACCELVALUE && joyconAccel.y < _RIGHTDASH_ACCELVALUE) ||
			  (joyconAccel.z > _RIGHTMOVE_ACCELVALUE && joyconAccel.z < _RIGHTDASH_ACCELVALUE)) && !_rightDash_vec3))
		{
			_rightMove_vec3 = true;
			_rightDash_vec3 = false;
			_accelTime = _RIGHTMOVE_ACCELTIME;
		}

		// �E�_�b�V��	---------------------------------------------------------------------------
		// �����ꂩ�̎��̉����x�����ȏ�̏ꍇ
		else if (joyconAccel.x >= _RIGHTDASH_ACCELVALUE ||
				 joyconAccel.y >= _RIGHTDASH_ACCELVALUE ||
				 joyconAccel.z >= _RIGHTDASH_ACCELVALUE)
		{
			_rightDash_vec3 = true;
			_rightMove_vec3 = false;
			_accelTime = _RIGHTMOVE_ACCELTIME;
		}

		// �����x�̃����ɂ��e�����y������
		if (_accelTime > 0)
		{
			_accelTime -= Time.deltaTime;
		}
		else
		{
			_accelTime = 0;
			_rightMove_vec3 = false;
			_rightDash_vec3 = false;
		}
	}

	/// <summary>
	/// �A�N�V������L���ɂ���
	/// </summary>
	private void Action()
	{
		// �E�ړ�
		_rightMove_performed = _rightMove_button || _rightMove_vec2 || _rightMove_vec3;
		// ���ړ�
		_leftMove_performed = _leftMove_button || _leftMove_vec2;
		// �E�_�b�V��
		_rightDash_performed = _rightDash_button || _rightDash_vec2 || _rightDash_vec3;
	}

	/// <summary>
	/// �O���f�o�C�X�̓��͏����擾����
	/// </summary>
	private void GetInput()
	{
		// �Q�[���p�b�hL�X�e�B�b�N
		if (Gamepad.all.Count > 0) _gamepadLeftStick = Gamepad.current.leftStick.ReadValue();
		// �Q�[���p�b�h�~�{�^��
		if (Gamepad.all.Count > 0) _gamepadSouthButton = Gamepad.current.buttonSouth.isPressed;

		// Joy-conL�����x
		if (_joycons.Count > 0) _joyconLeftAccel = _joyconL.GetAccel();

		// Joy-con�̉����x�𐳏�ɖ߂����߂̃I�t�Z�b�g
		Vector3 ofset = Vector3.zero;
		// x��
		if (_joyconLeftAccel.x > 1.0f) ofset.x = 1.0f;
		else if (_joyconLeftAccel.x < -1.0f) ofset.x = -1.0f;
		else ofset.x = _joyconLeftAccel.x;
		// y��
		if (_joyconLeftAccel.y > 1.0f) ofset.y = 1.0f;
		else if (_joyconLeftAccel.y < -1.0f) ofset.y = -1.0f;
		else ofset.y = _joyconLeftAccel.y;
		// z��
		if (_joyconLeftAccel.z > 1.0f) ofset.z = 1.0f;
		else if (_joyconLeftAccel.z < -1.0f) ofset.z = -1.0f;
		else ofset.z = _joyconLeftAccel.z;
		// ����
		_joyconLeftAccel -= ofset;
	}

	/// <summary>
	/// �ڑ����ꂽJoyCon���擾����
	/// </summary>
	private void FindJoycon()
	{
		// Joy-Con�̃��X�g���擾
		_joycons = JoyconManager.Instance.j;

		// Joycon��ڑ�����
		if (_joycons.Count > 0)
		{
			_joyconL = _joycons[0];
			Debug.Log("Joy-Con���ڑ�����܂����B");

		}
		else
		{
			Debug.LogWarning("Joy-Con���ڑ�����Ă��܂���B");
		}

		// Debug: �Q�[���p�b�h�̐ڑ����m�F����
		if (Gamepad.all.Count > 0)
		{
			Debug.Log("�Q�[���p�b�h���ڑ�����܂����B");
		}
		else
		{
			Debug.LogWarning("�Q�[���p�b�h���ڑ�����Ă��܂���B");
		}
	}

	/// <summary>
	/// �w�肵��Vector3�^�̕ϐ��̐�Βl���o�͂���
	/// </summary>
	/// <returns></returns>
	private Vector3 AbsoluteVec3(Vector3 vec3)
	{
		return new Vector3(
			Mathf.Abs(vec3.x),
			Mathf.Abs(vec3.y),
			Mathf.Abs(vec3.z)
			);
	}

	/// <summary>
	/// �f�o�b�O
	/// </summary>
	public void PrintText_GamepadLstick(Text text)
	{
		if (Gamepad.all.Count <= 0)
		{
			text.text = "Could not access gamepad";
			return;
		}
		text.text = $"x: {_gamepadLeftStick.x}  y: {_gamepadLeftStick.y}";
	}

	/// <summary>
	/// �f�o�b�O
	/// </summary>
	public void PrintText_JoyconLaccel(Text text)
	{
		if (_joycons.Count <= 0)
		{
			text.text = "Could not access Joy-con";
			return;
		}
		text.text = $"x: {_joyconLeftAccel.x}\ny: {_joyconLeftAccel.y}\nz: {_joyconLeftAccel.z}";
	}



	// Unity Events

	private void Awake()
	{
		_input = new InputSystemAction();

		// �V���O���g���̍쐬
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	private void Start()
	{
		FindJoycon();
	}

	private void Update()
	{
		GetInput();
		Action_Button();
		Action_Vec2();
		Action_Vec3();
		Action();
	}

	private void OnEnable()
	{
		_input.Enable();

		// �E�ړ�
		_input.Player.RightMove.started += OnRightMove_started;
		_input.Player.RightMove.canceled += OnRightMove_canceled;

		// ���ړ�
		_input.Player.LeftMove.started += OnLeftMove_started;
		_input.Player.LeftMove.canceled += OnLeftMove_canceled;

		// �E�_�b�V��
		_input.Player.RightDash.started += OnRightDash_started;
		_input.Player.RightDash.canceled += OnRightDash_canceled;
	}


	private void OnDisable()
	{
		_input.Disable();

		// �E�ړ�
		_input.Player.RightMove.started -= OnRightMove_started;
		_input.Player.RightMove.canceled -= OnRightMove_canceled;

		// ���ړ�
		_input.Player.LeftMove.started -= OnLeftMove_started;
		_input.Player.LeftMove.canceled -= OnLeftMove_canceled;

		// �E�_�b�V��
		_input.Player.RightDash.started -= OnRightDash_started;
		_input.Player.RightDash.canceled -= OnRightDash_canceled;
	}

	public void OnBuruBuru()
	{
		_joyconL.SetRumble(320, 640, 1.0f, 300);
    }

	public void Restart()
	{
		FindJoycon();
	}

}