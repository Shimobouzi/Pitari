using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HaritsukiMantesiten : MonoBehaviour
{
    [Header("アニメーション制御")]
    public Animator animator;

    [Header("プレイヤー検出設定")]
    public LayerMask playerLayer;

    [Header("視界判定コライダー（両手）")]
    public PolygonCollider2D rightVisionCollider;
    public PolygonCollider2D leftVisionCollider;

    private bool isHandOpen = false;
    private bool isEyeVisible = false;

    [Header("状態変化の時間（秒）")]
    public float minOpenDuration = 2f;
    public float maxOpenDuration = 4f;
    public float minCloseDuration = 3.5f;
    public float maxCloseDuration = 5.5f;

    private void Start()
    {
        // 初期化：視界判定を無効化
        if (rightVisionCollider != null) rightVisionCollider.enabled = false;
        if (leftVisionCollider != null) leftVisionCollider.enabled = false;

        Debug.Log("HaritsukiMan: 初期化完了。状態変化ルーチン開始。");
        StartCoroutine(RandomizeStateRoutine());
    }

    private IEnumerator RandomizeStateRoutine()
    {
        while (true)
        {
            // ランダムで手を開くか閉じるか決定
            isHandOpen = Random.value > 0.5f;
            isEyeVisible = isHandOpen;

            // アニメーションに反映
            animator.SetBool("HirakuBool", isHandOpen);

            // 視界判定のON/OFFを切り替え
            if (rightVisionCollider != null) rightVisionCollider.enabled = isHandOpen;
            if (leftVisionCollider != null) leftVisionCollider.enabled = isHandOpen;

            // 次の状態までの待機時間をランダムに設定
            float waitTime = isHandOpen
                ? Random.Range(minOpenDuration, maxOpenDuration)
                : Random.Range(minCloseDuration, maxCloseDuration);

            Debug.Log($"HaritsukiMan: 状態更新 → {(isHandOpen ? "手開" : "手閉")}, 判定:{isHandOpen}, 次の状態まで:{waitTime:F2}秒");

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 手が開いていて、目が見えているときだけ判定
        if (!isHandOpen || !isEyeVisible) return;

        if (other.CompareTag("Player"))
        {
            var playerMove = other.GetComponent<NewPlayerMove>();
            if (playerMove != null && !playerMove.GetisHiding())
            {
                Debug.Log("HaritsukiMan: 擬態していないプレイヤーを発見 → ゲームオーバー処理へ");
                Invoke(nameof(RestartScene), 0.1f);
            }
            else
            {
                Debug.Log("HaritsukiMan: プレイヤーは擬態中 → スルー");
            }
        }
    }

    private void RestartScene()
    {
        // Resultシーンに遷移
        SceneManager.LoadScene("Result");
    }
}
