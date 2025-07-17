using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class ClearLine : MonoBehaviour
{
	// フェードアウトのアニメーター
	[SerializeField]
	private Animator _fadeAnim;

	// フェードするかしないか
	private bool _onFade = false;

	// デバッグ用
	// private float _pointOfset;


	private void FixedUpdate()
	{
		// デバッグ用
		//_pointOfset += 0.1f;

		// レイキャストの内容
		Vector3 origin = transform.position + Vector3.left * _pointOfset;
		Vector2 direction = Vector2.up;
		float distance = 15.0f;

		// レイキャスト
		RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance);
		Debug.DrawRay(origin, direction * distance, Color.magenta);

		// 命中
		if (hit.transform.tag == "Player" && !_onFade)
		{
			_fadeAnim.SetTrigger("FadeOut");
		}
	}
}
