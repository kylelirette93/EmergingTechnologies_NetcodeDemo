using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mover : NetworkBehaviour
{
    private Vector3 moveVector;
    private Rigidbody rb;
    float moveSpeed = 60f;
    float rotationSpeed = 120f;

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

        float forwardInput = Mathf.Clamp01(moveVectorY);
        moveVector = new Vector3(0, 0, forwardInput * Time.deltaTime * moveSpeed);

        if (moveVectorX != 0)
        {
            float rotationAmount = moveVectorX * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
    }

    private void FixedUpdate()
    {
        moveVector = Mathf.Clamp(moveVector.magnitude, 0, moveVector.magnitude) * transform.forward;
        rb.MovePosition(transform.position + moveVector);
    }
}