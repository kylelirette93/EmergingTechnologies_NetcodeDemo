using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using UnityEditor;

public class ClientAuthorityHandler : NetworkBehaviour
{
    public override void OnNetworkSpawn()  
    {
        Camera sceneCamera = GameObject.FindWithTag("SceneCamera").GetComponent<Camera>();
        if (sceneCamera != null)
        {
            sceneCamera.enabled = false;
        }
        Camera playerCamera = GetComponentInChildren<Camera>();
        AudioListener playerListener = GetComponentInChildren<AudioListener>();
        PlayerInput playerInput = GetComponent<PlayerInput>();

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
