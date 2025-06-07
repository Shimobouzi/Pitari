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
	public bool RightMove_Started
	{ get { return _rightMove_started; } }
	// ���ړ�
	private bool _leftMove_started;
	public bool LeftMove_Started
	{ get { return _leftMove_started; } }

	// ���͂��Ă����	-------------------------------------------------------------------------------
	// �E�ړ�
	private bool _rightMove_performed;
	public bool RightMove_performed
	{ get { return _rightMove_performed; } }
	// ���ړ�
	private bool _leftMove_performed;
	public bool LeftMove_Performed
	{ get { return _leftMove_performed; } }

	// ���͉��������u��	---------------------------------------------------------------------------
	// �E�ړ�
	private bool _rightMove_canceled;
	public bool rightMove_Canceled
	{ get { return _rightMove_canceled; } }
	// ���ړ�
	private bool _leftMove_canceled;
	public bool LeftMove_Canceled
	{ get { return _leftMove_canceled; } }

	// 2,3�����I����	-------------------------------------------------------------------------------
	// �R���g���[���[�̍��X�e�B�b�N
	private Vector2 _controllerLeftStick = Vector2.zero;

	// Joy-con�̍������x
	private Vector3 _joyconLeftAccel = Vector3.zero;

	// ����ϐ�	-----------------------------------------------------------------------------------
	// �E�ړ����̑���f�o�C�X
	bool _rightMove_vector2 = false;
	bool _rightMove_vector3 = false;
	bool _rightMove_button = false;
	// �E�ړ����̑���f�o�C�X
	bool _leftMove_vector2 = false;
	bool _leftMove_button = false;

	// �ړ�����̂ɕK�v�ȃX�e�B�b�N�̌X��
	private const float _MOVERIGHT_STICKVALUE = 0.0f;
	// �ړ�����̂ɕK�v�ȉ����x
	private const float _MOVERIGHT_ACCELVALUE = 1.0f;
	// �����x���߂ɂ��A����p������
	private const float _MOVERIGHT_TIME = 0.2f;

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
	}

	/// <summary>
	/// 2�����I����(�X�e�B�b�N)�ɂ�鑀����󂯕t����
	/// </summary>
	private void Action_Vector2()
	{
		// �E�ړ�	-------------------------------------------------------------------------------
		// ���l�܂ŃX�e�B�b�N���X���Ă���
		if (_controllerLeftStick.x > _MOVERIGHT_STICKVALUE)
		{
			_rightMove_vector2 = true;
		}
		else
		{
			_rightMove_vector2 = false;
		}

		// ���ړ�	-------------------------------------------------------------------------------
		// ���l�܂ŃX�e�B�b�N���X���Ă���
		if (_controllerLeftStick.x < -_MOVERIGHT_STICKVALUE)
		{
			_leftMove_vector2 = true;
		}
		else
		{
			_leftMove_vector2 = false;
		}
	}

	private void Action_Vector3()
	{
		// �E�ړ�	-------------------------------------------------------------------------------
		// �����ꂩ�̎��̉����x��-1.0�ȉ��A��������1.0�ȏ�̏ꍇ
		if (_joyconLeftAccel.x > _MOVERIGHT_ACCELVALUE || _joyconLeftAccel.x < -_MOVERIGHT_ACCELVALUE ||
			_joyconLeftAccel.y > _MOVERIGHT_ACCELVALUE || _joyconLeftAccel.y < -_MOVERIGHT_ACCELVALUE ||
			_joyconLeftAccel.z > _MOVERIGHT_ACCELVALUE || _joyconLeftAccel.z < -_MOVERIGHT_ACCELVALUE)
		{
			_rightMove_vector3 = true;
		}
		else
		{
			_rightMove_vector3 = false;
		}
	}

	/// <summary>
	/// �A�N�V������L���ɂ���
	/// </summary>
	private void Action()
	{
		// �E�ړ�
		_rightMove_performed = _rightMove_button || _rightMove_vector2 || _rightMove_vector3;
		// ���ړ�
		_leftMove_performed = _leftMove_button || _leftMove_vector2;
	}

	/// <summary>
	/// �O���f�o�C�X�̓��͏����擾����
	/// </summary>
	private void GetInput()
	{
		// �R���g���[��L�X�e�B�b�N
		if (Gamepad.all.Count > 0) _controllerLeftStick = Gamepad.current.leftStick.ReadValue();

		// Joy-conL�����x
		if (_joycons.Count > 0) _joyconLeftAccel = _joyconL.GetAccel();

		Vector3 ofset = Vector3.zero;
		if (_joyconLeftAccel.x > 1.0f) ofset.x = 1.0f;
		else if (_joyconLeftAccel.x < -1.0f) ofset.x = -1.0f;
		else ofset.x = _joyconLeftAccel.x;

		if (_joyconLeftAccel.y > 1.0f) ofset.y = 1.0f;
		else if (_joyconLeftAccel.y < -1.0f) ofset.y = -1.0f;
		else ofset.y = _joyconLeftAccel.y;

		if (_joyconLeftAccel.z > 1.0f) ofset.z = 1.0f;
		else if (_joyconLeftAccel.z < -1.0f) ofset.z = -1.0f;
		else ofset.z = _joyconLeftAccel.z;

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
	/// �f�o�b�O
	/// </summary>
	public void PrintText_ControllerLstick(Text text)
	{
		text.text = $"x: {_controllerLeftStick.x}  y: {_controllerLeftStick.y}";
	}

	/// <summary>
	/// �f�o�b�O
	/// </summary>
	public void PrintText_JoyconLaccel(Text text)
	{
		text.text = $"x: {_joyconLeftAccel.x}\ny: {_joyconLeftAccel.y}\nz: {_joyconLeftAccel.z}";
	}



	// Unity Events

	private void Awake()
	{
		// �C���X�^���X��
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
		Action_Button();
		Action_Vector2();
		Action_Vector3();
		Action();
		GetInput();
	}

	/// <summary>
	/// ���͂�L����
	/// </summary>
	private void OnEnable()
	{
		_input.Enable();

		// �E�ړ�
		_input.Player.RightMove.started += ctx => _rightMove_started = true;
		_input.Player.RightMove.canceled += ctx => _rightMove_canceled = true;

		// ���ړ�
		_input.Player.LeftMove.started += ctx => _leftMove_started = true;
		_input.Player.LeftMove.canceled += ctx => _leftMove_canceled = true;
	}

	/// <summary>
	/// ���͂𖳌���
	/// </summary>
	private void OnDisable()
	{
		_input.Disable();

		// �E�ړ�
		_input.Player.RightMove.started -= ctx => _rightMove_started = true;
		_input.Player.RightMove.canceled -= ctx => _rightMove_canceled = true;

		// ���ړ�
		_input.Player.LeftMove.started -= ctx => _leftMove_started = true;
		_input.Player.LeftMove.canceled -= ctx => _leftMove_canceled = true;
	}
}