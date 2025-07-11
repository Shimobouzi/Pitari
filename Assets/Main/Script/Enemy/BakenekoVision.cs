using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 化け猫の視線型当たり判定（はや鬼風）
/// PolygonCollider2Dで三角形の視線領域を作成し、目の開閉に応じて当たり判定を切り替える。
/// プレイヤーが視線に入ったら即ゲームオーバー（擬態判定なし）。
/// </summary>
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BakenekoSight : MonoBehaviour
{
    [Header("視線の状態")]
    public bool eyeOpen = true;

    [Header("視線の広がり")]
    public float visionWidth = 1.5f;
    public float visionLength = 3f;

    private PolygonCollider2D visionCollider;

    void Awake()
    {
        visionCollider = GetComponent<PolygonCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Rigidbody設定（物理演算不要）
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.gravityScale = 0;

        // Trigger判定に設定
        visionCollider.isTrigger = true;

        // 初期視線形状を設定
        UpdateVisionShape();
    }

    void Update()
    {
        // 仮の目の開閉切り替え（スペースキー）
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eyeOpen = !eyeOpen;
            visionCollider.enabled = eyeOpen;
            Debug.Log("目の状態: " + (eyeOpen ? "開いている" : "閉じている"));
        }

        // 視線形状の更新（必要に応じて）
        UpdateVisionShape();
    }

    /// <summary>
    /// 三角形の視線形状を更新
    /// </summary>
    void UpdateVisionShape()
    {
        Vector2[] trianglePoints = new Vector2[3];
        trianglePoints[0] = Vector2.zero;
        trianglePoints[1] = new Vector2(-visionWidth, visionLength);
        trianglePoints[2] = new Vector2(visionWidth, visionLength);
        visionCollider.points = trianglePoints;
    }

    /// <summary>
    /// プレイヤーが視線に入ったときの処理（即ゲームオーバー）
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!eyeOpen) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("ゲームオーバー: プレイヤーが視線に入った！");
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
