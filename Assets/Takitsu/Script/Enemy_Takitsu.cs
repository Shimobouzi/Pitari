using UnityEngine;

public class Enemy_Takitsu : MonoBehaviour
{
	// 列挙型

	// 状態
	public enum State
	{
		Idle,	  // 静止中 (ぼっ立ち)
		Wating,	  // 待機中 (合掌)
		Watching, // 監視中 (手を開いてる)
		Detection // 検知   (発覚)
	}



	// 変数

	// ステータス	-------------------------------------------------------------------------------
	// 自身の状態
	private State _state = State.Watching;
	// 監視領域
	private readonly Vector2 _SIGHTAREA = new Vector2(5.0f, 1.0f);

	// ギズモ描画制御(デバッグ用)	-------------------------------------------------------------------
	// 監視領域
	private bool _isSight = false;



	// 関数

	/// <summary>
	/// 状態に応じた行動
	/// </summary>
	private void Action()
	{
		switch(_state)
		{
			// 静止中
			case State.Idle:
			{
				break;
			}

			// 待機中
			case State.Wating:
			{
				Action_Wating();
				break;
			}

			// 監視中
			case State.Watching:
			{
				Action_Watching();
				break;
			}

			// 検知
			case State.Detection:
			{
				break;
			}
		}
	}

	/// <summary>
	/// 待機中の行動
	/// </summary>
	private void Action_Wating()
	{

	}

	/// <summary>
	/// 監視中の行動
	/// </summary>
	private void Action_Watching()
	{
		// 監視領域	-------------------------------------------------------------------------------
		// ギズモ描画を有効化
		_isSight = true;

		// 正面に長方形型のBoxcast
		RaycastHit2D hit = Physics2D.BoxCast(transform.position, _SIGHTAREA, 0, Vector2.zero, 1, LayerSetting.Layer_Player());
		// 命中
		if (hit)
		{
			_state = State.Detection;
		}
	}

	/// <summary>
	/// ギズモ描画用の真偽値をリセットする
	/// </summary>
	private void GizomoReset()
	{
		_isSight = false;
	}



	// Unity Events

	private void FixedUpdate()
	{
		// 常に一番上に
		GizomoReset();

		Action();
	}

	// Editer上でcast系の範囲を描画する
	void OnDrawGizmos()
	{
		//　監視領域を可視化
		if (_isSight)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireCube(transform.position, _SIGHTAREA);
		}
	}
}
