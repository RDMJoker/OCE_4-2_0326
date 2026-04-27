using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float jumpForce = 5f;
    Rigidbody2D rb;
    Vector2 moveDirection;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext _context)
    {
        moveDirection = _context.ReadValue<Vector2>();
        Debug.Log(moveDirection);
    }

    public void Jump(InputAction.CallbackContext _context)
    {
        if (_context.phase != InputActionPhase.Started) return;
        var jumpDirection = new Vector2(moveDirection.x, Vector2.up.y);
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);
    }
}
