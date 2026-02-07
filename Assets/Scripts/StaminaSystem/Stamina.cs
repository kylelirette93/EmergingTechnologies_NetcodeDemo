using UnityEngine;

/// <summary>
/// Basic stamina class to hold stamina data and retrieve it.
/// </summary>
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
