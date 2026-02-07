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

        //Debug.Log($"[{(IsServer ? "HOST" : "CLIENT")}] Collision detected. isDashing {isDashing}, collided with: {collision.gameObject.name}");
        if (collision.gameObject.CompareTag("Player") && isDashing)
        {
            //Debug.Log($"[{(IsServer ? "HOST" : "CLIENT")}] Sending knockback ServerRPC");
            ulong networkObjectId = collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId;
            ApplyKnockbackForceServerRpc(networkObjectId);
        }
    }

    [ServerRpc]
    private void ApplyKnockbackForceServerRpc(ulong networkObjectId)
    {
        //Debug.Log($"[SERVER] ApplyKnockbackForceServerRpc called for object ID: {networkObjectId}");
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject targetObject))
        {
            Vector3 knockbackDirection = targetObject.transform.position - transform.position;
            knockbackDirection.Normalize();

            targetObject.GetComponent<CollisionHandler>().DisableMovementClientRpc(knockbackDuration);
            targetObject.GetComponent<CollisionHandler>().ApplyKnockbackClientRpc(knockbackDirection, knockbackForce, knockbackDuration);
        }
    }

    [ClientRpc]
    private void DisableMovementClientRpc(float duration)
    {
        StartCoroutine(DisableMovementCoroutine(duration));
    }

    [ClientRpc] 
    private void ApplyKnockbackClientRpc(Vector3 direction, float force, float duration)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode.Impulse);
        }
        StartCoroutine(DisableMovementCoroutine(duration));
    }

    private IEnumerator DisableMovementCoroutine(float duration)
    {
        Mover mover = GetComponent<Mover>();
        if (mover != null)
        {
            transform.DOScale(Vector3.one * 1.2f, duration / 2).SetEase(Ease.OutQuad);
            mover.CanMove = false;
            yield return new WaitForSeconds(duration);
            mover.CanMove = true;
            transform.DOScale(Vector3.one, duration / 2).SetEase(Ease.InQuad);
        }
    }
}
