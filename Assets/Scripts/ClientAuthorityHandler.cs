using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class ClientAuthorityHandler : NetworkBehaviour
{
    PlayerInput playerInput;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
    }
    public override void OnNetworkSpawn()  
    {
        Camera sceneCamera = GameObject.FindWithTag("SceneCamera").GetComponent<Camera>();
        if (sceneCamera != null)
        {
            sceneCamera.enabled = false;
        }
        Camera playerCamera = GetComponentInChildren<Camera>();
        AudioListener playerListener = GetComponentInChildren<AudioListener>();

        if (IsOwner)
        {
            playerCamera.enabled = true;
            playerListener.enabled = true;
            playerInput.enabled = true;
        }
        else
        {
            playerCamera.enabled = false;
            playerListener.enabled = false;
            playerInput.enabled = false;
        }
    }
}
