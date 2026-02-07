using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Player Spawn Handler is responsible for spawning players at correct spawn points when they join the game.
/// </summary>
public class PlayerSpawnHandler : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        bool isPlayerA = NetworkManager.Singleton.ConnectedClients.Count % 2 == 0;

        transform.position = isPlayerA ?
            SpawnHolder.Instance.playerASpawn.position :
            SpawnHolder.Instance.playerBSpawn.position;

        transform.rotation = isPlayerA ?
            SpawnHolder.Instance.playerASpawn.rotation :
            SpawnHolder.Instance.playerBSpawn.rotation;
    }
}
