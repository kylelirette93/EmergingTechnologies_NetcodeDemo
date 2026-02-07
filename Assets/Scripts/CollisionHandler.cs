using DG.Tweening;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class CollisionHandler : NetworkBehaviour
{
    float knockbackForce = 15f;
    float knockbackDuration = 0.3f;
    public bool isDashing = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;

        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            ulong networkObjectId = collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            // Send a server rpc to handle physics on server for all clients.
            ApplyKnockbackForceServerRpc(networkObjectId);
        }
    }

    [ServerRpc]
    private void ApplyKnockbackForceServerRpc(ulong networkObjectId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject targetObject))
        {
            Vector3 knockbackDirection = targetObject.transform.position - transform.position;
            knockbackDirection.Normalize();

            // Disabling movement to prevent overriding the knockback force.
            targetObject.GetComponent<CollisionHandler>().DisableMovementClientRpc(knockbackDuration);
            targetObject.GetComponent<CollisionHandler>().ApplyKnockbackClientRpc(knockbackDirection, knockbackForce, knockbackDuration);
        }
    }

    [ClientRpc]
    private void DisableMovementClientRpc(float duration)
    {
        // Disables movement specifically on this client.
        StartCoroutine(DisableMovementCoroutine(duration));
    }

    [ClientRpc] 
    private void ApplyKnockbackClientRpc(Vector3 direction, float force, float duration)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Handles knockback force on client side.
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
        StartCoroutine(DisableMovementCoroutine(duration));
    }

    private IEnumerator DisableMovementCoroutine(float duration)
    {
        Mover mover = GetComponent<Mover>();
        if (mover != null)
        {
            // Feedback for knockback, and resets movement after dash duration.
            transform.DOScale(Vector3.one * 1.2f, duration / 2).SetEase(Ease.OutQuad);
            mover.CanMove = false;
            yield return new WaitForSeconds(duration);
            mover.CanMove = true;
            transform.DOScale(Vector3.one, duration / 2).SetEase(Ease.InQuad);
        }
    }
}
