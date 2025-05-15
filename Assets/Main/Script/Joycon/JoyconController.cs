//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
// 作成日:	2025/05/08
// 作成者:	瀧津瑛主
// 仕様:		接続されているJoy-conの管理
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

/// <summary>
/// Joy-Conを振ると前に進むプレイヤー用スクリプト
/// JoyconLibが必要（https://github.com/Looking-Glass/JoyconLib）
/// </summary>

using System.Collections.Generic;
using UnityEngine;

public class JoyconController : MonoBehaviour
{   // variableーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    /* コントローラー関連*/

    // Joy-Conリスト（複数接続されたJoy-Conの情報）
    private List<Joycon> joycons;

    // 操作対象のJoy-Con（今回は右を使用）
    private Joycon joycon;

    //JoyconControllerインスタンス
    public static JoyconController Instance;




    // methodーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    /// <summary>
    /// PCに接続されているJoy-conを探す
    /// </summary>
    void FindJoycon()
	{
        // Joy-Conのリストを取得
        joycons = JoyconManager.Instance.j;

        // 1つ以上接続されていれば1番目を使う
        if (joycons.Count > 0)
        {
            joycon = joycons[0];
        }
        else
        {
            Debug.LogWarning("Joy-Conが接続されていません。");
        }
    }

	public Vector3 GetAccel()
	{
		return joycon.GetAccel();
	}

    public void OnBuruBuru()
    {
        joycon.SetRumble(320, 640, 1.0f, 500);
    }

    // unityEventーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

    private void Awake()
    {
        Instance = this;
    }

    void Start()
	{
		FindJoycon();
	}
}