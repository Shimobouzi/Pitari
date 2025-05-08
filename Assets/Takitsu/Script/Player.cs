//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
// 作成日:	2025/05/08
// 作成者:	瀧津瑛主
// 仕様:		プレイヤーのアクション
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

using UnityEngine;

public class Player : MonoBehaviour
{   // variableーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	public enum MoveStates		// 移動状態: 停止、歩行、走行
	{ Stop, Walk, Run }

	// 移動に関する変数
	MoveStates				moveState;		// 移動状態(外部参照可)
	public MoveStates	MoveState { get { return moveState; } }
	Vector3		velocity		= Vector3.zero;		// 座標更新量
	const float	walkSpeed	= 0.1f;						// 歩行速度
	const float	runSpeed		= 0.2f;						// 走行速度

	// 入力に関する変数
	float accel = 0.0f;		// J-conの加速度



	// methodーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	/// <summary>
	/// 移動量に応じて移動する
	/// </summary>
	void Move()
	{
		// 動静の判定
		switch(velocity.x)
		{
			case 0:					moveState = MoveStates.Stop;	break;
			case walkSpeed:	moveState = MoveStates.Walk;	break;
			case runSpeed:	moveState = MoveStates.Run;	break;
		}

		// 現在の座標に移動量を加算する
		transform.position += velocity;
	}

	/// <summary>
	/// 入力に応じて移動量を調節する
	/// </summary>
	void Walk()
	{
		if			(accel > 0.1f && accel <= 0.5f)	velocity.x = walkSpeed;	// 歩行
		else if	(accel > 0.1f && accel <= 0.5f)	velocity.x = runSpeed;		// 走行
		else															velocity.x = 0;					// 停止
	}



	// unityEventーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	void FixedUpdate()
	{
		Move();
	}

	// 入力判定用
	void Update()
	{
		Walk();
	}
}
