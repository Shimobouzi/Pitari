using UnityEngine;
using UnityEngine.UI;

public class StaminaBarController : MonoBehaviour
{
    public Image staminaFillImage;
    public CanvasGroup staminaUI;

    private float maxStamina = 100f;
    private float currentStamina;

    void Start()
    {
        currentStamina = maxStamina;
        staminaUI.alpha = 0f;
    }

    void Update()
    {
        bool isDashing = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f;

        if (isDashing)
            currentStamina -= 20f * Time.deltaTime;
        else
            currentStamina += 15f * Time.deltaTime;

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        staminaFillImage.fillAmount = currentStamina / maxStamina;
        staminaUI.alpha = isDashing || currentStamina < maxStamina ? 1f : 0f;
    }
}
