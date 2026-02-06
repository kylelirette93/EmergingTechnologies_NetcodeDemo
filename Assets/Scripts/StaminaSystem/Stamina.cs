using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stamina
{
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;

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
