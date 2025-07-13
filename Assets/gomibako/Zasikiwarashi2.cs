using UnityEngine;

public class ZashikiWarashi2 : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2f;           // 座敷童の移動速度
    public float walkDuration = 3f;        // 歩く秒数
    private float walkTimer = 0f;
    private bool isWalking = false;

    [Header("アニメーション")]
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false); // 最初は非表示

        // UIが閉じたら登場するイベントを購読
        TutorialUIManager.OnTutorialClosed += HandleTutorialClosed;
    }

    private void OnDestroy()
    {
        TutorialUIManager.OnTutorialClosed -= HandleTutorialClosed;
    }

    /// <summary>
    /// UIが閉じたら座敷童を登場させて移動開始
    /// </summary>
    private void HandleTutorialClosed()
    {
        gameObject.SetActive(true);
        isWalking = true;
        walkTimer = 0f;

        if (animator != null)
        {
            animator.SetBool("isWalk", true);
        }
    }

    private void Update()
    {
        if (!isWalking) return;

        // 左方向へ移動
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        walkTimer += Time.deltaTime;

        // 指定時間歩いたら停止
        if (walkTimer >= walkDuration)
        {
            isWalking = false;

            if (animator != null)
            {
                animator.SetBool("isWalk", false);
            }

            Debug.Log("座敷童の歩行が終了しました。ここで擬態通知などを行えます。");

            // TODO: 自動プレイヤースクリプトに通知する処理をここに追加
            // 例: AutoPlayer.Instance.OnZashikiStopped();
        }
    }
}
