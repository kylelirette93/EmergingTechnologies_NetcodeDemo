using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mover : NetworkBehaviour
{
    private Vector2 inputVector;
    private Vector3 moveVector;
    private Rigidbody rb;
    float moveSpeed = 20f;
    float rotationSpeed = 120f;
    public bool CanMove { get; set; } = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue inputValue)
    {
        inputVector = inputValue.Get<Vector2>();
    }

    void Update()
    {
        if (!IsOwner) return;

        float forwardInput = Mathf.Clamp(inputVector.y, -1f, 1f);
        moveVector = transform.forward * forwardInput * moveSpeed;

        if (inputVector.x != 0)
        {
            float rotationAmount = inputVector.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner || !CanMove) return;

        rb.MovePosition(rb.position + moveVector * Time.fixedDeltaTime);
    }
}