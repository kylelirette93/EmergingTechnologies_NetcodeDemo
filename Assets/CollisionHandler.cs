using Unity.Netcode;
using UnityEngine;

public class CollisionHandler : NetworkBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(new Vector3(0, 0, 20f), ForceMode.Impulse);
        }
    }
}
