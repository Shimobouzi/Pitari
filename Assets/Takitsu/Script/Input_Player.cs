using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Input_Player : MonoBehaviour
{
	// 変数

	// シングルトン	-------------------------------------------------------------------------------
	private InputSystemAction _input;
	public static Input_Player Instance;

	// 入力開始した瞬間	---------------------------------------------------------------------------
	// 右移動
	private bool _rightMove_started;
	public bool RightMove_Started
	{ get { return _rightMove_started; } }
	// 左移動
	private bool _leftMove_started;
	public bool LeftMove_Started
	{ get { return _leftMove_started; } }

	// 入力している間	-------------------------------------------------------------------------------
	// 右移動
	private bool _rightMove_performed;
	public bool RightMove_performed
	{ get { return _rightMove_performed; } }
	// 左移動
	private bool _leftMove_performed;
	public bool LeftMove_Performed
	{ get { return _leftMove_performed; } }

	// 入力解除した瞬間	---------------------------------------------------------------------------
	// 右移動
	private bool _rightMove_canceled;
	public bool rightMove_Canceled
	{ get { return _rightMove_canceled; } }
	// 左移動
	private bool _leftMove_canceled;
	public bool LeftMove_Canceled
	{ get { return _leftMove_canceled; } }

	// 2,3次元的入力	-------------------------------------------------------------------------------
	// コントローラーの左スティック
	private Vector2 _controllerLeftStick = Vector2.zero;

	// Joy-conの左加速度
	private Vector3 _joyconLeftAccel = Vector3.zero;

	// 制御変数	-----------------------------------------------------------------------------------
	// 右移動中の操作デバイス
	bool _rightMove_vector2 = false;
	bool _rightMove_vector3 = false;
	bool _rightMove_button = false;
	// 右移動中の操作デバイス
	bool _leftMove_vector2 = false;
	bool _leftMove_button = false;

	// 移動するのに必要なスティックの傾き
	private const float _MOVERIGHT_STICKVALUE = 0.0f;
	// 移動するのに必要な加速度
	private const float _MOVERIGHT_ACCELVALUE = 1.0f;
	// 加速度超過による、判定継続時間
	private const float _MOVERIGHT_TIME = 0.2f;

	// Joy-Con	-----------------------------------------------------------------------------------
	// Joy-Conリスト（複数接続されたJoy-Conの情報）
	private List<Joycon> _joycons;
	// Joy-Con左
	private Joycon _joyconL;



	// 関数

	/// <summary>
	/// ボタン入力による操作を受け付ける
	/// </summary>
	public void Action_Button()
	{
		// 右移動	-------------------------------------------------------------------------------
		// 入力開始
		if (_rightMove_started)
		{
			_rightMove_started = false;
			_rightMove_button = true;
		}
		// 入力解除
		else if (_rightMove_canceled)
		{
			_rightMove_canceled = false;
			_rightMove_button = false;
		}

		// 左移動	-------------------------------------------------------------------------------
		// 入力開始
		if (_leftMove_started)
		{
			_leftMove_started = false;
			_leftMove_button = true;
		}
		// 入力解除
		if (_leftMove_canceled)
		{
			_leftMove_canceled = false;
			_leftMove_button = false;
		}
	}

	/// <summary>
	/// 2次元的入力(スティック)による操作を受け付ける
	/// </summary>
	private void Action_Vector2()
	{
		// 右移動	-------------------------------------------------------------------------------
		// 一定値までスティックが傾いている
		if (_controllerLeftStick.x > _MOVERIGHT_STICKVALUE)
		{
			_rightMove_vector2 = true;
		}
		else
		{
			_rightMove_vector2 = false;
		}

		// 左移動	-------------------------------------------------------------------------------
		// 一定値までスティックが傾いている
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
		// 右移動	-------------------------------------------------------------------------------
		// いずれかの軸の加速度が-1.0以下、もしくは1.0以上の場合
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
	/// アクションを有効にする
	/// </summary>
	private void Action()
	{
		// 右移動
		_rightMove_performed = _rightMove_button || _rightMove_vector2 || _rightMove_vector3;
		// 左移動
		_leftMove_performed = _leftMove_button || _leftMove_vector2;
	}

	/// <summary>
	/// 外部デバイスの入力情報を取得する
	/// </summary>
	private void GetInput()
	{
		// コントローラLスティック
		if (Gamepad.all.Count > 0) _controllerLeftStick = Gamepad.current.leftStick.ReadValue();

		// Joy-conL加速度
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
	/// 接続されたJoyConを取得する
	/// </summary>
	private void FindJoycon()
	{
		// Joy-Conのリストを取得
		_joycons = JoyconManager.Instance.j;

		// Joyconを接続する
		if (_joycons.Count > 0)
		{
			_joyconL = _joycons[0];
			Debug.Log("Joy-Conが接続されました。");

		}
		else
		{
			Debug.LogWarning("Joy-Conが接続されていません。");
		}

		// Debug: ゲームパッドの接続を確認する
		if (Gamepad.all.Count > 0)
		{
			Debug.Log("ゲームパッドが接続されました。");
		}
		else
		{
			Debug.LogWarning("ゲームパッドが接続されていません。");
		}
	}

	/// <summary>
	/// デバッグ
	/// </summary>
	public void PrintText_ControllerLstick(Text text)
	{
		text.text = $"x: {_controllerLeftStick.x}  y: {_controllerLeftStick.y}";
	}

	/// <summary>
	/// デバッグ
	/// </summary>
	public void PrintText_JoyconLaccel(Text text)
	{
		text.text = $"x: {_joyconLeftAccel.x}\ny: {_joyconLeftAccel.y}\nz: {_joyconLeftAccel.z}";
	}



	// Unity Events

	private void Awake()
	{
		// インスタンス化
		_input = new InputSystemAction();

		// シングルトンの作成
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
	/// 入力を有効化
	/// </summary>
	private void OnEnable()
	{
		_input.Enable();

		// 右移動
		_input.Player.RightMove.started += ctx => _rightMove_started = true;
		_input.Player.RightMove.canceled += ctx => _rightMove_canceled = true;

		// 左移動
		_input.Player.LeftMove.started += ctx => _leftMove_started = true;
		_input.Player.LeftMove.canceled += ctx => _leftMove_canceled = true;
	}

	/// <summary>
	/// 入力を無効化
	/// </summary>
	private void OnDisable()
	{
		_input.Disable();

		// 右移動
		_input.Player.RightMove.started -= ctx => _rightMove_started = true;
		_input.Player.RightMove.canceled -= ctx => _rightMove_canceled = true;

		// 左移動
		_input.Player.LeftMove.started -= ctx => _leftMove_started = true;
		_input.Player.LeftMove.canceled -= ctx => _leftMove_canceled = true;
	}
}