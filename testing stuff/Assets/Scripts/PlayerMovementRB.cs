using UnityEngine;

public class PlayerMovementRB : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerView;
    public float playerViewYOffset = 0.6f;
    public float xMouseSensitivity = 30.0f;
    public float yMouseSensitivity = 30.0f;
    public Camera playerCamera;
    public float defaultFOV = 80f;
    public float sprintFOV = 100f;
    public float fovChangeSpeed = 8f;

    [Header("Movement Settings")]
    public float gravity = 9.81f;
    public float friction = 6f;
    public float baseMoveSpeed = 7.0f;
    public float maxMoveSpeed = 20.0f;
    public float sprintSpeed = 12.0f;
    public float acceleration = 14.0f;
    public float airAcceleration = 5.0f; // Stärkere Air Control
    public float maxAirControlSpeed = 15.0f;
    public float jumpForce = 8.0f;
    public bool holdJumpToBhop = true;

    private Rigidbody _rigidbody;
    private float rotX = 0.0f;
    private float rotY = 0.0f;
    private bool wishJump = false;
    private bool isGrounded = false;
    private float currentSpeed;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody nicht gefunden! Bitte Rigidbody hinzufügen.");
            return;
        }

        _rigidbody.freezeRotation = true;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerView = mainCamera.transform;
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        currentSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleSprintFOV();
        QueueJump();
    }

    private void FixedUpdate()
    {
        if (_rigidbody == null) return;

        CheckGroundStatus();

        if (isGrounded)
            GroundMove();
        else
            AirMove();
    }

    private void HandleMouseLook()
    {
        rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
        rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;
        rotX = Mathf.Clamp(rotX, -90, 90);

        transform.rotation = Quaternion.Euler(0, rotY, 0);
        playerView.rotation = Quaternion.Euler(rotX, rotY, 0);
        playerView.position = new Vector3(transform.position.x, transform.position.y + playerViewYOffset, transform.position.z);
    }

    private void HandleSprintFOV()
    {
        if (playerCamera == null) return;

        float targetFOV = (currentSpeed > baseMoveSpeed) ? sprintFOV : defaultFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovChangeSpeed * Time.deltaTime);
    }

    private void QueueJump()
    {
        if (holdJumpToBhop)
        {
            wishJump = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !wishJump)
            wishJump = true;
        if (Input.GetButtonUp("Jump"))
            wishJump = false;
    }

    private void CheckGroundStatus()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void GroundMove()
    {
        ApplyFriction();

        Vector3 moveDirection = GetMovementInput();
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : baseMoveSpeed;

        if (wishJump && currentSpeed < maxMoveSpeed)
            currentSpeed += 0.5f;
        else if (!wishJump)
            currentSpeed = targetSpeed;

        Vector3 velocity = moveDirection * currentSpeed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;

        if (wishJump)
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
            wishJump = false;
        }
    }

    private void AirMove()
    {
        Vector3 moveDirection = GetMovementInput();
        Vector3 airVelocity = _rigidbody.velocity;

        if (moveDirection.magnitude > 0)
        {
            Vector3 projectedVelocity = Vector3.ProjectOnPlane(airVelocity, Vector3.up);
            float currentAirSpeed = projectedVelocity.magnitude;

            if (currentAirSpeed < maxAirControlSpeed)
            {
                airVelocity += moveDirection * airAcceleration * Time.fixedDeltaTime;
            }
        }

        airVelocity.y -= gravity * Time.fixedDeltaTime;
        _rigidbody.velocity = airVelocity;
    }

    private Vector3 GetMovementInput()
    {
        float forwardMove = Input.GetAxisRaw("Vertical");
        float rightMove = Input.GetAxisRaw("Horizontal");

        Vector3 moveDirection = new Vector3(rightMove, 0, forwardMove);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.Normalize();

        return moveDirection;
    }

    private void ApplyFriction()
    {
        if (!isGrounded) return;

        Vector3 velocity = _rigidbody.velocity;
        float speed = velocity.magnitude;
        if (speed == 0) return;

        float drop = speed * friction * Time.fixedDeltaTime;
        float newSpeed = Mathf.Max(speed - drop, 0);

        _rigidbody.velocity = velocity.normalized * newSpeed;
    }
}
