//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
// �쐬��:	2025/05/08
// �쐬��:	��Él��
// �d�l:		�v���C���[�̃A�N�V����
//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class shimoPlayer : MonoBehaviour
{   // variable�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    private List<Joycon> m_joycons;
	private Joycon m_joyconR;
	
    public enum MoveStates		// �ړ����: ��~�A���s�A���s
	{ Stop, Walk, Run }

	// �ړ��Ɋւ���ϐ�
	MoveStates				moveState;		// �ړ����(�O���Q�Ɖ�)
	public MoveStates	MoveState { get { return moveState; } }
	Vector3		velocity		= Vector3.zero;		// ���W�X�V��
	const float	walkSpeed	= 0.1f;						// ���s���x
	const float	runSpeed		= 0.2f;						// ���s���x

	// ���͂Ɋւ���ϐ�
	float accel = 0.0f;     // J-con�̉����x

    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        //if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }




    // method�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

    /// <summary>
    /// �ړ��ʂɉ����Ĉړ�����
    /// </summary>
    void Move()
	{
		// ���Â̔���
		switch(velocity.x)
		{
			case 0:					moveState = MoveStates.Stop;	break;
			case walkSpeed:	moveState = MoveStates.Walk;	break;
			case runSpeed:	moveState = MoveStates.Run;	break;
		}

		// ���݂̍��W�Ɉړ��ʂ����Z����
		transform.position += velocity;
	}

	/// <summary>
	/// ���͂ɉ����Ĉړ��ʂ𒲐߂���
	/// </summary>
	void Walk()
	{
		if			(accel > 0.1f && accel <= 0.5f)	velocity.x = walkSpeed;	// ���s
		else if	(accel > 0.1f && accel <= 0.5f)	velocity.x = runSpeed;		// ���s
		else															velocity.x = 0;					// ��~
	}



	// unityEvent�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

	void FixedUpdate()
	{
		Move();
	}

	// ���͔���p
	void Update()
	{
        foreach (var joycon in m_joycons)
        {
            var v_accel = joycon.GetAccel();
            accel = v_accel.x;
			Debug.Log(accel);
        }
        Walk();
	}
}
