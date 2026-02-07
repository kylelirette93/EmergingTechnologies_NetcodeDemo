using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class Relay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private Button JoinButton;
    [SerializeField] private Button HostButton;

    const int maxPlayerCount = 2;

    private async void Start()
    {
        try
        {
            // Initialize unity servics and sign in anonymously.
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");

        }
    }

    private void Update()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            // If max player count is reached hide all UI elements.
            int playerCount = NetworkManager.Singleton.ConnectedClients.Count;
            if (playerCount >= maxPlayerCount)
            {
                HideUI();
            }
        }
    }
    public async void StartHost()
    {
        try
        {
            // Creates space on relay server for 2 players.
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

            // Creates join code clients use to connect to relay server.
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            joinCodeText.text = $"Join Code: {joinCode}";

            // Encrypt connection to server and clients.
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

            // Start host and listen for clients.
            NetworkManager.Singleton.StartHost();

            if (joinCodeInput != null) joinCodeInput.gameObject.SetActive(false);
            if (JoinButton != null) JoinButton.gameObject.SetActive(false);
            if (HostButton != null) HostButton.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to start host: {e.Message}");
        }
    }

    public async void JoinGame(string joinCode)
    {
        try
        {
            if (string.IsNullOrEmpty(joinCode))
            {
                Debug.LogError("Join code empty.");
                return;
            }

            // Input field string is passed to relay server to find allocation to join.
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // Encrypt connection to server and clients.
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            // Start client and connect to relay server.
            NetworkManager.Singleton.StartClient();

            joinCodeText.text = $"Joining...";
            // Once joined, hide ui after delay.
            StartCoroutine(HideUIWithDelay(1f));
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to join game. {e.Message}");
        }
    }

    private IEnumerator HideUIWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        joinCodeText.gameObject.SetActive(false);
        joinCodeInput.gameObject.SetActive(false);
        menuUI.gameObject.SetActive(false);
    }

    private void HideUI()
    {
        if (joinCodeText != null) joinCodeText.gameObject.SetActive(false);
        if (joinCodeInput != null) joinCodeInput.gameObject.SetActive(false);
        if (menuUI != null) menuUI.gameObject.SetActive(false);
    }
}
