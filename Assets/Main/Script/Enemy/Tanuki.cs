using UnityEngine;

public class Tanuki : MonoBehaviour
{
    [Header("�ړ��ݒ�")]
    public float moveSpeed = 2f;
    private bool isStopped = false;

    [Header("�v���C���[���m")]
    public Transform playerTransform;
    public float sightRange = 5f;        // ���E�x���p
    public float stopDistance = 2.0f;    // ��~����

    private bool isWarningShown = false;

    private void Start()
    {
        playerTransform = transform.Find("Player");
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // �������߂��Ȃ������~�i�C�x���g�p�j
        if (!isStopped && distance <= stopDistance)
        {
            StopMovement();
        }

        // ��~���Ă��Ȃ���Έړ����p��
        if (!isStopped)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }

        // �v���C���[�����E���ɓ�������x���i�x���͏�ɓ���j
        if (distance <= sightRange)
        {
            if (!isWarningShown)
            {
                ShowWarning("�[�Ԃ���I");
                isWarningShown = true;
            }
        }
        else
        {
            if (isWarningShown)
            {
                HideWarning();
                isWarningShown = false;
            }
        }
    }

    /// <summary>
    /// ��~�����i�ړ���~�A����C�x���g�Ăяo���Ȃǁj
    /// </summary>
    private void StopMovement()
    {
        isStopped = true;
        Debug.Log("���~���q����~���܂����B�C�x���g�J�n����OK�B");

        // TODO: �����Ń^�k�L�o��������t�̕t�^���Ă�
        // TanukiManager.Instance.SpawnTanuki(); �Ȃ�
    }

    /// <summary>
    /// �v���C���[�����E�ɂ��邩�i�P�������j
    /// </summary>
    private void ShowWarning(string message)
    {
        Debug.Log("[�x��] " + message);
        //TutorialUI.Instance?.ShowMessage(message);
    }

    private void HideWarning()
    {
        //TutorialUI.Instance?.HideMessage();
    }

    /// <summary>
    /// �v���C���[�ɓ������Ă������N���Ȃ��i�`���[�g���A���p�j
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("���~���q�ɂԂ������i���Q�j");
        }
    }
}
