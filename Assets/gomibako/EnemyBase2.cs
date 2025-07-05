using System.Collections;
using UnityEngine;

/// <summary>
/// 敵のベースクラス（共通処理）
/// </summary>
public class EnemyBase2 : MonoBehaviour
{
    [Header("移動速度設定")]
    protected float normalSpeed = 2f;
    protected float runSpeed = 10f;

    protected int isLeft = 1;
    protected Vector3 thisScale;

    protected virtual void Start()
    {
        thisScale = transform.localScale;
        Idle();
    }

    protected virtual void Update()
    {
        // 向き反転処理
        transform.localScale = new Vector3(thisScale.x * isLeft, thisScale.y, thisScale.z);
    }

    /// <summary>
    /// プレイヤーが視界に入っているか（継承先で上書き可）
    /// </summary>
    protected virtual bool IsPlayerInSight()
    {
        return false; // デフォルトでは false。必要ならオーバーライドして使う
    }

    /// <summary>
    /// プレイヤーがやられたときの処理（振動や音なし）
    /// </summary>
    protected virtual IEnumerator PlayerDeadSequence()
    {
        yield return new WaitForSeconds(1f);

        // リザルトへ遷移（false = ゲームオーバー）
        PitariSceneManager.Instance.ToResult(false);
    }

    /// <summary>
    /// プレイヤーが範囲内にいる間、追跡処理を実行
    /// </summary>
    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(ChasePlayer(5f, runSpeed));
        }
    }

    /// <summary>
    /// プレイヤーと衝突したら即ゲームオーバー演出
    /// </summary>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(PlayerDeadSequence());
        }
    }

    /// <summary>
    /// 通常の左右移動（開始）
    /// </summary>
    protected virtual void Idle()
    {
        StartCoroutine(LeftRightMove(5f, normalSpeed));
    }

    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    protected IEnumerator ChasePlayer(float duration, float moveSpeed)
    {
        if (isLeft == 1)
            yield return StartCoroutine(MoveLeft(duration, moveSpeed));
        else
            yield return StartCoroutine(MoveRight(duration, moveSpeed));

        Idle(); // 終了後は通常移動に戻す
    }

    protected IEnumerator MoveRight(float duration, float moveSpeed)
    {
        isLeft = -1;
        float timer = 0f;
        while (timer < duration)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
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
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 左→右→左→…を繰り返す移動
    /// </summary>
    protected IEnumerator LeftRightMove(float duration, float moveSpeed)
    {
        while (true)
        {
            yield return StartCoroutine(MoveRight(duration, moveSpeed));
            yield return StartCoroutine(MoveLeft(duration, moveSpeed));
        }
    }
}
