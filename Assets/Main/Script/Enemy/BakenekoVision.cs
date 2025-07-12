using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BakenekoSight : MonoBehaviour
{
    [Header("視線の広がり")]
    public float visionWidth = 1.5f;
    public float visionLength = 3f;

    [Header("視線の原点オフセット")]
    public Vector2 visionOffset = Vector2.zero;

    [Header("目の開閉アニメーション")]
    public Animator animator;

    [Header("開閉時間（秒）")]
    public float minOpenDuration = 2.5f;
    public float maxOpenDuration = 4f;
    public float minCloseDuration = 2.5f;
    public float maxCloseDuration = 5f;

    private PolygonCollider2D visionCollider;
    private bool eyeOpen = false;

    void Awake()
    {
        visionCollider = GetComponent<PolygonCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.gravityScale = 0;

        visionCollider.isTrigger = true;
        visionCollider.enabled = false;

        UpdateVisionShape();
    }

    void Start()
    {
        StartCoroutine(EyeStateRoutine());
    }

    void UpdateVisionShape()
    {
        Vector2[] trianglePoints = new Vector2[3];
        trianglePoints[0] = visionOffset;
        trianglePoints[1] = visionOffset + new Vector2(visionWidth, -visionLength);
        trianglePoints[2] = visionOffset + new Vector2(-visionWidth, -visionLength);
        visionCollider.points = trianglePoints;
    }

    IEnumerator EyeStateRoutine()
    {
        while (true)
        {
            // 目を開く
            SetEyeState(true);
            float openTime = Random.Range(minOpenDuration, maxOpenDuration);
            yield return new WaitForSeconds(openTime);

            // 目を閉じる
            SetEyeState(false);
            float closeTime = Random.Range(minCloseDuration, maxCloseDuration);
            yield return new WaitForSeconds(closeTime);
        }
    }

    void SetEyeState(bool open)
    {
        eyeOpen = open;
        visionCollider.enabled = eyeOpen;

        if (animator != null)
        {
            animator.Play(open ? "EyeOpen" : "EyeClose");
        }

        Debug.Log($"化け猫: {(eyeOpen ? "目を開いた" : "目を閉じた")}（判定: {visionCollider.enabled}）");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!eyeOpen) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("ゲームオーバー: プレイヤーが視線に入った！");
            SceneManager.LoadScene("Result");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + visionOffset;
        Gizmos.DrawLine(center, center + new Vector2(visionWidth, -visionLength));
        Gizmos.DrawLine(center, center + new Vector2(-visionWidth, -visionLength));
        Gizmos.DrawLine(center + new Vector2(visionWidth, -visionLength), center + new Vector2(-visionWidth, -visionLength));
    }
}
