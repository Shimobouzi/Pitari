using UnityEngine;

public class FastOni : MonoBehaviour
{
    public enum OniState { Approaching, Waiting, Chasing }

    [Header("移動速度（接近時）")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("追跡速度（擬態解除後）")]
    [SerializeField] private float chaseSpeed = 4f;

    [Header("右→左への停止距離")]
    [SerializeField] private float approachDistance = 5f;

    [Header("接近時に反応する距離（擬態確認）")]
    [SerializeField] private float detectionRange = 2f;

    [Header("追跡中の捕獲判定距離")]
    [SerializeField] private float catchDistance = 1f;

    private Transform player;
    private Vector3 stopPosition;
    private OniState currentState = OniState.Approaching;

    private bool alreadyCheckedDuringApproach = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player タグが見つかりません！");
            return;
        }

        // 右→左方向に停止位置を決定
        stopPosition = transform.position + Vector3.left * approachDistance;
    }

    private void Update()
    {
        if (player == null) return;

        switch (currentState)
        {
            case OniState.Approaching:
                MoveLeft();
                CheckPlayerDuringApproach();
                break;

            case OniState.Waiting:
                CheckPlayerReveal();
                break;

            case OniState.Chasing:
                ChasePlayer();
                break;
        }
    }

    /// <summary>
    /// 右から左へ移動（接近）
    /// </summary>
    private void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= stopPosition.x)
        {
            currentState = OniState.Waiting;
            Debug.Log("停止地点到達 → 待機状態へ");
        }
    }

    /// <summary>
    /// 接近時の擬態チェック（最初の1回のみ判定）
    /// </summary>
    private void CheckPlayerDuringApproach()
    {
        if (alreadyCheckedDuringApproach) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= detectionRange)
        {
            alreadyCheckedDuringApproach = true;

            if (!PlayerMove.GetisHiding())
            {
                Debug.Log("【GAME OVER】擬態していない状態で見つかった！");
                // GameOver処理（例: シーン切り替え・アニメ再生など）
            }
            else
            {
                Debug.Log("擬態成功 → 鬼はスルーして停止位置へ向かう");
            }
        }
    }

    /// <summary>
    /// 停止中にプレイヤーが擬態解除したら追跡へ
    /// </summary>
    private void CheckPlayerReveal()
    {
        if (!PlayerMove.GetisHiding())
        {
            currentState = OniState.Chasing;
            Debug.Log("擬態解除検知 → 鬼ごっこ開始！");
        }
    }

    /// <summary>
    /// プレイヤーを追いかける（左→右）
    /// </summary>
    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * chaseSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= catchDistance)
        {
            Debug.Log("【GAME OVER】鬼ごっこ中に捕まった！");
            // GameOver処理
        }
    }

    /// <summary>
    /// デバッグ用ギズモ描画
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + Vector3.left * approachDistance, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchDistance);
    }
}
