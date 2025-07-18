using UnityEngine;

public class SkipManager : MonoBehaviour
{
    [Header("自動で動くオブジェクト")]
    public GameObject[] autoObjects;

    [Header("プレイヤー操作用オブジェクト")]
    public GameObject playerController;

    [Header("カメラ設定")]
    public Camera eventCamera;
    public Camera playerCamera;

    [Header("イベント用の音")]
    public AudioSource[] eventAudioSources;

    [Header("スキップ可能かどうか")]
    public bool canSkip = true;

    void Start()
    {
        // 初期状態：イベントカメラON、プレイヤーカメラOFF、プレイヤー操作OFF
        eventCamera.enabled = true;
        playerCamera.enabled = false;
        playerController.SetActive(false);
    }

    void Update()
    {
        if (canSkip && Input.GetKeyDown(KeyCode.Return))
        {
            SkipToGameplay();
        }
    }

    void SkipToGameplay()
    {
        // 自動オブジェクトの動作停止＆非表示
        foreach (GameObject obj in autoObjects)
        {
            var mover = obj.GetComponent<AutoMover>();
            if (mover != null)
            {
                mover.StopMovement();
            }
            obj.SetActive(false); // 完全非表示
        }

        // イベント用の音を停止
        foreach (AudioSource audio in eventAudioSources)
        {
            if (audio.isPlaying)
            {
                audio.Stop();
            }
        }

        // カメラ切り替え
        eventCamera.enabled = false;
        playerCamera.enabled = true;

        // プレイヤー操作有効化
        playerController.SetActive(true);

        Debug.Log("スキップ完了：オブジェクト非表示・音停止・カメラ切替・操作可能");
    }
}
