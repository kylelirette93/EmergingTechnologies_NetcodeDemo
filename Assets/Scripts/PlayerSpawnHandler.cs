using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Player Spawn Handler is responsible for spawning players at correct spawn points when they join the game.
/// </summary>
public class PlayerSpawnHandler : NetworkBehaviour
{
    Transform lastPickedSpawnPoint;
    int maxSpawns = 4;
    public override void OnNetworkSpawn()
    {
        if (!IsServer || NetworkManager.Singleton.ConnectedClients.Count < maxSpawns) return;
      
        bool isPlayerA = NetworkManager.Singleton.ConnectedClients.Count % 2 == 0;

        transform.position = isPlayerA ?
            SpawnHolder.Instance.spawnPointA.position :
            SpawnHolder.Instance.spawnPointB.position;

        transform.rotation = isPlayerA ?
            SpawnHolder.Instance.spawnPointA.rotation :
            SpawnHolder.Instance.spawnPointB.rotation;
    }

    private void Update()
    {
        if (!IsOwner && transform.position.y > 0f) return;
        if (transform.position.y < -6f) 
        {
            SpawnHolder.Instance.SwitchSpawnPoints();
            bool isPlayerA = NetworkManager.Singleton.ConnectedClients.Count % 2 == 0;

            transform.position = isPlayerA ?
                SpawnHolder.Instance.spawnPointA.position :
                SpawnHolder.Instance.spawnPointB.position;

            transform.rotation = isPlayerA ?
                SpawnHolder.Instance.spawnPointA.rotation :
                SpawnHolder.Instance.spawnPointB.rotation;
        }
    }
}
