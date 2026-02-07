using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DashController : NetworkBehaviour
{
    [SerializeField] private float staminaRegenRate = 30f;
    [SerializeField] private TrailRenderer dashTrail;

    Stamina stamina;
    StaminaBar staminaBar;
    Rigidbody rb;
    CollisionHandler collisionHandler;

    private float currentStamina;
    private float maxStamina;
    private float dashCost = 30f;
    private bool isDashing = false;

    [SerializeField] AudioClip dashSound;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        collisionHandler = GetComponent<CollisionHandler>();
        audioSource = GetComponentInChildren<AudioSource>();

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
            currentStamina -= dashCost;
            staminaBar.UpdateStaminaUI(currentStamina, maxStamina);
            DashServerRpc();
        }

        RegenerateStamina();
        staminaBar.UpdateStaminaUI(currentStamina, maxStamina);
    }

    [ServerRpc]
    public void DashServerRpc()
    {   
        PerformDashClientRpc();
    }

    [ClientRpc]
    public void PerformDashClientRpc()
    {
        EnableDashTrailClientRpc();
        audioSource.PlayOneShot(dashSound);
        isDashing = true;
        if (collisionHandler != null)
        {
            collisionHandler.isDashing = true;
        }

        rb.AddForce(transform.forward * 30f, ForceMode.Impulse);
        StartCoroutine(ResetDashFlag());
    }

    [ClientRpc]
    public void EnableDashTrailClientRpc()
    {
        dashTrail.gameObject.SetActive(true);
    }

    [ClientRpc]
    public void DisableDashTrailClientRpc()
    {
        dashTrail.Clear();
        dashTrail.gameObject.SetActive(false);
    }

    private IEnumerator ResetDashFlag()
    {
        // Let's the collision handler know we're dashing to apply knockback.
        yield return new WaitForSeconds(0.3f);
        isDashing = false;
        DisableDashTrailClientRpc();
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
