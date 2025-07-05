using UnityEngine;
using System.Collections;

/// <summary>
/// Joyconの加速度を使ってプレイヤーを移動・変身・ダッシュさせるスクリプト
/// </summary>
public class PlayerMove3 : MonoBehaviour
{
    // ==========================
    // ▼ 設定項目（インスペクターで調整可能）
    // ==========================

    [Header("加速度と移動設定")]
    [Tooltip("この加速度を超えると『ダッシュ』と判定される")]
    [SerializeField] private float accelThreshold = 2.5f;

    [Tooltip("通常の移動速度")]
    [SerializeField] private float moveSpeed = 3f;

    [Tooltip("加速度に応じた速度倍率（強く振るほど速く）")]
    [SerializeField] private float speedMultiplier = 2f;

    [Tooltip("1回の移動時間（秒）")]
    [SerializeField] private float duration = 0.2f;

    [Tooltip("ダッシュ速度の持続時間（秒）")]
    [SerializeField] private float boostDuration = 5f;

    [Header("擬態設定")]
    [Tooltip("擬態（モノに変身）するまでの遅延時間")]
    [SerializeField] private float hideDelay = 0.2f;

    [Tooltip("人の見た目オブジェクト")]
    [SerializeField] private GameObject People;

    [Tooltip("モノの見た目オブジェクト")]
    [SerializeField] private GameObject Object;

    [Tooltip("変身時のエフェクト")]
    [SerializeField] private GameObject Effect;

    // ==========================
    // ▼ 内部変数
    // ==========================

    private Animator p_animator;           // アニメーター
    private float originalMoveSpeed;       // 初期移動速度
    private float dynamicSpeed;            // 実際に使う移動速度（加速度で変動）
    private bool isMoving = false;         // 現在移動中かどうか
    private bool isFirst = true;           // 初回移動かどうか
    private static bool isHiding = false;  // 擬態中かどうか
    private bool isDashing = false;        // ダッシュ状態かどうか

    // ==========================
    // ▼ 初期化
    // ==========================
    void Start()
    {
        originalMoveSpeed = moveSpeed;
        dynamicSpeed = moveSpeed;
        p_animator = GetComponent<Animator>();

        // 初期状態：人の姿でスタート
        People.SetActive(true);
        Object.SetActive(false);
        Effect.SetActive(false);
    }

    // ==========================
    // ▼ 毎フレームの処理
    // ==========================
    void Update()
    {
        // Joyconの加速度を取得
        Vector3 accel = JoyconUpdate();
        float accelPower = accel.magnitude;

        // ダッシュ判定（加速度がしきい値を超えたら）
        isDashing = accelPower >= accelThreshold;

        // ダッシュ時の移動処理
        if (isDashing && !isMoving)
        {
            // 加速度に応じて速度を強化（最大4倍）
            float strengthMultiplier = Mathf.Clamp((accelPower - accelThreshold) * speedMultiplier, 1f, 4f);
            dynamicSpeed = originalMoveSpeed * strengthMultiplier;

            // 一定時間後に速度をリセット
            StartCoroutine(ResetSpeedAfterDelay(boostDuration));

            // 擬態中なら人に戻る
            if (isHiding) StartCoroutine(DontHidePlayer());

            // プレイヤーを移動させる
            StartCoroutine(MovePlayer());

            isFirst = false;
        }
        // 動いていない＆擬態していない＆初回移動済み → 擬態する
        else if (!isFirst && !isHiding && !isMoving)
        {
            StartCoroutine(HidePlayer());
        }

        // アニメーション制御（Animatorに "isDashing" パラメータが必要）
        p_animator.SetBool("isDashing", isDashing);
    }

    // ==========================
    // ▼ プレイヤーを一定時間移動させる
    // ==========================
    IEnumerator MovePlayer()
    {
        p_animator.SetBool("isFirstTime", true);
        isMoving = true;
        float timer = 0f;

        AudioManager.PlaySE("playerWalk");

        while (timer < duration)
        {
            transform.Translate(Vector3.right * dynamicSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        dynamicSpeed = originalMoveSpeed;
        isMoving = false;
    }

    // ==========================
    // ▼ モノに擬態する処理
    // ==========================
    IEnumerator HidePlayer()
    {
        yield return new WaitForSeconds(hideDelay);
        if (isMoving) yield break;

        isHiding = true;
        People.SetActive(false);
        Object.SetActive(true);
        Effect.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        Effect.SetActive(false);
    }

    // ==========================
    // ▼ 人に戻る処理
    // ==========================
    IEnumerator DontHidePlayer()
    {
        isHiding = false;
        Object.SetActive(false);
        People.SetActive(true);
        Effect.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        Effect.SetActive(false);
    }

    // ==========================
    // ▼ 一定時間後に速度をリセット
    // ==========================
    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dynamicSpeed = originalMoveSpeed;
    }

    // ==========================
    // ▼ 敵に当たったときの処理
    // ==========================
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            JoyconController.Instance.OnBuruBuru(); // 振動
            Debug.Log("敵に当たりました！");
        }
    }

    // ==========================
    // ▼ 外部から擬態状態を取得する
    // ==========================
    public static bool GetisHiding()
    {
        return isHiding;
    }

    // ==========================
    // ▼ 外部から加速度を取得する
    // ==========================
    public Vector3 GetAccel()
    {
        return JoyconUpdate();
    }

    // ==========================
    // ▼ Joyconの加速度を取得
    // ==========================
    private Vector3 JoyconUpdate()
    {
        return JoyconController.Instance.GetAccel();
    }

    // ==========================
    // ▼ 外部からダッシュ状態を取得
    // ==========================
    // public bool IsDashing()
    // {
    //     return isDashing;
    // }
}
