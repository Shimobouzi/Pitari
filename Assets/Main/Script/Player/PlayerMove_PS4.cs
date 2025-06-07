using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMove_PS4 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;           // 通常の移動速度
    [SerializeField] private float boostMultiplier = 2f;     // ブースト時の速度倍率
    [SerializeField] private float boostDuration = 5f;       // ブーストの持続時間
    [SerializeField] private float hideDelay = 0.2f;         // 擬態の遅延時間

    [SerializeField] private GameObject People;              // 人の見た目
    [SerializeField] private GameObject Object;              // モノの見た目
    [SerializeField] private GameObject Effect;              // エフェクト（煙など）

    private PlayerControls controls;
    private Vector2 moveInput;
    private float currentSpeed;
    private bool isMoving = false;
    private static bool isHiding = false;
    private bool isBoosting = false;

    private void Awake()
    {
        controls = new PlayerControls(); // InputSystemの自動生成クラスのインスタンス作成

        // 移動入力：スティックを動かしたとき
        controls.Player.Move.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();

            if (moveInput != Vector2.zero)
            {
                ShowPlayer(); // 擬態解除（人に戻る）
            }
        };

        // 移動入力終了：スティックを離したとき
        controls.Player.Move.canceled += ctx =>
        {
            moveInput = Vector2.zero;
            isMoving = false;
            StartCoroutine(HidePlayer()); // 擬態化処理をスタート
        };
    }


    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Start()
    {
        currentSpeed = moveSpeed;
        People.SetActive(true);
        Object.SetActive(false);
        Effect.SetActive(false);
    }

    private void Update()
    {
        if (moveInput != Vector2.zero)
        {
            // 左スティックで移動
            Vector3 move = new Vector3(moveInput.x, 0f, 0f);//横方向のみ
            transform.Translate(move * currentSpeed * Time.deltaTime);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// 一定時間だけブースト（速度を上げる）
    /// </summary>
    public void StartBoost()
    {
        if (!isBoosting)
        {
            StartCoroutine(BoostCoroutine());
        }
    }

    private IEnumerator BoostCoroutine()
    {
        isBoosting = true;
        currentSpeed = moveSpeed * boostMultiplier;
        yield return new WaitForSeconds(boostDuration);
        currentSpeed = moveSpeed;
        isBoosting = false;
    }

    /// <summary>
    /// モノに擬態する処理
    /// </summary>
    private IEnumerator HidePlayer()
    {
        yield return new WaitForSeconds(hideDelay);
        if (moveInput != Vector2.zero) yield break;

        isHiding = true;
        People.SetActive(false);
        Object.SetActive(true);
        Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Effect.SetActive(false);
    }

    /// <summary>
    /// 人の見た目に戻る処理
    /// </summary>
    private void ShowPlayer()
    {
        if (isHiding)
        {
            isHiding = false;
            Object.SetActive(false);
            People.SetActive(true);
            Effect.SetActive(true);
            Invoke(nameof(DisableEffect), 0.5f);
        }
    }

    private void DisableEffect()
    {
        Effect.SetActive(false);
    }

    /// <summary>
    /// 擬態状態の取得
    /// </summary>
    public static bool GetIsHiding()
    {
        return isHiding;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("EnemySight"))
        {
            if (PlayerMove.GetisHiding())
            {
                Debug.Log("プレイヤー：視界内にいるけど擬態中（セーフ）");
            }
            else
            {
                Debug.Log("プレイヤー：視界内にいて擬態してない！（アウト）");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("EnemySight"))
        {
            Debug.Log("プレイヤー：視界外に出た");
        }
    }

}
