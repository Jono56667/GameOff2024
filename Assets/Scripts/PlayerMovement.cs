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

    // Footstep sound variables
    public AudioSource footstepAudioSource;
    public AudioClip[] dirtFootsteps;  // Footstep sounds for dirt
    public AudioClip[] woodFootsteps;  // Footstep sounds for wood
    private float footstepTimer = 0f;   // Timer to control the footstep sound frequency
    public float footstepInterval = 0.5f; // Time interval between footsteps

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

        // Ensure the footstepAudioSource is set
        if (footstepAudioSource == null)
        {
            footstepAudioSource = GetComponent<AudioSource>();
        }
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

            // Play footstep sounds when moving
            if (isGrounded && (moveX != 0 || moveZ != 0)) // Only play when moving
            {
                footstepTimer -= Time.deltaTime;

                // If the timer has passed, play a footstep sound
                if (footstepTimer <= 0f)
                {
                    PlayFootstepSound();
                    footstepTimer = footstepInterval; // Reset the timer
                }
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

    // Method to play a random footstep sound based on the surface material
    private void PlayFootstepSound()
    {
        if (footstepAudioSource != null)
        {
            // Perform a raycast to get the surface material
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
            {
                string surfaceName = "Default"; // Default value

                // Check for the material of the object hit by the raycast
                Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
                if (hitRenderer != null && hitRenderer.sharedMaterial != null)
                {
                    surfaceName = hitRenderer.sharedMaterial.name;
                }

                AudioClip[] selectedFootstepSounds = null;

                // Choose footstep sounds based on the material name
                if (surfaceName.Contains("Dirt") || surfaceName.Contains("Default"))
                {
                    selectedFootstepSounds = dirtFootsteps;
                }
                else if (surfaceName.Contains("Wooden") || surfaceName.Contains("ColourPallet"))
                {
                    selectedFootstepSounds = woodFootsteps;
                }
                else
                {
                    // Log unknown material name (optional)
                    Debug.Log("Unknown material: " + surfaceName, hit.collider);
                }

                // If we have footstep sounds, play one randomly
                if (selectedFootstepSounds != null && selectedFootstepSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, selectedFootstepSounds.Length);
                    footstepAudioSource.PlayOneShot(selectedFootstepSounds[randomIndex]);
                }
            }
        }
    }




}


