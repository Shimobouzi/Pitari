using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class NewPlayerMove : MonoBehaviour
{
    // ==========================
    // ▼ 設定項目（インスペクターで調整可能）
    // ==========================
    [Header("移動設定")]
    [Tooltip("通常の移動速度")]
    [SerializeField] private float moveSpeed = 0.01f;

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
    private float MoveSpeed;               // 初期移動速度
    private bool isMoving = false;         // 現在移動中かどうか
    private bool isHiding = false;         // 擬態中かどうか
    private bool isFirst = true;           // 初回移動かどうか
    //private bool isDashing = false;        // ダッシュ状態かどうか


    // ==========================
    // ▼ 初期化
    // ==========================
    void Start()
    {
        MoveSpeed = moveSpeed;
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
        Oricon();
    }

    private void MoveRight(bool isMove, bool isRun)
    { 
        if (isMove)
        {
            p_animator.SetBool("isWalk", true);
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(DontHidePlayer());
            }
            Debug.Log("右移動");
            transform.position += Vector3.right * MoveSpeed;
        }else if (isRun)
        {
            p_animator.SetBool("isWalk", true);
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(DontHidePlayer());
            }
            Debug.Log("右ダッシュ移動");
            transform.position += Vector3.right * MoveSpeed * 1.3f;
        }
        else
        {
            p_animator.SetBool("isWalk", false);
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
        if (!isFirst)
        {
            Effect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Effect.SetActive(false);
        }
        isFirst = false;
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
        MoveRight(Input_Player.Instance.RightMove_performed, Input_Player.Instance.RightDash_Performed);
        MoveLeft(Input_Player.Instance.LeftMove_Performed);
    }

    private void Oricon()
    {
        MoveRight(OriconManager.instance.pvcController(), false);
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

    private void MoveLeft(bool isMove)
    {
        if (isMove)
        {
            transform.position += Vector3.left * MoveSpeed * 10;
        }
    }
}
