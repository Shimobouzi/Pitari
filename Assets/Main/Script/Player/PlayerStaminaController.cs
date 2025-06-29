using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaController : MonoBehaviour
{
    [Header("スタミナ設定")]
    [SerializeField] private float maxStamina = 100f;              // スタミナの最大値
    [SerializeField] private float staminaDrainRate = 20f;         // 移動中に毎秒減るスタミナ量
    [SerializeField] private float staminaRecoveryRate = 10f;      // 停止中に毎秒回復するスタミナ量
    [SerializeField] private float speedThreshold = 0f;            // スタミナを消費し始める速度のしきい値

    [Header("UI")]
    [SerializeField] private Image staminaBar;                     // スタミナゲージのImage（Fill型）
    [SerializeField] private CanvasGroup staminaUI;                // スタミナUIの表示・非表示制御用

    private float currentStamina;                                  // 現在のスタミナ量
    private PlayerMove2 playerMove;                                // プレイヤーの移動スクリプト参照

    void Start()
    {
        currentStamina = maxStamina;                               // スタミナを満タンで開始
        playerMove = GetComponent<PlayerMove2>();                  // 同じオブジェクトにあるPlayerMove2を取得

        if (staminaUI != null)
            staminaUI.alpha = 0f;                                  // 初期状態ではスタミナUIを非表示
    }

    void Update()
    {
        // Joy-Conの加速度を取得
        Vector3 accel = JoyconController.Instance.GetAccel();
        float speed = accel.magnitude;

        // 一定以上のスピードで移動しているか判定
        bool isMovingFast = speed > speedThreshold;

        // スタミナUIの表示切り替え（移動中またはスタミナが満タンでないときに表示）
        if (staminaUI != null)
            staminaUI.alpha = isMovingFast || currentStamina < maxStamina ? 1f : 0f;

        // スタミナの減少・回復処理
        if (isMovingFast)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;   // 移動中はスタミナを減らす
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime; // 停止中はスタミナを回復
        }

        // スタミナの値を制限（0〜最大値）
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // スタミナゲージUIの更新
        if (staminaBar != null)
            staminaBar.fillAmount = currentStamina / maxStamina;
    }
}
