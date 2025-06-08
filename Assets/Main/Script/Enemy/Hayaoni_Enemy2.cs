using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class HayaoEnemy : MonoBehaviour
{
    public float moveSpeed = 4f;
    public Vector2 visionSize = new Vector2(3f, 2f);
    public Vector2 visionOffset = new Vector2(-1.5f, 0f);
    public LayerMask playerLayer;

    private Transform player;
    private Rigidbody2D rb;
    private bool hasDetectedPlayer = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log("速鬼：プレイヤーを取得");

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        Debug.Log("速鬼：初期化完了");
    }

    private void Update()
    {
        if (hasDetectedPlayer) return;

        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        Vector2 visionCenter = (Vector2)transform.position + visionOffset;
        Collider2D hit = Physics2D.OverlapBox(visionCenter, visionSize, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("速鬼：視界内にプレイヤーを検出");

            if (!IsPlayerHiding())
            {
                hasDetectedPlayer = true;
                Debug.Log("速鬼：擬態していないプレイヤーを視界で発見 → ゲームオーバー");
                Invoke(nameof(RestartScene), 0.1f); // 少し遅延させて安全にリスタート
            }
            else
            {
                Debug.Log("速鬼：擬態中のプレイヤーを視界でスルー");
            }
        }
    }

    private bool IsPlayerHiding()
    {
        var joycon = player.GetComponent<PlayerMove>();
        if (joycon != null) return PlayerMove.GetisHiding();

        var ps4 = player.GetComponent<PlayerMove_PS4>();
        if (ps4 != null) return PlayerMove_PS4.GetIsHiding();

        Debug.LogWarning("速鬼：プレイヤーの擬態状態を取得できませんでした");
        return false;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 visionCenter = (Vector2)transform.position + visionOffset;
        Gizmos.DrawWireCube(visionCenter, visionSize);
    }
}
