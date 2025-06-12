// �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
// �쐬��:	��Él��
// �쐬��:	2025/05/28
// �d�l:		InputSystem�̈����Ɋ���邽�߂̃X�N���v�g 
// �[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

using UnityEngine;
using UnityEngine.UI;

public class InputSystemTest : MonoBehaviour
{
	// �f�o�b�O�p
	[SerializeField]
	private Text[] text;

	private void FixedUpdate()
	{
		Input_Player.Instance.PrintText_GamepadLstick(text[1]);
		Input_Player.Instance.PrintText_JoyconLaccel(text[2]);


		if (Input_Player.Instance.RightMove_performed)
		{
			text[0].text = "�E�ړ�";
			transform.position += Vector3.right / 10.0f;
		}
		else if (Input_Player.Instance.LeftMove_Performed)
		{
			text[0].text = "���ړ�";
			transform.position += Vector3.left / 10.0f;
		}
		else if(Input_Player.Instance.RightDash_Performed)
		{
			text[0].text = "�E�_�b�V��";
			transform.position += Vector3.right / 5.0f;
		}
		else
		{
			text[0].text = "���͂Ȃ�";
		}
	}
}