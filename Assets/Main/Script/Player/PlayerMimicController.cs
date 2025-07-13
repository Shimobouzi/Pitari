using UnityEngine;

public class PlayerMimicController : MonoBehaviour
{
    public float speed = 3f;
    public Sprite normalSprite;
    public Sprite mimicSprite;
    public GameObject smokeEffectPrefab;

    private SpriteRenderer sr;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isMimicking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float move = 0f;

        // 入力取得（A = 左, S = 右）
        if (Input.GetKey(KeyCode.A)) move -= 1f;
        if (Input.GetKey(KeyCode.S)) move += 1f;

        // プレイヤー移動
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);

        // 向き反転
        if (move != 0)
            sr.flipX = move < 0;

        // 状態切り替え（移動してる？）
        if (Mathf.Abs(move) > 0.01f)
        {
            if (isMimicking)
                Unmimic();
        }
        else
        {
            if (!isMimicking)
                Mimic();
        }
    }

    void Mimic()
    {
        isMimicking = true;
        Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
        animator.enabled = false;
        sr.sprite = mimicSprite;
    }

    void Unmimic()
    {
        isMimicking = false;
        Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
        sr.sprite = normalSprite;
        animator.enabled = true;
    }
}