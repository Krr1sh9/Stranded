using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Base movement speed of the player when they are walking.")]
    public float walkSpeed = 5.0f;
    [Tooltip("Speed multiplier when the player is sprinting.")]
    public float sprintMultiplier = 1.8f;
    [Tooltip("Height the player can jump.")]
    public float jumpHeight = 5.0f;
    [Tooltip("Maximum horizontal speed (X and Z) the player can reach mid air. Must be greater than walkSpeed.")]
    public float maxAirSpeed = 7.5f;
    [Tooltip("Force multiplier for steering while mid air.")]
    public float airControlMultiplier = 5f;

    [Header("Look Settings")]
    [Tooltip("Sensitivity for horizontal and vertical mouse movement.")]
    public float mouseSensitivity = 2.0f;
    [Tooltip("The maximum angle (in degrees) the camera can look up or down.")]
    [Range(0f, 90f)]
    public float lookLimit = 85.0f;

    [Header("Head Bob Settings")]
    [Tooltip("Amplitude of the head bob motion.")]
    public float bobAmplitude = 0.1f;
    [Tooltip("Frequency of the head bob motion.")]
    public float bobFrequency = 10.0f;

    private CharacterController characterController;
    private Camera playerCamera;
    private float rotationX = 0;
    private Vector3 moveDirection;
    private float currentSpeed; // Stores the actual speed (walking or sprinting)
    private float gravity = 20.0f; // Default gravity force
    private Vector3 cameraOriginalLocalPosition; // Used for head bob calculation

    [Header("Crouch Settings")]
    [Tooltip("Speed multiplier when crouching.")]
    public float crouchSpeedMultiplier = 0.5f;
    [Tooltip("Target height while crouching.")]
    public float crouchHeight = 2.5f;
    private float originalHeight;
    private bool isCrouching = false;
    private float crouchSmoothSpeed = 10f;

    private bool isSwinging = false;
    private float swingTimer = 0f;
    private float swingDuration = 0.3f; // total time for the swing animation
    private float swingAmplitude = 15f; // how much the camera rotates on swing

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        if (playerCamera == null)
        {
            Debug.LogError("PlayerController requires a Camera child object to control view.");
        }

        // Lock cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Store the camera's starting position for head bob calculation
        cameraOriginalLocalPosition = playerCamera.transform.localPosition;

        // Store original height for the crouching system
        originalHeight = characterController.height;
    }

    void Update()
    {
        HandleCameraLook();
        HandleMovement();

        if (playerCamera != null)
        {
            HandleHeadBob();
        }

        HandleCrouch();

        // Escape key unlocks cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleCameraLook()
    {
        // Get mouse input values
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation on the player body
        transform.Rotate(Vector3.up * mouseX);

        // Vertical rotation on the camera
        rotationX -= mouseY;
        // Clamp the vertical rotation to prevent flipping the camera
        rotationX = Mathf.Clamp(rotationX, -lookLimit, lookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void HandleMovement()
    {
        // Store the current vertical velocity (needed for jumping and falling)
        float currentYVelocity = moveDirection.y;

        if (characterController.isGrounded)
        {
            // If we hit the ground, reset the downward Y velocity to a small negative value
            currentYVelocity = -0.5f;

            // Get input for horizontal and vertical movement
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Calculate current speed (walking or sprinting)
            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            currentSpeed = isSprinting ? walkSpeed * sprintMultiplier : walkSpeed;

            // Apply crouch speed reduction
            if (isCrouching)
            {
                currentSpeed *= crouchSpeedMultiplier;
            }

            // Calculate the desired movement vector relative to the player's rotation
            Vector3 forward = transform.forward * moveZ;
            Vector3 right = transform.right * moveX;

            // Recalculate horizontal direction based on input
            moveDirection = (forward + right).normalized * currentSpeed;

            // Handle Jumping
            if (Input.GetButton("Jump"))
            {
                // Calculate initial vertical velocity required to reach the jump height
                currentYVelocity = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
        }
        else
        {
            // Calculate air steering force based on player input
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            // Calculate force relative to the player's direction using airControlMultiplier
            Vector3 airControlForce = (transform.forward * moveZ + transform.right * moveX) * (walkSpeed * airControlMultiplier);

            // This is what allows for mid-air steering control.
            moveDirection.x += airControlForce.x * Time.deltaTime;
            moveDirection.z += airControlForce.z * Time.deltaTime;

            // Clamp Horizontal Speed in order to prevent infinite acceleration while falling
            Vector3 horizontalMovement = new Vector3(moveDirection.x, 0, moveDirection.z);

            if (horizontalMovement.magnitude > maxAirSpeed)
            {
                Vector3 clampedVelocity = horizontalMovement.normalized * maxAirSpeed;
                moveDirection.x = clampedVelocity.x;
                moveDirection.z = clampedVelocity.z;
            }
        }

        // Apply the stored Y velocity (currentYVelocity, which includes jump velocity or previous fall velocity)
        moveDirection.y = currentYVelocity;

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Execute the movement using the CharacterController
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleHeadBob()
    {
        // Calculate the magnitude of the horizontal movement
        float horizontalVelocity = new Vector2(characterController.velocity.x, characterController.velocity.z).magnitude;

        // Only head bob when the player is moving and grounded
        if (horizontalVelocity > 0.1f && characterController.isGrounded)
        {
            // Use a sine wave to create a smooth, repeating bobbing motion
            float waveSlice = Mathf.Sin(Time.time * bobFrequency * (currentSpeed / walkSpeed));

            float translateX = waveSlice * bobAmplitude * 0.5f; // Small horizontal shift
            float translateY = cameraOriginalLocalPosition.y + waveSlice * bobAmplitude; // Vertical shift

            playerCamera.transform.localPosition = new Vector3(
                cameraOriginalLocalPosition.x + translateX,
                translateY,
                cameraOriginalLocalPosition.z
            );
        }
        else
        {
            // Smoothly return the camera to its original position when stationary
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                cameraOriginalLocalPosition,
                Time.deltaTime * bobFrequency
            );
        }
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            // Only allow uncrouch if there is no obstruction above by using raycast
            RaycastHit hit;
            float headClearance = (originalHeight - crouchHeight) * 0.5f;
            Vector3 start = transform.position + Vector3.up * (characterController.height / 2f);
            bool blocked = Physics.Raycast(start, Vector3.up, out hit, headClearance + 0.1f);

            if (!blocked)
            {
                isCrouching = false;
            }
        }

        // Checks if the player is able to stand up and is not being obstructed
        if (isCrouching && !Input.GetKey(KeyCode.LeftControl))
        {
            // Check for obstructions above (same logic as GetKeyUp)
            RaycastHit hit;
            float headClearance = (originalHeight - crouchHeight) * 0.5f;
            Vector3 start = transform.position + Vector3.up * (characterController.height / 2f);
            bool blocked = Physics.Raycast(start, Vector3.up, out hit, headClearance + 0.1f);

            if (!blocked)
            {
                // If no longer blocked, automatically uncrouch
                isCrouching = false;
            }
        }

        float targetHeight = isCrouching ? crouchHeight : originalHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchSmoothSpeed);

        // Adjust camera height proportionally
        Vector3 camPos = playerCamera.transform.localPosition;
        float heightRatio = characterController.height / originalHeight;
        camPos.y = Mathf.Lerp(camPos.y, cameraOriginalLocalPosition.y * heightRatio, Time.deltaTime * crouchSmoothSpeed);
        playerCamera.transform.localPosition = camPos;
    }


    private void LateUpdate()
    {
        HandleSwordSwing();
    }

    private void HandleSwordSwing()
    {
        // Swing when left mouse button is pressed
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;
            swingTimer = 0f;
        }

        if (isSwinging && playerCamera != null)
        {
            swingTimer += Time.deltaTime;
            float progress = swingTimer / swingDuration;

            // Use a sine wave for smooth acceleration and deceleration
            float swingCurve = Mathf.Sin(progress * Mathf.PI);

            // Swing left and down
            float horizontalSwing = -swingCurve * swingAmplitude;
            float verticalSwing = swingCurve * (swingAmplitude * 0.5f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX + verticalSwing, horizontalSwing, 0);

            // End swing and reset camera position
            if (swingTimer >= swingDuration)
            {
                isSwinging = false;
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            }
        }
    }
}
