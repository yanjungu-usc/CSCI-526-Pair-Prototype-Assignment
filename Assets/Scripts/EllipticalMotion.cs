using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipticalMotion : MonoBehaviour
{
    public Transform center;            // The center point of the ellipse.
    public float initialSemiMajorAxis = 5f;  // Initial semi-major axis length.
    public float initialSemiMinorAxis = 3f;  // Initial semi-minor axis length.
    public float speed = 2f;            // Rotation speed (in radians per second).
    public float initialRadius = 5f;    // Initial radius of the circular constraint.
    public float axisIncrementRate = 0.1f; // Rate of increase for both axes (units per second).
    public float radiusIncrementRate = 0.1f; // Rate of increase for the radius (units per second).
    public float jumpForce = 10f;       // Initial jump force.
    public float jumpDuration = 0.2f;   // Duration of the jump.
    private float targetAngle = 0f;     // Target angle for smooth rotation.

    private float semiMajorAxis;
    private float semiMinorAxis;
    private float radius;
    private float angle = 0f;
    private float timeElapsed = 0f;
    private bool isJumping = false;
    private bool canJump = true;         // Flag to track if the object can jump.
    private Vector3 jumpStartPosition;
    private Rigidbody2D rb2D;

    private bool startEllipticalMotion = false; // Flag to track when to start elliptical motion.
    private float peakHeightY; // Y-coordinate of the peak height during jump.

    private bool isEnabled = true; // Flag to track if the script is enabled.
    private float disableTimer = 10f;
    void Start()
    {
        semiMajorAxis = initialSemiMajorAxis;
        semiMinorAxis = initialSemiMinorAxis;
        radius = initialRadius;
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.isKinematic = true; // Make the Rigidbody2D initially kinematic.
    }

    void Update()
    {
        if (isEnabled)
        {
            if (!isJumping && canJump)
            {
                // Check for the spacebar input to initiate the jump.
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
            }
            else if (isJumping)
            {
                // Increment the time elapsed during the jump.
                timeElapsed += Time.deltaTime;

                // Check if we should transition to elliptical motion.
                if (timeElapsed >= jumpDuration / 2 && !startEllipticalMotion)
                {
                    // Start elliptical motion at peak height of jump.
                    peakHeightY = transform.position.y; // Store the peak height Y-coordinate.
                    CalculateEllipticalAxes(); // Calculate the semi-major and semi-minor axes based on peak height.
                    SetupEllipticalMotion(); // Setup elliptical motion.
                }
                else if (timeElapsed >= jumpDuration)
                {
                    // End the jump.
                    isJumping = false;
                    rb2D.velocity = Vector2.zero;
                    rb2D.isKinematic = false; // Set Rigidbody2D to non-kinematic to enable physics.
                }
            }
        }


        disableTimer -= Time.deltaTime;
        if (disableTimer <= 0f)
        {
            isEnabled = false; // Disable the script.
            rb2D.velocity = Vector2.zero; // Stop the object's movement.
        }
    }

    void FixedUpdate()
    {
        // If not jumping or during elliptical motion, perform elliptical motion.
        if (!isJumping && startEllipticalMotion)
        {
            PerformEllipticalMotion();
        }
    }

    void Jump()
    {
        // Start the jump.
        isJumping = true;
        canJump = false; // Disable further jumps.
        timeElapsed = 0f;
        startEllipticalMotion = false; // Reset the startEllipticalMotion flag.
        jumpStartPosition = transform.position;

        // Apply an upward force for the jump.
        rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    void CalculateEllipticalAxes()
    {
        // Calculate the semi-major and semi-minor axes based on the peak height.
        semiMajorAxis = Mathf.Abs(peakHeightY - center.position.y);
        semiMinorAxis = semiMajorAxis * (initialSemiMinorAxis / initialSemiMajorAxis);
    }

    void SetupEllipticalMotion()
    {
        // Determine the initial angle based on the current position.
        Vector2 initialPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 centerPosition = new Vector2(center.position.x, center.position.y);
        Vector2 initialOffset = initialPosition - centerPosition;
        float initialAngle = Mathf.Atan2(initialOffset.y, initialOffset.x);

        // Set the initial angle and target angle for smooth transition.
        angle = initialAngle;
        targetAngle = angle + Mathf.PI; // Rotate by 180 degrees for a smoother transition.

        // Calculate the initial velocity to follow the elliptical path.
        Vector2 initialVelocity = new Vector2(-semiMajorAxis * Mathf.Sin(initialAngle), semiMinorAxis * Mathf.Cos(initialAngle));
        rb2D.velocity = initialVelocity.normalized * speed; // Set initial velocity.
    }

    void PerformEllipticalMotion()
    {
        // Smoothly interpolate the angle change.
        angle = Mathf.Lerp(angle, targetAngle, Time.fixedDeltaTime * speed);

        // Calculate the new position of the object along the ellipse.
        float x = center.position.x + semiMajorAxis * Mathf.Cos(angle);
        float y = center.position.y + semiMinorAxis * Mathf.Sin(angle);

        // Constrain the position to stay within the circular area.
        Vector2 newPosition = new Vector2(x, y);
        Vector2 centerPosition = new Vector2(center.position.x, center.position.y);
        Vector2 offset = newPosition - centerPosition;

        // Check if the object is outside the circular constraint.
        if (offset.magnitude > radius)
        {
            // If outside, normalize the offset and set it to the constrained position.
            offset.Normalize();
            offset *= radius;
            newPosition = centerPosition + offset;
        }

        // Set the object's position.
        rb2D.MovePosition(newPosition);

        // Update the target angle for the next frame.
        targetAngle += speed * Time.fixedDeltaTime;

        // Ensure the target angle stays within the range of a full ellipse.
        if (targetAngle > angle + 2 * Mathf.PI)
        {
            targetAngle = angle;
        }
    }
}

