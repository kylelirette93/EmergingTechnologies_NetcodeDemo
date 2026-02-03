using Unity.Netcode;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        HideUI();
    }

    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        HideUI();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        HideUI();
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
