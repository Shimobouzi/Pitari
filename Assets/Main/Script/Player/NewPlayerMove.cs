using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

public class NewPlayerMove : MonoBehaviour
{
    // ==========================
    // �� �ݒ荀�ځi�C���X�y�N�^�[�Œ����\�j
    // ==========================
    [Header("�ړ��ݒ�")]
    [Tooltip("�ʏ�̈ړ����x")]
    [SerializeField] private float moveSpeed = 0.01f;

    [Header("�[�Ԑݒ�")]
    [Tooltip("�[�ԁi���m�ɕϐg�j����܂ł̒x������")]
    [SerializeField] private float hideDelay = 0.3f;

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
    private float MoveSpeed;               // �����ړ����x
    private bool isMoving = false;         // ���݈ړ������ǂ���
    private bool isHiding = false;         // �[�Ԓ����ǂ���
    private bool isFirst = true;           // ����ړ����ǂ���
    //private bool isDashing = false;        // �_�b�V����Ԃ��ǂ���


    // ==========================
    // �� ������
    // ==========================
    void Start()
    {
        MoveSpeed = moveSpeed;
        p_animator = GetComponent<Animator>();

        // ������ԁF�l�̎p�ŃX�^�[�g
        People.SetActive(true);
        Object.SetActive(false);
        Effect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PitariDB.Instance.GetConBool() == 0)
        {
            Joycon();
        }else if (PitariDB.Instance.GetConBool() == 1)
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
            Debug.Log("�E�ړ�");
            transform.position += Vector3.right * MoveSpeed;
        }else if (isRun)
        {
            p_animator.SetBool("isWalk", true);
            if (!isMoving)
            {
                isMoving = true;
                StartCoroutine(DontHidePlayer());
            }
            Debug.Log("�E�_�b�V���ړ�");
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
        if (!isFirst)
        {
            Effect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Effect.SetActive(false);
        }
        isFirst = false;
    }

    // ==========================
    // �� �G�ɓ��������Ƃ��̏���
    // ==========================
    public void OnBuruBuru()
    {
        Input_Player.Instance.OnBuruburu();
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
        MoveRight(Input_Player.Instance.RightMove_performed, Input_Player.Instance.RightDash_Performed);
        MoveLeft(Input_Player.Instance.LeftMove_Performed);
    }

    private void Oricon()
    {
        MoveRight(OriconManager.instance.pvcController(), OriconManager.instance.pvcDash());
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
