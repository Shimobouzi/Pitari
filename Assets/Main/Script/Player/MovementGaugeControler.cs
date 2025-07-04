using UnityEngine;
using UnityEngine.UI;

public class MovementGaugeController : MonoBehaviour
{
    public Image gaugeFill;
    public PlayerMove3 player;

    [Header("加速度しきい値")]
    public float walkThreshold = 1.0f;
    public float dashThreshold = 2.5f;
    public float maxAccel = 4.0f;

    void Update()
    {
        Vector3 accel = player.GetAccel();
        float accelPower = accel.magnitude;

        // 停止状態ならゲージをゼロにする
        if (accelPower < walkThreshold)
        {
            gaugeFill.fillAmount = 0f;
            Debug.Log("停止状態");
            return;
        }

        // ゲージ更新（動いているときのみ）
        float normalized = Mathf.Clamp01(accelPower / maxAccel);
        gaugeFill.fillAmount = normalized;

        // 状態判定（歩き・ダッシュ）
        if (accelPower >= dashThreshold)
        {
            Debug.Log("ダッシュ状態");
        }
        else
        {
            Debug.Log("歩き状態");
        }
    }
}
