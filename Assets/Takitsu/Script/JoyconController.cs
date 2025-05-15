//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
// 作成日:	2025/05/08
// 作成者:	瀧津瑛主
// 仕様:		接続されているJoy-conの管理
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

using System.Collections.Generic;
using UnityEngine;

public class JoyconController : MonoBehaviour
{   // variableーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	// コントローラー関連
	private List<Joycon> m_joycons;
	Joycon joyconL;
	Joycon joyconR;


	// methodーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	/// <summary>
	/// PCに接続されているJoy-conを探す
	/// </summary>
	void FindJoycon()
	{
		// Joy-conマネージャーを取得
		m_joycons = JoyconManager.Instance.j;

		// 接続されているJoy-conが無い場合、処理を打ち切る
		if (m_joycons == null || m_joycons.Count <= 0)
		{
			Debug.Log("PCに接続されているJoy-conが見つかりませんでした");
			return;
		}

		// Joy-con左を接続する
		joyconL = m_joycons.Find(c => c.isLeft);
		// Joy-con右を接続する
		joyconR = m_joycons.Find(c => c.isLeft);
	}

	// unityEventーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	void Start()
	{
		FindJoycon();
	}
}