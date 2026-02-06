using UnityEngine;

public class DashController : MonoBehaviour
{
    Stamina stamina;
    StaminaBar staminaBar;
    private float currentStamina;
    private float maxStamina;
    private float dashCost = 30f;
    [SerializeField] private float staminaRegenRate = 30f;
    Rigidbody rb;
    bool isDashing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (staminaBar == null)
        {
            staminaBar = FindFirstObjectByType<StaminaBar>();
        }
        InitializeStamina();
    }

    private void InitializeStamina()
    {
        stamina = new Stamina();
        currentStamina = stamina.GetCurrentStamina();
        maxStamina = stamina.GetMaxStamina();
        staminaBar.UpdateStaminaUI(currentStamina, maxStamina);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isDashing = true;
            Dash();
        }
        RegenerateStamina();
        staminaBar.UpdateStaminaUI(currentStamina, maxStamina);
    }
    public void Dash()
    {
        if (currentStamina >= dashCost)
        {
            isDashing = true;
            currentStamina -= dashCost;
            staminaBar.UpdateStaminaUI(currentStamina, maxStamina);
            rb.AddForce(new Vector3(0, 0, 10f), ForceMode.Impulse);
            currentStamina -= dashCost;
        }
        isDashing = false;
    }

    void RegenerateStamina()
    {
        if (currentStamina < maxStamina && !isDashing)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }
}
