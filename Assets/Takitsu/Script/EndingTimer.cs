using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingTimer : MonoBehaviour
{
	// 時間
	private float _time = 0.0f;

	// エンディング終了時間
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
