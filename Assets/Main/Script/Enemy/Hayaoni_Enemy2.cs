using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Hayaoni_Enemy2 : MonoBehaviour
{
    public float moveSpeed = 3f;         // 出現時の移動速度（右→左）
    public float chaseSpeed = 5f;        // 追跡時の移動速度（左→右）
    public float stopDistance = 3f;      // プレイヤーとの停止距離

    private enum State { Appear, Wait, Chase }
    private State currentState = State.Appear;

    private Transform player;
    private Rigidbody2D rb;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        // Rigidbody2D 設定（非推奨な isKinematic の代わりに bodyType を使用）
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // トリガー設定
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        Debug.Log("敵：出現 → 左に移動開始");
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Appear:
                Move(Vector2.left, moveSpeed);
                if (Vector2.Distance(transform.position, player.position) < stopDistance)
                {
                    currentState = State.Wait;
                    Debug.Log("敵：停止 → プレイヤーの擬態解除を待機");
                }
                break;

            case State.Wait:
                if (!IsPlayerHiding())
                {
                    currentState = State.Chase;
                    Debug.Log("敵：擬態解除を検知 → 追跡開始！");
                }
                break;

            case State.Chase:
                Move(Vector2.right, chaseSpeed);
                break;
        }
    }

    private void Move(Vector2 direction, float speed)
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private bool IsPlayerHiding()
    {
        var joycon = player.GetComponent<PlayerMove>();
        if (joycon != null) return PlayerMove.GetisHiding();

        var ps4 = player.GetComponent<PlayerMove_PS4>();
        if (ps4 != null) return PlayerMove_PS4.GetIsHiding();

        return false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentState == State.Appear)
            {
                if (!IsPlayerHiding())
                {
                    Debug.Log("敵：擬態していないプレイヤーを発見 → ゲームオーバー");
                    GameOver();
                }
                else
                {
                    Debug.Log("敵：擬態中のプレイヤーをスルー");
                }
            }
            else if (currentState == State.Chase)
            {
                Debug.Log("敵：追跡中にプレイヤーに接触 → ゲームオーバー");
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("ゲームオーバー処理開始");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}