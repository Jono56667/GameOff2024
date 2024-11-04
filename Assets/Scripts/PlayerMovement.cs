using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundMask;

    private Rigidbody rb;
    public bool isGrounded;
    public bool UiEnabled;

    private void OnEnable()
    {
        GameEvents.OnEnableInput += ToggleInput;
    }
    private void OnDisable()
    {
        GameEvents.OnEnableInput -= ToggleInput;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent rotation from physics interactions
    }

    void Update()
    {
        if (!UiEnabled)
        {
            // Check if the player is grounded
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

            // Get input from player
            float moveX = Input.GetAxis("Horizontal"); // Left/right
            float moveZ = Input.GetAxis("Vertical");   // Forward/backward

            // Calculate movement direction based on the character's Y rotation
            Vector3 moveDirection = transform.TransformDirection(new Vector3(moveX, 0, moveZ)).normalized;

            // Move the character
            MoveCharacter(moveDirection);

            // Handle jumping
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                Jump();
            }

        }
    }

    private void MoveCharacter(Vector3 direction)
    {
        // Calculate the movement vector
        Vector3 move = direction * moveSpeed;

        // Apply the movement vector to the Rigidbody
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            // Apply jump force
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    public void ToggleInput(bool toggle)
    {
        if (toggle)
        {
            UiEnabled = false;
        }
        else
        {
            UiEnabled = true;
        }
    }

}
