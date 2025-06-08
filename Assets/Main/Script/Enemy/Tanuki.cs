using UnityEngine;

public class Tanuki : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2f;
    private bool isStopped = false;

    [Header("プレイヤー検知")]
    public Transform playerTransform;
    public float sightRange = 5f;        // 視界警告用
    public float stopDistance = 2.0f;    // 停止距離

    private bool isWarningShown = false;

    private void Start()
    {
        playerTransform = transform.Find("Player");
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 距離が近くなったら停止（イベント用）
        if (!isStopped && distance <= stopDistance)
        {
            StopMovement();
        }

        // 停止していなければ移動を継続
        if (!isStopped)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // プレイヤーが視界内に入ったら警告（警告は常に動作）
        if (distance <= sightRange)
        {
            if (!isWarningShown)
            {
                ShowWarning("擬態しろ！");
                isWarningShown = true;
            }
        }
        else
        {
            if (isWarningShown)
            {
                HideWarning();
                isWarningShown = false;
            }
        }
    }

    /// <summary>
    /// 停止処理（移動停止、今後イベント呼び出しなど）
    /// </summary>
    private void StopMovement()
    {
        isStopped = true;
        Debug.Log("座敷童子が停止しました。イベント開始準備OK。");

        // TODO: ここでタヌキ出現処理や葉の付与を呼ぶ
        // TanukiManager.Instance.SpawnTanuki(); など
    }

    /// <summary>
    /// プレイヤーが視界にいるか（単純距離）
    /// </summary>
    private void ShowWarning(string message)
    {
        Debug.Log("[警告] " + message);
        //TutorialUI.Instance?.ShowMessage(message);
    }

    private void HideWarning()
    {
        //TutorialUI.Instance?.HideMessage();
    }

    /// <summary>
    /// プレイヤーに当たっても何も起きない（チュートリアル用）
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("座敷童子にぶつかった（無害）");
        }
    }
}
