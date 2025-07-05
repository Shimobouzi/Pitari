using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public static EnemyBase instance;

    protected float nomalSpeed = 2f;
    protected float runSpeed = 10f;
    protected float coolTime;
    protected int isLeft = 1;
    protected Vector3 thisScale;

    protected void Awake()
    {
        instance = this;
    }
    protected virtual void Start()
    {
        thisScale = transform.localScale;
        Idel();
    }

    protected void Update()
    {
        transform.localScale = new Vector3(thisScale.x * isLeft, thisScale.y, thisScale.z);
    }

    protected void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StopAllCoroutines();
            StartCoroutine(ChasePlayer(5f, runSpeed));
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        { 
            Debug.Log("Attack");
            StopAllCoroutines();
            StartCoroutine(AttackPlayer());
        }
    }

    protected virtual void Idel()
    {
        StartCoroutine(LeftRightMove(5f, nomalSpeed));
    }

    protected IEnumerator ChasePlayer(float duration, float moveSpeed)
    {
        if (isLeft == 1)
        {
            StartCoroutine(MoveLeft(duration, moveSpeed));
        }
        else
        {
            StartCoroutine(MoveRight(duration, moveSpeed));
        }
        yield return new WaitForSeconds(duration);
        Idel();
    }

    protected IEnumerator AttackPlayer()
    {
        JoyconController.Instance.OnBuruBuru();
        yield return new WaitForSeconds(1f);
        PitariSceneManager.Instance.ToResult(false);
    }

    protected IEnumerator MoveRight(float duration, float moveSpeed)
    {
        isLeft = -1;
        float timer = 0f;
        while (timer < duration)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); // 2Dなら右方向に移動
            //一秒間に指定した数値分右に動く処理
            timer += Time.deltaTime;
            yield return null;
        }
    }

    protected IEnumerator MoveLeft(float duration, float moveSpeed)
    {
        isLeft = 1;
        float timer = 0f;
        while (timer < duration)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime); // 2Dなら左方向に移動
            //一秒間に指定した数値分右に動く処理
            timer += Time.deltaTime;
            yield return null;
        }
    }

    protected IEnumerator LeftRightMove(float duration, float moveSpeed)
    {
        StartCoroutine(MoveRight(duration, moveSpeed));
        yield return new WaitForSeconds(duration);
        StartCoroutine(MoveLeft(duration, moveSpeed));
        yield return new WaitForSeconds(duration);
        StartCoroutine(LeftRightMove(duration, moveSpeed));
    }
}
