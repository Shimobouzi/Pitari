
using UnityEngine;
using UnityEngine.SceneManagement;

// このスクリプトは、敵キャラクター「速鬼」がプレイヤーを検出し、擬態していない場合にゲームオーバーにする処理を行います。
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class NewHayaoni_TypeA : MonoBehaviour
{
    // 敵の移動速度
    public float moveSpeed = 4f;

    // 視界のサイズ（幅と高さ）
    public Vector2 visionSize = new Vector2(3f, 2f);

    // 視界の中心位置のオフセット（敵の位置からの相対位置）
    public Vector2 visionOffset = new Vector2(-1.5f, 0f);

    // プレイヤーを検出するためのレイヤー
    public LayerMask playerLayer;

    private Transform player;         // プレイヤーのTransform
    private Rigidbody2D rb;           // 敵のRigidbody2D
    private bool hasDetectedPlayer = false; // プレイヤーを検出したかどうかのフラグ

    private void Start()
    {
        // プレイヤーオブジェクトをタグで取得
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("速鬼：プレイヤーが見つかりません");
            return;
        }

        // Rigidbody2Dの初期設定（重力なし、キネマティック）
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // BoxCollider2Dをトリガーに設定（衝突判定は使わない）
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        Debug.Log("速鬼：初期化完了");
    }

    private void Update()
    {
        // すでにプレイヤーを検出していたら処理をスキップ
        if (hasDetectedPlayer || player == null) return;

        // 左方向に移動
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // 視界の中心位置を計算
        Vector2 visionCenter = (Vector2)transform.position + visionOffset;

        // 視界内にプレイヤーがいるか判定
        Collider2D hit = Physics2D.OverlapBox(visionCenter, visionSize, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("速鬼：視界内にプレイヤーを検出");

            // プレイヤーが擬態していない場合、ゲームオーバー処理
            if (!IsPlayerHiding(hit.gameObject))
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

    // プレイヤーが擬態しているかどうかを判定する関数（インスタンスメソッドを使用）
    private bool IsPlayerHiding(GameObject playerObj)
    {
        var joycon = playerObj.GetComponent<NewPlayerMove>();
        if (joycon != null) return joycon.GetisHiding();

        Debug.LogWarning("速鬼：プレイヤーの擬態状態を取得できませんでした");
        return false;
    }

    // 現在のシーンを再読み込みしてゲームオーバー処理を実行
    private void RestartScene()
    {
        SceneManager.LoadScene("Result");
    }

    // Unityエディタ上で視界範囲を表示するためのGizmos描画
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 visionCenter = (Vector2)transform.position + visionOffset;
        Gizmos.DrawWireCube(visionCenter, visionSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<NewPlayerMove>().OnBuruBuru();
            Debug.Log("速鬼：プレイヤーと接触しました！");
        }
    }

}
