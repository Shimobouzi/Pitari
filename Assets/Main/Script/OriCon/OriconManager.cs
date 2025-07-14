using UnityEngine;

public class OriconManager : MonoBehaviour
{
    public static OriconManager instance;
    [SerializeField]
    private float accelThreshold = 2.5f;

    [SerializeField]
    private float DashAT = 5f;

    /* 両手同時振り判定用変数 ------------------------- */
    bool leftSwing = false;
    bool rightSwing = false;
    float swingTimer = 0f;
    const float swingWindow = 0.3f;   // 両手がそろう許容時間（秒）
    /* -------------------------------------------------- */
    PicoSensorHub hub;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        hub = FindFirstObjectByType<PicoSensorHub>();
    }

    /// <summary>
    /// 塩ビコントローラーでの両手振り動作を検出し、条件を満たした場合にtrueを返す
    /// </summary>
    public bool pvcController()
    {

        if (hub == null) return false;

        Vector3 l = hub.leftAcc;
        Vector3 r = hub.rightAcc;

        // 加速度が閾値を超えたら振ったと判定
        if (l.magnitude > accelThreshold) leftSwing = true;
        if (r.magnitude > accelThreshold) rightSwing = true;

        // 両手が揃って振られた場合 → 成功（true）
        if (leftSwing && rightSwing)
        {
            leftSwing = rightSwing = false;
            swingTimer = 0f;
            return true;
        }
        // どちらの手も振られていない場合 → 失敗（false）
        else if (!leftSwing && !rightSwing)
        {
            return false;
        }
        else
        {
            // 片方だけ振られている状態 → タイマーを測定
            swingTimer += Time.deltaTime;

            if (swingTimer > swingWindow)
            {
                // もう片方が時間内に来なかった → フラグを無効化してリセット
                leftSwing = rightSwing = false;
                swingTimer = 0f;
            }

            // ※バグ修正ポイント：
            // 以前のコードでは、ここでreturn trueとしていたため
            // 「片手だけ振っただけでもtrueが返る」状態になっていた。
            // 結果として、前進アニメーションが意図せず発動していた。
            return false; // 両手が揃わなかったのでfalseを返す
        }

        // ※以前のコードではここでreturn trueしていたが、すべての分岐でreturnするため不要。
    }


    public bool pvcDash()
    {
        Debug.Log(accelThreshold);

        if (hub == null) return false;

        Vector3 l = hub.leftAcc;
        Vector3 r = hub.rightAcc;

        // 加速度が閾値を超えたら振ったと判定
        if (l.magnitude > DashAT) leftSwing = true;
        if (r.magnitude > DashAT) rightSwing = true;

        // 両手が揃って振られた場合 → 成功（true）
        if (leftSwing && rightSwing)
        {
            leftSwing = rightSwing = false;
            swingTimer = 0f;
            return true;
        }
        // どちらの手も振られていない場合 → 失敗（false）
        else if (!leftSwing && !rightSwing)
        {
            return false;
        }
        else
        {
            // 片方だけ振られている状態 → タイマーを測定
            swingTimer += Time.deltaTime;

            if (swingTimer > swingWindow)
            {
                // もう片方が時間内に来なかった → フラグを無効化してリセット
                leftSwing = rightSwing = false;
                swingTimer = 0f;
            }

            // ※バグ修正ポイント：
            // 以前のコードでは、ここでreturn trueとしていたため
            // 「片手だけ振っただけでもtrueが返る」状態になっていた。
            // 結果として、前進アニメーションが意図せず発動していた。
            return false; // 両手が揃わなかったのでfalseを返す
        }

        // ※以前のコードではここでreturn trueしていたが、すべての分岐でreturnするため不要。
    }

    public void GetPicoHub()
    {
        hub = FindFirstObjectByType<PicoSensorHub>();
    }

}


