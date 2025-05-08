//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[
// �쐬��:	2025/05/08
// �쐬��:	��Él��
// �d�l:		�v���C���[�̃A�N�V����
//�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

using UnityEngine;

public class Player : MonoBehaviour
{   // variable�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[�[

	public enum MoveStates		// �ړ����: ��~�A���s�A���s
	{ Stop, Walk, Run }

	// �ړ��Ɋւ���ϐ�
	MoveStates				moveState;		// �ړ����(�O���Q�Ɖ�)
	public MoveStates	MoveState { get { return moveState; } }
	Vector3		velocity		= Vector3.zero;		// ���W�X�V��
	const float	walkSpeed	= 0.1f;						// ���s���x
	const float	runSpeed		= 0.2f;						// ���s���x

	// ���͂Ɋւ���ϐ�
	float accel = 0.0f;		// J-con�̉����x



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
		Walk();
	}
}
