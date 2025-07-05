//プレイヤーを視界範囲で検知して追跡後当たるとゲームオーバー
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyChase : MonoBehaviour
{
    public Transform player;           // プレイヤーの Transform
    public float moveSpeed = 3f;       // 敵の移動速度
    public float smoothTime = 0.2f;    // 追跡の滑らかさ

    private Rigidbody2D rb;
    private Vector2 velocity = Vector2.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector2 targetPosition = player.position;
            Vector2 newPosition = Vector2.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, moveSpeed);
            rb.MovePosition(newPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("【敵】プレイヤーに接触しました！ → ゲームオーバー処理を実行します。");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
