using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class NewPlayerMove : MonoBehaviour
{
    // ==========================
    // ■ 設定項目（インスペクターで調整可能）
    // ==========================
    [Header("移動設定")]
    [Tooltip("通常の移動速度")]
    [SerializeField] private float moveSpeed = 0.01f;

    [Header("擬態設定")]
    [Tooltip("擬態（物陰に隠れる）までの待機時間")]
    [SerializeField] private float hideDelay = 0.3f;

    [Tooltip("人の見た目のオブジェクト")]
    [SerializeField] private GameObject People;

    [Tooltip("物の見た目のオブジェクト")]
    [SerializeField] private GameObject Object;

    [Tooltip("擬態時のエフェクト")]
    [SerializeField] private GameObject Effect;

    [Header("振動コントローラー")]
    [Tooltip("振動コントローラー（Inspectorでリンク）")]
    public VibrationController vibrationController;

    // ==========================
    // ■ 内部変数
    // ==========================
    private Animator p_animator;
    private float MoveSpeed;
    private bool isMoving = false;
    private bool isHiding = false;
    private bool isFirst = true;

    // ==========================
    // ■ 初期化処理
    // ==========================
    void Start()
    {
        MoveSpeed = moveSpeed;
        p_animator = GetComponent<Animator>();

        People.SetActive(true);
        Object.SetActive(false);
        Effect.SetActive(false);
    }

    void Update()
    {
        if (PitariDB.Instance.GetConBool() == 0)
        {
            Joycon();
        }
        else if (PitariDB.Instance.GetConBool() == 1)
        {
            Oricon();
        }
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
            transform.position += Vector3.right * MoveSpeed;
        }
        else if (isRun)
        {
            p_animator.SetBool("isWalk", true);
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(DontHidePlayer());
            }
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

    // public void OnBuruBuru()
    // {
    //     Input_Player.Instance.OnBuruBuru();
    // }

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
        MoveRight(OriconManager.instance.pvcController(), OriconManager.instance.pvcDash());
    }

    private void MoveLeft(bool isMove)
    {
        if (isMove)
        {
            transform.position += Vector3.left * MoveSpeed * 10;
        }
    }

    // ==========================
    // ■ 敵との接触時の処理（振動）
    // ==========================
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (vibrationController != null)
            {
                vibrationController.OnBuruburu();
            }
        }
    }
}
