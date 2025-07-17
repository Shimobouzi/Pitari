using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTimer : MonoBehaviour
{
	// ����
	private float _time = 0.0f;

	// �G���f�B���O�I������
	[SerializeField]
	private float _endtime = 30.0f;



	private void FixedUpdate()
	{
		_time += Time.deltaTime;

		if(_time >= _endtime)
		{
			SceneManager.LoadScene("Title");
		}
	}
}
