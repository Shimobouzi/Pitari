using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KeyPlayerMove : MonoBehaviour
{
    [SerializeField]
    private float hideDelay = 0.2f; //隠れるラグ

    [SerializeField]
    private float moveSpeed = 3f;

    //初めて移動したらfalse
    private bool isFirst = true;

    // プレイヤーが移動中かどうかのフラグ
    private bool isMoving = false;

    //プレイヤーが隠れているかのフラグ
    private static bool isHiding = false;

    private Animator p_animator;

    /*Editor内格納*/
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
            if (isHiding)//モノから人に化ける処理
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

        // ぶつかったオブジェクトの名前を取得
        Debug.Log("衝突したオブジェクト: " + collision.gameObject.name);

        // もし特定のタグを持つオブジェクトと衝突したら
        if (collision.gameObject.CompareTag("Enemy"))
        {
            JoyconController.Instance.OnBuruBuru();
            Debug.Log("敵に当たりました！");
        }
    }

    /// <summary>
    /// モノに化ける処理
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
    /// モノから人に化ける処理
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
