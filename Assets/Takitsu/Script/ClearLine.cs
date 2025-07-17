using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class ClearLine : MonoBehaviour
{
	// �t�F�[�h�A�E�g�̃A�j���[�^�[
	[SerializeField]
	private Animator _fadeAnim;

	// �t�F�[�h���邩���Ȃ���
	private bool _onFade = false;

	// �f�o�b�O�p
	// private float _pointOfset;


	private void FixedUpdate()
	{
		// �f�o�b�O�p
		//_pointOfset += 0.1f;

		// ���C�L���X�g�̓��e
		Vector3 origin = transform.position + Vector3.left; //* _pointOfset;
		Vector2 direction = Vector2.up;
		float distance = 15.0f;

		// ���C�L���X�g
		RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance);
		Debug.DrawRay(origin, direction * distance, Color.magenta);

		// ����
		if (hit.transform.tag == "Player" && !_onFade)
		{
			_fadeAnim.SetTrigger("FadeOut");
		}
	}
}
