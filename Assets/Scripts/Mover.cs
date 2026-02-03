using Unity.Netcode;
using UnityEngine;

public class Mover : NetworkBehaviour
{
    private Vector3 moveVector;
    private Rigidbody rb;
    float moveSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        float moveVectorX = Input.GetAxis("Horizontal");
        float moveVectorY = Input.GetAxis("Vertical");

        moveVector = new Vector3(moveVectorX * Time.fixedDeltaTime * moveSpeed, 0, moveVectorY * Time.fixedDeltaTime * moveSpeed);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + moveVector);
    }
}