using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Joy-Conを振ると前に進むプレイヤー用スクリプト
/// JoyconLibが必要（https://github.com/Looking-Glass/JoyconLib）
/// </summary>
public class ToriPlayer : MonoBehaviour
{
    // Joy-Conリスト（複数接続されたJoy-Conの情報）
    private List<Joycon> joycons;

    // 操作対象のJoy-Con（今回は右を使用）
    private Joycon joycon;

    // 加速度のしきい値（この値を超えたら「振った」とみなす）
    //個々の値を大きくすればするほど強く降らないと反応しない仕組み
    [SerializeField]
    private float accelThreshold = 2.5f;

    // 移動スピード
    //個々の値を大きくすればするほど移動速度が早くなる仕組み
    [SerializeField]
    private float moveSpeed = 3f;

    // プレイヤーが移動中かどうかのフラグ
    private bool isMoving = false;

    void Start()
    {
        // Joy-Conのリストを取得
        joycons = JoyconManager.Instance.j;

        // 1つ以上接続されていれば1番目を使う
        if (joycons.Count > 0)
        {
            joycon = joycons[0];
        }
        else
        {
            Debug.LogWarning("Joy-Conが接続されていません。");
        }
    }

    void Update()
    {
        // Joy-Conが取得できていない場合は何もしない
        if (joycon == null) return;

        // 現在の加速度を取得
        Vector3 accel = joycon.GetAccel();

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

        float duration = 1f; // 移動時間
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
			joycon.SetRumble(320, 640, 1.0f, 500);
			Debug.Log("敵に当たりました！");
		}
	}


}
