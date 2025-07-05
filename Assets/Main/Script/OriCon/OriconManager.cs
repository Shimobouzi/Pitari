using UnityEngine;

public class OriconManager : MonoBehaviour
{
    public static OriconManager instance;
    [SerializeField]
    private float accelThreshold = 2.5f;

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

    public bool pvcController()
    {
        /* hub が取得できていない場合は何もしない */
        if (hub == null) return false;

        // 塩ビコントローラー：左右それぞれの加速度
        Vector3 l = hub.leftAcc;
        Vector3 r = hub.rightAcc;

        /* ---- 左右どちらかがしきい値超えたらフラグ ---- */
        if (l.magnitude > accelThreshold) leftSwing = true;
        if (r.magnitude > accelThreshold) rightSwing = true;

        /* ---- 両方そろえば前進 ---- */
        if (leftSwing && rightSwing)
        {
            return true;
            /* フラグとタイマーをリセット 
            leftSwing = rightSwing = false;
            swingTimer = 0f;*/
        }
        else
        {
            /* 片手だけ振られている状態でタイマー測定 */
            if (leftSwing || rightSwing)
            {
                swingTimer += Time.deltaTime;
                if (swingTimer > swingWindow)
                {
                    /* 規定時間内にもう片方が来なかった → フラグ無効化 */
                    leftSwing = rightSwing = false;
                    swingTimer = 0f;
                }
            }
        }

        return false;

        
    }
}


