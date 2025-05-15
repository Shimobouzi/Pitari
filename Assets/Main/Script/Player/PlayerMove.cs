using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMove : MonoBehaviour
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

    private float duration = 0.2f; // 移動時間

    // プレイヤーが移動中かどうかのフラグ
    private bool isMoving = false;


    void Update()
    {
        Vector3 accel = JoyconUpdate();

        // 一定の加速度以上で「振った」と判定
        if (accel.magnitude > accelThreshold && !isMoving)
        {
            StartCoroutine(MovePlayer());
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
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime); // 2Dなら右方向に移動
            //一秒間に指定した数値分右に動く処理
            timer += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
    }
    
	private void OnCollisionEnter2D(Collision2D collision)
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

    private Vector3 JoyconUpdate()
    {
        // 現在の加速度を取得
        return JoyconController.Instance.GetAccel();
    }


}
