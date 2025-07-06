// ------------------------------------------------------------------------------------------------
// 製作者: 瀧津瑛主
// 製作日: 2025/06/07
// 仕様　: 入力を各デバイスから受け取り、適切な判定を行う
// ------------------------------------------------------------------------------------------------

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
	public bool RightMove_Started => _rightMove_started;
	// 左移動
	private bool _leftMove_started;
	public bool LeftMove_Started => _leftMove_started;
	// 右ダッシュ
	private bool _rightDash_started;
	public bool RightDash_Started => _rightDash_started;

	// 入力している間	-------------------------------------------------------------------------------
	// 右移動
	private bool _rightMove_performed;
	public bool RightMove_performed => _rightMove_performed;
	// 左移動
	private bool _leftMove_performed;
	public bool LeftMove_Performed => _leftMove_performed;
	// 右ダッシュ
	private bool _rightDash_performed;
	public bool RightDash_Performed => _rightDash_performed;

	// 入力解除した瞬間	---------------------------------------------------------------------------
	// 右移動
	private bool _rightMove_canceled;
	public bool rightMove_Canceled => _rightMove_canceled;
	// 左移動
	private bool _leftMove_canceled;
	public bool LeftMove_Canceled => _leftMove_canceled;
	// 右ダッシュ
	private bool _rightDash_canceled;
	public bool RightDash_Canceled => _rightDash_canceled;

	// イベントハンドラ	---------------------------------------------------------------------------
	// 右移動
	private void OnRightMove_started(InputAction.CallbackContext ctx) => _rightMove_started = true;
	private void OnRightMove_canceled(InputAction.CallbackContext ctx) => _rightMove_canceled = true;
	// 左移動
	private void OnLeftMove_started(InputAction.CallbackContext ctx) => _leftMove_started = true;
	private void OnLeftMove_canceled(InputAction.CallbackContext ctx) => _leftMove_canceled = true;
	// 右ダッシュ
	private void OnRightDash_started(InputAction.CallbackContext ctx) => _rightDash_started = true;
	private void OnRightDash_canceled(InputAction.CallbackContext ctx) => _rightDash_canceled = true;

	// 1次元的入力	-------------------------------------------------------------------------------
	// コントローラーの×ボタン
	private bool _gamepadSouthButton = false;

	// 2,3次元的入力	-------------------------------------------------------------------------------
	// コントローラーの左スティック
	private Vector2 _gamepadLeftStick = Vector2.zero;
	// Joy-conの左加速度
	private Vector3 _joyconLeftAccel = Vector3.zero;

	// 制御変数	-----------------------------------------------------------------------------------
	// 右移動中の操作デバイス
	bool _rightMove_vec2 = false;
	bool _rightMove_vec3 = false;
	bool _rightMove_button = false;
	// 右移動中の操作デバイス
	bool _leftMove_vec2 = false;
	bool _leftMove_button = false;
	// 右移動中の操作デバイス
	bool _rightDash_vec2 = false;
	bool _rightDash_vec3 = false;
	bool _rightDash_button = false;
	// 判定継続の残り時間
	private float _accelTime = 0.0f;

	// 移動するのに必要なスティックの傾き
	private const float _RIGHTMOVE_STICKVALUE = 0.0f;
	// 移動するのに必要な加速度
	private const float _RIGHTMOVE_ACCELVALUE = 0.33f;
	// ダッシュするのに必要な加速度
	private const float _RIGHTDASH_ACCELVALUE = 2.5f;
	// 判定継続時間
	private const float _RIGHTMOVE_ACCELTIME = 0.08f;

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

		// 右ダッシュ	---------------------------------------------------------------------------
		// 入力開始
		if (_rightDash_started)
		{
			_rightDash_started = false;
			_rightDash_button = true;
		}
		// 入力解除
		else if (_rightDash_canceled)
		{
			_rightDash_canceled = false;
			_rightDash_button = false;
		}
	}

	/// <summary>
	/// 2次元的入力(スティック等)による操作を受け付ける
	/// </summary>
	private void Action_Vec2()
	{
		// 右移動	-------------------------------------------------------------------------------
		// 一定値までスティックが傾いている
		if (_gamepadLeftStick.x > _RIGHTMOVE_STICKVALUE && !_gamepadSouthButton)
		{
			_rightMove_vec2 = true;
		}
		else
		{
			_rightMove_vec2 = false;
		}

		// 左移動	-------------------------------------------------------------------------------
		// 一定値までスティックが傾いている
		if (_gamepadLeftStick.x < -_RIGHTMOVE_STICKVALUE)
		{
			_leftMove_vec2 = true;
		}
		else
		{
			_leftMove_vec2 = false;
		}

		// 右ダッシュ	---------------------------------------------------------------------------
		// 一定値までスティックが傾いている
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
	/// 2次元的入力(Joycon加速度等)による操作を受け付ける
	/// </summary>
	private void Action_Vec3()
	{
		// Joy-conLの加速度を絶対値に
		Vector3 joyconAccel = AbsoluteVec3(_joyconLeftAccel);

		// 右移動	-------------------------------------------------------------------------------
		// いずれかの軸の加速度が一定以上かつ、右ダッシュ中でない場合
		if ((((joyconAccel.x > _RIGHTMOVE_ACCELVALUE && joyconAccel.x < _RIGHTDASH_ACCELVALUE) ||
			  (joyconAccel.y > _RIGHTMOVE_ACCELVALUE && joyconAccel.y < _RIGHTDASH_ACCELVALUE) ||
			  (joyconAccel.z > _RIGHTMOVE_ACCELVALUE && joyconAccel.z < _RIGHTDASH_ACCELVALUE)) && !_rightDash_vec3))
		{
			_rightMove_vec3 = true;
			_rightDash_vec3 = false;
			_accelTime = _RIGHTMOVE_ACCELTIME;
		}

		// 右ダッシュ	---------------------------------------------------------------------------
		// いずれかの軸の加速度が一定以上の場合
		else if (joyconAccel.x >= _RIGHTDASH_ACCELVALUE ||
				 joyconAccel.y >= _RIGHTDASH_ACCELVALUE ||
				 joyconAccel.z >= _RIGHTDASH_ACCELVALUE)
		{
			_rightDash_vec3 = true;
			_rightMove_vec3 = false;
			_accelTime = _RIGHTMOVE_ACCELTIME;
		}

		// 加速度のムラによる影響を軽減する
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
	/// アクションを有効にする
	/// </summary>
	private void Action()
	{
		// 右移動
		_rightMove_performed = _rightMove_button || _rightMove_vec2 || _rightMove_vec3;
		// 左移動
		_leftMove_performed = _leftMove_button || _leftMove_vec2;
		// 右ダッシュ
		_rightDash_performed = _rightDash_button || _rightDash_vec2 || _rightDash_vec3;
	}

	/// <summary>
	/// 外部デバイスの入力情報を取得する
	/// </summary>
	private void GetInput()
	{
		// ゲームパッドLスティック
		if (Gamepad.all.Count > 0) _gamepadLeftStick = Gamepad.current.leftStick.ReadValue();
		// ゲームパッド×ボタン
		if (Gamepad.all.Count > 0) _gamepadSouthButton = Gamepad.current.buttonSouth.isPressed;

		// Joy-conL加速度
		if (_joycons.Count > 0) _joyconLeftAccel = _joyconL.GetAccel();

		// Joy-conの加速度を正常に戻すためのオフセット
		Vector3 ofset = Vector3.zero;
		// x軸
		if (_joyconLeftAccel.x > 1.0f) ofset.x = 1.0f;
		else if (_joyconLeftAccel.x < -1.0f) ofset.x = -1.0f;
		else ofset.x = _joyconLeftAccel.x;
		// y軸
		if (_joyconLeftAccel.y > 1.0f) ofset.y = 1.0f;
		else if (_joyconLeftAccel.y < -1.0f) ofset.y = -1.0f;
		else ofset.y = _joyconLeftAccel.y;
		// z軸
		if (_joyconLeftAccel.z > 1.0f) ofset.z = 1.0f;
		else if (_joyconLeftAccel.z < -1.0f) ofset.z = -1.0f;
		else ofset.z = _joyconLeftAccel.z;
		// 統合
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
	/// 指定したVector3型の変数の絶対値を出力する
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
	/// デバッグ
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
	/// デバッグ
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
		GetInput();
		Action_Button();
		Action_Vec2();
		Action_Vec3();
		Action();
	}

	private void OnEnable()
	{
		_input.Enable();

		// 右移動
		_input.Player.RightMove.started += OnRightMove_started;
		_input.Player.RightMove.canceled += OnRightMove_canceled;

		// 左移動
		_input.Player.LeftMove.started += OnLeftMove_started;
		_input.Player.LeftMove.canceled += OnLeftMove_canceled;

		// 右ダッシュ
		_input.Player.RightDash.started += OnRightDash_started;
		_input.Player.RightDash.canceled += OnRightDash_canceled;
	}


	private void OnDisable()
	{
		_input.Disable();

		// 右移動
		_input.Player.RightMove.started -= OnRightMove_started;
		_input.Player.RightMove.canceled -= OnRightMove_canceled;

		// 左移動
		_input.Player.LeftMove.started -= OnLeftMove_started;
		_input.Player.LeftMove.canceled -= OnLeftMove_canceled;

		// 右ダッシュ
		_input.Player.RightDash.started -= OnRightDash_started;
		_input.Player.RightDash.canceled -= OnRightDash_canceled;
	}

	public void OnBuruburu()
	{
		_joyconL.SetRumble(320, 640, 1.0f, 500);
    }
}