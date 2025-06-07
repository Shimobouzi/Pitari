using UnityEngine;

//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
// 作成者:	瀧津瑛主
// 仕様:		音を管理する
//ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

public class TakitsuAudioManager : MonoBehaviour
{	// variableーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	// シングルトン
	public static TakitsuAudioManager Instance;

	//音量 
	public static float SEVolume;
	public static float BgmVolume;



	// unityEventーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
