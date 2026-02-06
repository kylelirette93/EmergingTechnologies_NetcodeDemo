using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stamina
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;
    [SerializeField] private float staminaCost = 30f;
    [SerializeField] private float staminaRegenRate = 10f;

    public Stamina()
    {
        currentStamina = maxStamina;
    }
    public float GetCurrentStamina()
    {
        return currentStamina;
    }

    public float GetMaxStamina()
    {
        return maxStamina;
    }
}
