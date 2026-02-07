using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class DashController : NetworkBehaviour
{
    Stamina stamina;
    StaminaBar staminaBar;
    private float currentStamina;
    private float maxStamina;
    private float dashCost = 30f;
    [SerializeField] private float staminaRegenRate = 30f;
    Rigidbody rb;
    bool isDashing = false;
    CollisionHandler collisionHandler;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collisionHandler = GetComponent<CollisionHandler>();
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
        if (!IsOwner) return;
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
        //UnityEngine.Debug.Log($"[{(IsServer ? "HOST" : "CLIENT")}] Dash called. Current stamina: {currentStamina}");
        if (currentStamina >= dashCost)
        {
            isDashing = true;
            if (collisionHandler != null)
            {
                collisionHandler.isDashing = true;
            }
            currentStamina -= dashCost;
            staminaBar.UpdateStaminaUI(currentStamina, maxStamina);
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            StartCoroutine(ResetDashFlag());
        }     
    }

    private IEnumerator ResetDashFlag()
    {
        yield return new WaitForSeconds(0.3f);
        isDashing = false;
        if (collisionHandler != null)
        {
            collisionHandler.isDashing = false;
        }
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
