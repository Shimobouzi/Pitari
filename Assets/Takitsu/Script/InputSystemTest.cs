// ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
// 作成者:	瀧津瑛主
// 作成日:	2025/05/28
// 仕様:		InputSystemの扱いに慣れるためのスクリプト 
// ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

using UnityEngine;
using UnityEngine.UI;

public class InputSystemTest : MonoBehaviour
{
	// デバッグ用
	[SerializeField]
	private Text[] text;

	private void FixedUpdate()
	{
		Input_Player.Instance.PrintText_GamepadLstick(text[1]);
		Input_Player.Instance.PrintText_JoyconLaccel(text[2]);


		if (Input_Player.Instance.RightMove_performed)
		{
			text[0].text = "右移動";
			transform.position += Vector3.right / 10.0f;
		}
		else if (Input_Player.Instance.LeftMove_Performed)
		{
			text[0].text = "左移動";
			transform.position += Vector3.left / 10.0f;
		}
		else if(Input_Player.Instance.RightDash_Performed)
		{
			text[0].text = "右ダッシュ";
			transform.position += Vector3.right / 5.0f;
		}
		else
		{
			text[0].text = "入力なし";
		}
	}
}