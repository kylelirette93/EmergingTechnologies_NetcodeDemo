using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class LocalPlayer : NetworkBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    Vector2 moveVector = Vector2.zero;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        transform.Translate(moveVector.x * Time.deltaTime * moveSpeed, 0, moveVector.y * Time.deltaTime * moveSpeed);
    }
}
