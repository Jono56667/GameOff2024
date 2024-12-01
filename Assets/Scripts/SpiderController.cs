using UnityEngine;

public class SpiderAI : MonoBehaviour
{
    public float moveSpeed = 5f;            // Speed of movement
    public float climbSpeed = 3f;           // Speed of climbing
    public float rotationSpeed = 10f;       // Speed of rotation alignment to the surface
    public float raycastDistance = 1.5f;    // Distance to check for surfaces
    public LayerMask surfaceLayer;          // Layer for walls, floors, and ceilings
    public float gravityForce = 9.81f;      // Custom gravity strength
    public float groundCheckDistance = 0.5f; // Distance to check for ground
    public float maxDistanceFromSurface = 2f; // Maximum distance allowed from the surface (to prevent floating away)

    // Raycast offsets from the spider’s body
    public Vector3 frontOffset = new Vector3(0f, 0f, 0.5f);    // Raycast front position offset
    public Vector3 backOffset = new Vector3(0f, 0f, -0.5f);    // Raycast back position offset
    public Vector3 leftOffset = new Vector3(-0.5f, 0f, 0f);    // Raycast left position offset
    public Vector3 rightOffset = new Vector3(0.5f, 0f, 0f);    // Raycast right position offset

    private Vector3 velocity;               // The velocity of the spider (used for movement)
    private Vector3 lastSurfaceNormal;      // The surface normal of the surface the spider is on
    private bool isGrounded;                // Check if the spider is grounded

    private Vector3 targetDirection;        // The target direction the spider is moving towards
    private float moveTime = 0f;            // Timer for how long the spider moves in one direction
    private float moveDuration;             // Random duration to move in the current direction

    private bool isFleeing = false;         // Flag to check if the spider is in flee mode
    private float fleeTime = 0f;            // Timer for how long the spider should flee
    private float maxFleeDuration = 5f;     // Max duration for flee behavior

    private GameObject fleeSource;          // The object the spider is fleeing from

    private void Start()
    {
        lastSurfaceNormal = Vector3.up; // Default to ground normal at the start
        SetNewMovementDirection();      // Start with a random direction
    }

    private void Update()
    {
        // Check if the spider is grounded or on a surface
        isGrounded = IsGrounded();

        // Detect surfaces and align to them using raycasts from multiple points
        RaycastForSurface();

        // Apply manual gravity if not grounded
        if (!isGrounded)
        {
            ApplyManualGravity();
        }

        // Flee behavior if the spider is fleeing
        if (isFleeing)
        {
            Flee();
        }
        else
        {
            // Regular movement if not fleeing
            MoveAndClimb();

            // Update move timer for regular movement
            moveTime += Time.deltaTime;

            // If the spider has moved for the duration, pick a new random direction
            if (moveTime >= moveDuration)
            {
                SetNewMovementDirection(); // Get a new random direction
                moveTime = 0f;             // Reset the timer
            }
        }

        // Ensure the spider stays within the max distance from the surface
        EnforceMaxDistanceFromSurface();
    }

    private void MoveAndClimb()
    {
        // Only apply climbing behavior if the spider is on a surface
        if (lastSurfaceNormal != Vector3.zero)
        {
            // Determine the direction of movement based on the surface normal
            Vector3 moveDirection = Vector3.zero;

            // If it's on the ground, move forward (along the local forward axis)
            if (lastSurfaceNormal == Vector3.up)
            {
                moveDirection = transform.forward;
            }
            // If it's climbing on a wall or ceiling, move along the surface (parallel to the surface normal)
            else
            {
                // Project the movement direction to move along the surface (flatten the up direction)
                moveDirection = Vector3.Cross(transform.right, lastSurfaceNormal).normalized;
            }

            // Align the spider's rotation to match the direction of movement
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, lastSurfaceNormal);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Calculate the velocity for movement
            velocity = moveDirection * climbSpeed * Time.deltaTime;

            // Move the spider by directly modifying its position
            transform.position += velocity;
        }
    }

    private void SetNewMovementDirection()
    {
        // Set a random movement direction
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));

        // Set the new target direction for the spider's movement
        targetDirection = randomDirection;

        // Randomize the duration for which the spider will move in this direction
        moveDuration = Random.Range(2f, 5f); // Move for 2 to 5 seconds
    }

    private void Flee()
    {
        // Flee away from the fleeing source
        if (fleeSource != null)
        {
            // Calculate the flee direction (away from the fleeSource)
            Vector3 fleeDirection = (transform.position - fleeSource.transform.position).normalized;

            // Project the flee direction onto the surface plane to ensure it moves along the surface
            Vector3 surfaceDirection = Vector3.Cross(transform.right, lastSurfaceNormal).normalized;
            fleeDirection = Vector3.ProjectOnPlane(fleeDirection, lastSurfaceNormal).normalized;

            // Align the spider's rotation to the flee direction while respecting the surface normal
            Quaternion targetRotation = Quaternion.LookRotation(fleeDirection, lastSurfaceNormal);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Calculate the velocity for fleeing
            velocity = fleeDirection * climbSpeed * Time.deltaTime;

            // Move the spider by directly modifying its position
            transform.position += velocity;

            // Increase the flee timer
            fleeTime += Time.deltaTime;

            // If the flee time exceeds the maximum flee duration, stop fleeing and resume normal movement
            if (fleeTime >= maxFleeDuration)
            {
                isFleeing = false;
                fleeTime = 0f;
                SetNewMovementDirection(); // Resume regular movement
            }
        }
    }

    private void RaycastForSurface()
    {
        RaycastHit hit;

        // Perform 4 raycasts from different points and average the normals
        Vector3[] raycastOrigins = new Vector3[]
        {
            transform.position + frontOffset, // Front ray
            transform.position + backOffset,  // Back ray
            transform.position + leftOffset,  // Left ray
            transform.position + rightOffset  // Right ray
        };

        Vector3 averageNormal = Vector3.zero;
        Vector3 hitPosition = transform.position; // Default to the spider's current position
        int hitCount = 0;

        // Cast rays from all 4 points
        foreach (var origin in raycastOrigins)
        {
            if (Physics.Raycast(origin, -transform.up, out hit, raycastDistance, surfaceLayer))
            {
                averageNormal += hit.normal;
                hitPosition = hit.point; // Update the hit position
                hitCount++;
            }
        }

        // If we hit at least one surface, calculate the average normal
        if (hitCount > 0)
        {
            averageNormal /= hitCount;
            lastSurfaceNormal = averageNormal.normalized;

            // Snap the spider to the surface by adjusting its position
            transform.position = hitPosition;
        }
        else
        {
            lastSurfaceNormal = Vector3.up; // Default to the ground normal if no surfaces were hit
        }

        // Align the spider to the average surface normal
        if (lastSurfaceNormal != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, lastSurfaceNormal);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private bool IsGrounded()
    {
        // Perform a ground check using a raycast slightly below the spider
        RaycastHit hit;
        return Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, surfaceLayer);
    }

    private void ApplyManualGravity()
    {
        // Apply gravity only when the spider is not grounded
        if (!isGrounded)
        {
            // Apply a downward force as gravity
            velocity += Vector3.down * gravityForce * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }
    }

    private void EnforceMaxDistanceFromSurface()
    {
        // Ensure the spider doesn't move too far from the surface
        float distanceFromSurface = Vector3.Dot(transform.position - transform.position, lastSurfaceNormal);

        if (distanceFromSurface > maxDistanceFromSurface)
        {
            transform.position -= (distanceFromSurface - maxDistanceFromSurface) * lastSurfaceNormal;
        }
    }

    // Trigger detection for fleeing
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpiderFlee"))
        {
            isFleeing = true;
            fleeSource = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SpiderFlee"))
        {
            isFleeing = false;
            fleeSource = null;
            SetNewMovementDirection();
        }
    }
}
