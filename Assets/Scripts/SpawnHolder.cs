using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Hold's reference to spawn points so spawn handler on player can grab them when spawned.
/// </summary>
public class SpawnHolder : NetworkBehaviour
{
    public Transform playerASpawn;
    public Transform playerBSpawn;

    public static SpawnHolder Instance { get; private set; }

    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        #endregion
    }
}
