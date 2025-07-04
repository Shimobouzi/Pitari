using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class NewPlayerMove : MonoBehaviour
{
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
    //private float originalMoveSpeed;       // 初期移動速度
    //private float dynamicSpeed;            // 実際に使う移動速度（加速度で変動）
    private bool isMoving = false;         // 現在移動中かどうか
    private bool isHiding = false;         // 擬態中かどうか
    //private bool isDashing = false;        // ダッシュ状態かどうか


    // ==========================
    // ▼ 初期化
    // ==========================
    void Start()
    {
        //originalMoveSpeed = moveSpeed;
        //dynamicSpeed = moveSpeed;
        p_animator = GetComponent<Animator>();

        // 初期状態：人の姿でスタート
        People.SetActive(true);
        Object.SetActive(false);
        Effect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Joycon();
    }

    private void MoveRight(bool isMove)
    {
        if (isMove)
        {
            p_animator.SetBool("isFirstTime", true);
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(DontHidePlayer());
            }
            Debug.Log("右移動");
            transform.position += Vector3.right / 10.0f;
        }else
        {
            if (isMoving)
            {
                isMoving = false;
                StartCoroutine(HidePlayer());
            }
        }
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
        if (!isMoving) yield break;
        isHiding = false;
        Object.SetActive(false);
        People.SetActive(true);
        Effect.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        Effect.SetActive(false);
    }

    // ==========================
    // ▼ 敵に当たったときの処理
    // ==========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //JoyconController.Instance.OnBuruBuru(); // 振動
            Debug.Log("敵に当たりました！");
        }
    }

    // ==========================
    // ▼ 外部から擬態状態を取得する
    // ==========================
    public bool GetisHiding()
    {
        return isHiding;
    }


    private void Joycon()
    {
        MoveRight(Input_Player.Instance.RightMove_performed);
    }

    private void Oricon()
    {

    }

    private void MediaPipe()
    {

    }

    //private void KeyCon()
    //{
    //}

    private void Preste()
    {

    }
}
