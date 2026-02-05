using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : NetworkBehaviour
{
    [SerializeField] private Button HostButton;
    private const string HOST_EXISTS_KEY = "HostExists";

    private void Start()
    {
        UpdateButtonStates();

        InvokeRepeating(nameof(UpdateButtonStates), 0.5f, 0.5f);
    }

    private void UpdateButtonStates()
    {
        bool hostExists = PlayerPrefs.GetInt(HOST_EXISTS_KEY, 0) == 1;
        HostButton.interactable = !hostExists;
    }
    public void HostGame()
    {
        PlayerPrefs.SetInt(HOST_EXISTS_KEY, 1);
        PlayerPrefs.Save();

        NetworkManager.Singleton.StartHost();
        UpdateButtonStates();
        HideUI();
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        HideUI();
    }

    private void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(HOST_EXISTS_KEY);
        PlayerPrefs.Save();
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public override void OnDestroy()
    {
        ClearPlayerPrefs();
    }

    void OnApplicationQuit()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost)
        {
            ClearPlayerPrefs();
        }
    }
}
