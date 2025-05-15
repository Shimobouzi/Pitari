//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
// �쐬��:	2025/05/08
// �쐬��:	��Él��
// �d�l:		�ڑ�����Ă���Joy-con�̊Ǘ�
//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

/// <summary>
/// Joy-Con��U��ƑO�ɐi�ރv���C���[�p�X�N���v�g
/// JoyconLib���K�v�ihttps://github.com/Looking-Glass/JoyconLib�j
/// </summary>

using System.Collections.Generic;
using UnityEngine;

public class JoyconController : MonoBehaviour
{   // variable�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    /* �R���g���[���[�֘A*/

    // Joy-Con���X�g�i�����ڑ����ꂽJoy-Con�̏��j
    private List<Joycon> joycons;

    // ����Ώۂ�Joy-Con�i����͉E���g�p�j
    private Joycon joycon;

    //JoyconController�C���X�^���X
    public static JoyconController Instance;




    // method�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    /// <summary>
    /// PC�ɐڑ�����Ă���Joy-con��T��
    /// </summary>
    void FindJoycon()
	{
        // Joy-Con�̃��X�g���擾
        joycons = JoyconManager.Instance.j;

        // 1�ȏ�ڑ�����Ă����1�Ԗڂ��g��
        if (joycons.Count > 0)
        {
            joycon = joycons[0];
        }
        else
        {
            Debug.LogWarning("Joy-Con���ڑ�����Ă��܂���B");
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

    // unityEvent�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    private void Awake()
    {
        Instance = this;
    }

    void Start()
	{
		FindJoycon();
	}
}