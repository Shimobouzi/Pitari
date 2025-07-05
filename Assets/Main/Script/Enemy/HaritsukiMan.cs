using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 張り付き男：手が開いていて目が見えているとき、擬態していないプレイヤーを検出してゲームオーバーにする敵キャラクター
/// </summary>
public class HaritsukiMan : MonoBehaviour
{
    [Header("アニメーション制御")]
    public Animator animator; // Animatorコンポーネント（手の開閉・目の表示を制御）

    [Header("プレイヤー検出設定")]
    public LayerMask playerLayer; // プレイヤーを検出するためのレイヤー

    [Header("当たり判定の基本サイズと位置")]
    public Vector2 baseDetectionSize = new Vector2(2f, 2f); // 基本の判定サイズ
    public Vector2 baseDetectionOffset = new Vector2(1f, 0f); // 基本の判定位置（敵の前方）

    private Vector2 currentDetectionSize;   // 実際に使用される判定サイズ（ランダム変化）
    private Vector2 currentDetectionOffset; // 実際に使用される判定位置（ランダム変化）

    private bool isHandOpen = false;    // 手が開いているかどうか
    private bool isEyeVisible = false;  // 目が見えているかどうか（手が開いているときのみ表示）

    [Header("状態変化の間隔（秒）")]
    public float minChangeInterval = 2f; // 状態変化の最短間隔
    public float maxChangeInterval = 5f; // 状態変化の最長間隔

    private void Start()
    {
        // ランダムに状態を変化させるコルーチンを開始
        StartCoroutine(RandomizeStateRoutine());
    }

    private void Update()
    {
        // 手が開いていて目が見えているときのみ、プレイヤーの検出を行う
        if (isHandOpen && isEyeVisible)
        {
            // 判定エリアの中心位置を計算
            Vector2 center = (Vector2)transform.position + currentDetectionOffset;

            // 判定エリア内にプレイヤーがいるか確認
            Collider2D hit = Physics2D.OverlapBox(center, currentDetectionSize, 0f, playerLayer);

            if (hit != null && hit.CompareTag("Player"))
            {
                // プレイヤーの擬態状態を取得
                var playerMove = hit.GetComponent<NewPlayerMove>();
                if (playerMove != null && !playerMove.GetisHiding())
                {
                    // 擬態していない → ゲームオーバー
                    Debug.Log("張り付き男：擬態していないプレイヤーを発見 → ゲームオーバー");
                    Invoke(nameof(RestartScene), 0.1f); // 少し遅延させて安全にシーン再読み込み
                }
                else
                {
                    // 擬態中 → スルー
                    Debug.Log("張り付き男：擬態中のプレイヤーをスルー");
                }
            }
        }
    }

    /// <summary>
    /// 一定時間ごとに手の開閉・目の表示・当たり判定をランダムに変更するコルーチン
    /// </summary>
    private IEnumerator RandomizeStateRoutine()
    {
        while (true)
        {
            // 次の状態変更までの待機時間をランダムに決定
            float waitTime = Random.Range(minChangeInterval, maxChangeInterval);
            yield return new WaitForSeconds(waitTime);

            // ランダムに手を開くか閉じるか決定（50%の確率）
            isHandOpen = Random.value > 0.5f;
            isEyeVisible = isHandOpen; // 手が開いているときだけ目が見える

            // Animatorに状態を反映（アニメーションと連動）
            animator.SetBool("IsHandOpen", isHandOpen);
            animator.SetBool("IsEyeVisible", isEyeVisible);

            // 当たり判定のサイズと位置をランダムに調整（±30%程度）
            float sizeFactor = Random.Range(0.7f, 1.3f);
            float offsetFactor = Random.Range(0.8f, 1.2f);

            currentDetectionSize = baseDetectionSize * sizeFactor;
            currentDetectionOffset = baseDetectionOffset * offsetFactor;

            // デバッグログで状態を確認
            Debug.Log($"張り付き男：状態更新 → 手開:{isHandOpen}, 目:{isEyeVisible}, 判定サイズ:{currentDetectionSize}");
        }
    }

    /// <summary>
    /// 現在のシーンを再読み込みしてゲームオーバー処理を実行
    /// </summary>
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Unityエディタ上で当たり判定エリアを可視化するためのGizmos描画
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector2 center = (Vector2)transform.position + currentDetectionOffset;
        Gizmos.DrawWireCube(center, currentDetectionSize);
    }
}
