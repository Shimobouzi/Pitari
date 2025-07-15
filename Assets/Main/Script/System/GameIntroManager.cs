using UnityEngine;
using System.Collections;

public class GameIntroSequence : MonoBehaviour
{
    [Header("プレイヤー関連")]
    [Tooltip("ゲーム本編用のプレイヤーPrefab")]
    public GameObject realPlayerPrefab;

    [Tooltip("プレイヤーの出現位置")]
    public Transform playerSpawnPoint;

    private GameObject spawnedPlayer;

    [Header("カメラ関連")]
    [Tooltip("通常のメインカメラ")]
    public Camera mainCamera;

    [Tooltip("カメラがプレイヤーを追いかけるオフセット")]
    public Vector3 cameraOffset = new Vector3(0, 5, -10);

    [Header("UI・演出関連")]
    [Tooltip("狸の説明UI")]
    public GameObject tanukiUI;

    [Tooltip("UIやゲーム開始処理を管理するスクリプト")]
    public TutorialUIManager tutorialUIManager;

    void Start()
    {
        // ゲーム開始時に演出シーケンスを開始
        StartCoroutine(IntroSequence());
    }

    /// <summary>
    /// ゲーム開始からの演出を秒数で順番に実行する
    /// </summary>
    IEnumerator IntroSequence()
    {
        // 必要に応じて前の演出をここに追加

        // 11秒後：プレイヤー出現とカメラ切り替え
        yield return new WaitForSeconds(11f);
        SpawnPlayerAndSwitchCamera();

        // 12秒後：狸UIを再表示
        yield return new WaitForSeconds(1f);
        tanukiUI.SetActive(true);

        // 15秒後：狸UIを非表示にしてゲーム開始
        yield return new WaitForSeconds(3f);
        tanukiUI.SetActive(false);
        tutorialUIManager.StartGame();
    }

    /// <summary>
    /// プレイヤーを生成し、カメラをそのプレイヤーに追従させる
    /// </summary>
    void SpawnPlayerAndSwitchCamera()
    {
        // プレイヤーを生成
        spawnedPlayer = Instantiate(realPlayerPrefab, playerSpawnPoint.position, Quaternion.identity);

        // カメラの追従を開始（Updateで追いかける）
        StartCoroutine(FollowPlayer());
    }

    /// <summary>
    /// カメラがプレイヤーを追いかける処理（毎フレーム更新）
    /// </summary>
    IEnumerator FollowPlayer()
    {
        while (spawnedPlayer != null)
        {
            Vector3 targetPosition = spawnedPlayer.transform.position + cameraOffset;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 5f);
            yield return null;
        }
    }

}
