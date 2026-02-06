using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    Stamina stamina;
    [SerializeField] private TextMeshProUGUI staminaText;

    private void Start()
    {
        stamina = new Stamina();
        staminaBar.fillAmount = stamina.GetCurrentStamina() / stamina.GetMaxStamina();
    }

    private void UpdateStaminaUI()
    {
        if (staminaBar != null && staminaText != null)
        {
            staminaBar.fillAmount = stamina.GetCurrentStamina() / stamina.GetMaxStamina();
            staminaText.text = "Energy";

            float staminaPercentage = stamina.GetCurrentStamina() / stamina.GetMaxStamina();
            Color startColor = Color.red;
            Color endColor = Color.green;
            Color currentColor = Color.Lerp(startColor, endColor, staminaPercentage);

            
        }
    }
}
