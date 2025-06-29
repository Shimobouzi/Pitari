using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("スタミナ設定")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRecoveryRate = 15f;
    public float currentStamina;

    [Header("UI")]
    public Slider staminaSlider;
    public CanvasGroup staminaUI;

    [Header("ダッシュ設定")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public float dashSpeed = 10f;
    public float normalSpeed = 5f;

    private CharacterController controller;

    void Start()
    {
        currentStamina = maxStamina;
        controller = GetComponent<CharacterController>();
        staminaSlider.maxValue = maxStamina;
        staminaUI.alpha = 0f; // 初期は非表示
    }

    void Update()
    {
        bool isDashing = Input.GetKey(dashKey) && currentStamina > 0;

        // UIの表示切り替え
        staminaUI.alpha = isDashing || currentStamina < maxStamina ? 1f : 0f;

        // スタミナ処理
        if (isDashing)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            controller.Move(transform.forward * dashSpeed * Time.deltaTime);
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            controller.Move(transform.forward * normalSpeed * Time.deltaTime);
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaSlider.value = currentStamina;
    }
}
