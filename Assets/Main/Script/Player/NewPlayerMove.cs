using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class NewPlayerMove : MonoBehaviour
{
    [Header("�[�Ԑݒ�")]
    [Tooltip("�[�ԁi���m�ɕϐg�j����܂ł̒x������")]
    [SerializeField] private float hideDelay = 0.2f;

    [Tooltip("�l�̌����ڃI�u�W�F�N�g")]
    [SerializeField] private GameObject People;

    [Tooltip("���m�̌����ڃI�u�W�F�N�g")]
    [SerializeField] private GameObject Object;

    [Tooltip("�ϐg���̃G�t�F�N�g")]
    [SerializeField] private GameObject Effect;


    // ==========================
    // �� �����ϐ�
    // ==========================

    private Animator p_animator;           // �A�j���[�^�[
    //private float originalMoveSpeed;       // �����ړ����x
    //private float dynamicSpeed;            // ���ۂɎg���ړ����x�i�����x�ŕϓ��j
    private bool isMoving = false;         // ���݈ړ������ǂ���
    private bool isHiding = false;         // �[�Ԓ����ǂ���
    //private bool isDashing = false;        // �_�b�V����Ԃ��ǂ���


    // ==========================
    // �� ������
    // ==========================
    void Start()
    {
        //originalMoveSpeed = moveSpeed;
        //dynamicSpeed = moveSpeed;
        p_animator = GetComponent<Animator>();

        // ������ԁF�l�̎p�ŃX�^�[�g
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
            Debug.Log("�E�ړ�");
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
    // �� ���m�ɋ[�Ԃ��鏈��
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
    // �� �l�ɖ߂鏈��
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
    // �� �G�ɓ��������Ƃ��̏���
    // ==========================
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //JoyconController.Instance.OnBuruBuru(); // �U��
            Debug.Log("�G�ɓ�����܂����I");
        }
    }

    // ==========================
    // �� �O������[�ԏ�Ԃ��擾����
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
