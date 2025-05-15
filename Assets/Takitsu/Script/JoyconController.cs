//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
// �쐬��:	2025/05/08
// �쐬��:	��Él��
// �d�l:		�ڑ�����Ă���Joy-con�̊Ǘ�
//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

using System.Collections.Generic;
using UnityEngine;

public class JoyconController : MonoBehaviour
{   // variable�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

	// �R���g���[���[�֘A
	private List<Joycon> m_joycons;
	Joycon joyconL;
	Joycon joyconR;


	// method�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

	/// <summary>
	/// PC�ɐڑ�����Ă���Joy-con��T��
	/// </summary>
	void FindJoycon()
	{
		// Joy-con�}�l�[�W���[���擾
		m_joycons = JoyconManager.Instance.j;

		// �ڑ�����Ă���Joy-con�������ꍇ�A������ł��؂�
		if (m_joycons == null || m_joycons.Count <= 0)
		{
			Debug.Log("PC�ɐڑ�����Ă���Joy-con��������܂���ł���");
			return;
		}

		// Joy-con����ڑ�����
		joyconL = m_joycons.Find(c => c.isLeft);
		// Joy-con�E��ڑ�����
		joyconR = m_joycons.Find(c => c.isLeft);
	}

	// unityEvent�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

	void Start()
	{
		FindJoycon();
	}
}