using UnityEngine;

public class Enemy_Takitsu : MonoBehaviour
{
	// �񋓌^

	// ���
	public enum State
	{
		Idle,	  // �Î~�� (�ڂ�����)
		Wating,	  // �ҋ@�� (����)
		Watching, // �Ď��� (����J���Ă�)
		Detection // ���m   (���o)
	}



	// �ϐ�

	// �X�e�[�^�X	-------------------------------------------------------------------------------
	// ���g�̏��
	private State _state = State.Watching;
	// �Ď��̈�
	private readonly Vector2 _SIGHTAREA = new Vector2(5.0f, 1.0f);

	// �M�Y���`�搧��(�f�o�b�O�p)	-------------------------------------------------------------------
	// �Ď��̈�
	private bool _isSight = false;



	// �֐�

	/// <summary>
	/// ��Ԃɉ������s��
	/// </summary>
	private void Action()
	{
		switch(_state)
		{
			// �Î~��
			case State.Idle:
			{
				break;
			}

			// �ҋ@��
			case State.Wating:
			{
				Action_Wating();
				break;
			}

			// �Ď���
			case State.Watching:
			{
				Action_Watching();
				break;
			}

			// ���m
			case State.Detection:
			{
				break;
			}
		}
	}

	/// <summary>
	/// �ҋ@���̍s��
	/// </summary>
	private void Action_Wating()
	{

	}

	/// <summary>
	/// �Ď����̍s��
	/// </summary>
	private void Action_Watching()
	{
		// �Ď��̈�	-------------------------------------------------------------------------------
		// �M�Y���`���L����
		_isSight = true;

		// ���ʂɒ����`�^��Boxcast
		RaycastHit2D hit = Physics2D.BoxCast(transform.position, _SIGHTAREA, 0, Vector2.zero, 1, LayerSetting.Layer_Player());
		// ����
		if (hit)
		{
			_state = State.Detection;
		}
	}

	/// <summary>
	/// �M�Y���`��p�̐^�U�l�����Z�b�g����
	/// </summary>
	private void GizomoReset()
	{
		_isSight = false;
	}



	// Unity Events

	private void FixedUpdate()
	{
		// ��Ɉ�ԏ��
		GizomoReset();

		Action();
	}

	// Editer���cast�n�͈̔͂�`�悷��
	void OnDrawGizmos()
	{
		//�@�Ď��̈������
		if (_isSight)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireCube(transform.position, _SIGHTAREA);
		}
	}
}
