using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private Button HostButton;
    [SerializeField] private Button JoinButton;
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] Relay relay;

    public void OnHostButtonClicked()
    {
        Debug.Log("Host button clicked.");
        relay.StartHost();
    }

    public void OnJoinButtonClicked()
    {
        relay.JoinGame(joinCodeInput.text);
    }

    public void DisableUI()
    {
        HostButton.enabled = false;
        JoinButton.enabled = false;
    }
}
