using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Image staminaBar;
    [SerializeField] private TextMeshProUGUI staminaText;

    public void UpdateStaminaUI(float currentStamina, float maxStamina)
    {
        if (staminaBar != null && staminaText != null)
        {
            staminaBar.fillAmount = currentStamina / maxStamina;
            staminaText.text = "Energy";

            float staminaPercentage = currentStamina / maxStamina;
            Color startColor = Color.red;
            Color endColor = Color.green;
            Color currentColor = Color.Lerp(startColor, endColor, staminaPercentage);

            staminaText.DOColor(currentColor, 0.2f);        
        }
    }
}
