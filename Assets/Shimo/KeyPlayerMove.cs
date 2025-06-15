using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyPlayerMove : MonoBehaviour
{
    [SerializeField]
    private float hideDelay = 0.2f; //�B��郉�O

    [SerializeField]
    private float moveSpeed = 3f;

    //���߂Ĉړ�������false
    private bool isFirst = true;

    // �v���C���[���ړ������ǂ����̃t���O
    private bool isMoving = false;

    //�v���C���[���B��Ă��邩�̃t���O
    private static bool isHiding = false;

    private Animator p_animator;

    /*Editor���i�[*/
    [SerializeField]
    private GameObject People;
    [SerializeField]
    private GameObject Object;
    [SerializeField]
    private GameObject Effect;

    private void Start()
    {
        p_animator = GetComponent<Animator>();
        People.SetActive(true);
        Object.SetActive(false);
        Effect.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (isHiding)//���m����l�ɉ����鏈��
            {
                StartCoroutine(DontHidePlayer());
            }
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            isFirst = false;
        
        }else if(!isFirst && !isHiding)
        {
            StartCoroutine(HidePlayer());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        // �Ԃ������I�u�W�F�N�g�̖��O���擾
        Debug.Log("�Փ˂����I�u�W�F�N�g: " + collision.gameObject.name);

        // ��������̃^�O�����I�u�W�F�N�g�ƏՓ˂�����
        if (collision.gameObject.CompareTag("Enemy"))
        {
            JoyconController.Instance.OnBuruBuru();
            Debug.Log("�G�ɓ�����܂����I");
        }
    }

    /// <summary>
    /// ���m�ɉ����鏈��
    /// </summary>
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


    /// <summary>
    /// ���m����l�ɉ����鏈��
    /// </summary>
    IEnumerator DontHidePlayer()
    {
        isHiding = false;
        Object.SetActive(false);
        People.SetActive(true);
        Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Effect.SetActive(false);
    }

    public static bool GetisHiding()
    {
        return isHiding;
    }

}
