using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class HaritsukiMan : MonoBehaviour
{
    [Header("アニメーション制御")]
    public Animator animator;

    [Header("プレイヤー検出設定")]
    public LayerMask playerLayer;

    [Header("当たり判定のサイズと位置（固定）")]
    public Vector2 detectionSize = new Vector2(2f, 2f);
    public Vector2 detectionOffset = new Vector2(1f, 0f);

    [Header("当たり判定コライダー")]
    public BoxCollider2D detectionCollider;

    private bool isHandOpen = false;
    private bool isEyeVisible = false;

    [Header("状態変化の時間（秒）")]
    public float minOpenDuration = 2f;
    public float maxOpenDuration = 4f;
    public float minCloseDuration = 3.5f;
    public float maxCloseDuration = 5.5f;

    private void Start()
    {
        if (detectionCollider == null)
        {
            detectionCollider = GetComponent<BoxCollider2D>();
        }

        detectionCollider.isTrigger = true;
        detectionCollider.size = detectionSize;
        detectionCollider.offset = detectionOffset;
        detectionCollider.enabled = false;

        Debug.Log("HaritsukiMan: 初期化完了。状態変化ルーチン開始。");
        StartCoroutine(RandomizeStateRoutine());
    }

    private void Update()
    {
        if (isHandOpen && isEyeVisible)
        {
            Vector2 center = (Vector2)transform.position + detectionOffset;
            Collider2D hit = Physics2D.OverlapBox(center, detectionSize, 0f, playerLayer);

            if (hit != null)
            {
                Debug.Log($"HaritsukiMan: プレイヤーと接触中 → {hit.name}");

                if (hit.CompareTag("Player"))
                {
                    var playerMove = hit.GetComponent<NewPlayerMove>();
                    if (playerMove != null)
                    {
                        if (!playerMove.GetisHiding())
                        {
                            Debug.Log("HaritsukiMan: 擬態していないプレイヤーを発見 → ゲームオーバー処理へ");
                            Invoke(nameof(RestartScene), 0.1f);
                        }
                        else
                        {
                            Debug.Log("HaritsukiMan: プレイヤーは擬態中 → スルー");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("HaritsukiMan: PlayerMove コンポーネントが見つかりませんでした。");
                    }
                }
            }
        }
    }

    private IEnumerator RandomizeStateRoutine()
    {
        while (true)
        {
            isHandOpen = Random.value > 0.5f;
            isEyeVisible = isHandOpen;

            animator.SetBool("HirakuBool", isHandOpen);
            detectionCollider.enabled = isHandOpen && isEyeVisible;

            float waitTime = isHandOpen
                ? Random.Range(minOpenDuration, maxOpenDuration)
                : Random.Range(minCloseDuration, maxCloseDuration);

            Debug.Log($"HaritsukiMan: 状態更新 → {(isHandOpen ? "手開" : "手閉")}, 判定:{detectionCollider.enabled}, 次の状態まで:{waitTime:F2}秒");

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + detectionOffset;
        Gizmos.DrawWireCube(center, detectionSize);
    }
}
