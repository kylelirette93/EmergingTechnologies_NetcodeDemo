using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class Relay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI joinCodeText;
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private TextMeshProUGUI ipAddressText;
    [SerializeField] private TMP_InputField ipInputText;
    [SerializeField] private MenuUI menuUI;
    [SerializeField] private Button JoinButton;
    [SerializeField] private Button HostButton;
    [SerializeField] private Button HostLocalButton;
    [SerializeField] private Button JoinLocalButton;

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

    public void LocalHost()
    {
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.ServerListenAddress = GetLocalIPv4();
        ipAddressText.text = GetLocalIPv4();
        NetworkManager.Singleton.StartHost();
    }

    public void JoinLocal()
    {
        string ip = ipInputText.text;

        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.ServerListenAddress = ip;

        NetworkManager.Singleton.StartClient();
    }

    public string GetLocalIPv4()
    {
        // Get all network interfaces
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            // Filter for IPv4 addresses and ignore loopback (127.0.0.1)
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "No IPv4 address found.";
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
