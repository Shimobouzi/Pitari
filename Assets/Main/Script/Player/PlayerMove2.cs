using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMove2 : MonoBehaviour
{

    // 加速度のしきい値（この値を超えたら「振った」とみなす）
    //個々の値を大きくすればするほど強く降らないと反応しない仕組み
    [SerializeField]
    private float accelThreshold = 2.5f;

    // 移動スピード
    //個々の値を大きくすればするほど移動速度が早くなる仕組み
    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float speedMultiplier = 2f; //振る強さにかかる速度倍率

    [SerializeField]
    private float duration = 0.2f; // 移動時間

    [SerializeField]
    private float hideDelay = 0.2f; //隠れるラグ

    [SerializeField]
    private float boostDuration = 5f; //移動速度が早くなる持続時間（秒）


    //初めて移動したらfalse
    private bool isFirst = true;

    // プレイヤーが移動中かどうかのフラグ
    private bool isMoving = false;

    //プレイヤーが隠れているかのフラグ
    private static bool isHiding = false;

    //元の速度（初期移動速度)と値は同じ
    private float originalMoveSpeed;
    //実際の移動速度（振った強さで変動）これも（初期移動速度)と値は同じ
    private float dynamicSpeed;

    /*Editor内格納*/
    [SerializeField]
    private GameObject People;
    [SerializeField]
    private GameObject Object;
    [SerializeField]
    private GameObject Effect;



    private void Start()
    {
        originalMoveSpeed = moveSpeed;
        dynamicSpeed = moveSpeed;

        People.SetActive(true);
        Object.SetActive(false);
        Effect.SetActive(false);
    }

    void Update()
    {
        Vector3 accel = JoyconUpdate();
        float accelPower = accel.magnitude;

        // 一定の加速度以上で「振った」と判定
        if (accel.magnitude > accelThreshold && !isMoving)
        {
            //加速度の強さに応じて速度を調整
            float strengthMultiplier = Mathf.Clamp((accelPower - accelThreshold) * speedMultiplier,  1f, 4f);//速度倍率を計算//最小1倍　最大4倍
            dynamicSpeed = originalMoveSpeed * strengthMultiplier; //実際に適用する速度を更新

            StartCoroutine(ResetSpeedAfterDelay(boostDuration)); //5秒後に元の速度へ戻す

            if (isHiding) //モノから人に化ける処理
            {
                StartCoroutine(DontHidePlayer());
            }
            StartCoroutine(MovePlayer());
            isFirst = false;
        }
        else if (!isFirst && !isHiding && !isMoving) //モノに化ける処理
        {
            StartCoroutine(HidePlayer());
        }
    }

    /// <summary>
    /// 一定時間プレイヤーを前進させる
    /// </summary>
    IEnumerator MovePlayer()
    {
        isMoving = true;
        float timer = 0f;

        while (timer < duration)
        {
            transform.Translate(Vector3.right * dynamicSpeed * Time.deltaTime); // 2Dなら右方向に移動
            //一秒間に指定した数値分右に動く処理
            timer += Time.deltaTime;
            yield return null;
        }

        dynamicSpeed = originalMoveSpeed;
        isMoving = false;
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
    /// 一定時間経過後に動的スピードをリセットする
    /// </summary>
    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 指定された秒数だけ待機
        dynamicSpeed = originalMoveSpeed;       // スピードを元に戻す
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

    /// <summary>
    /// 人が敵に当たるとコントローラー振動処理
    /// </summary>
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

    public static bool GetisHiding()
    {
        return isHiding;
    }

    private Vector3 JoyconUpdate()
    {
        // 現在の加速度を取得
        return JoyconController.Instance.GetAccel();
    }


}
