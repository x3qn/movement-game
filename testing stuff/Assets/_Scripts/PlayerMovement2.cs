using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPMPlayer : MonoBehaviour
{
    public Transform playerView;
    public float playerViewYOffset = 0.6f;
    public float xMouseSensitivity = 30.0f;
    public float yMouseSensitivity = 30.0f;

    [Header("Camera Settings")]
    public Camera playerCamera; // Die Kamera für das FOV
    public float defaultFOV = 80f;
    public float sprintFOV = 100f;
    public float fovChangeSpeed = 8f;

    [Header("Movement Settings")]
    public float gravity = 20.0f;
    public float friction = 6f;

    public float moveSpeed = 7.0f;
    public float sprintSpeed = 12.0f;
    public float crouchSpeed = 4.0f;
    public float slideSpeed = 14.0f;
    public float slideFriction = 0.2f;
    public float slideThreshold = 9.0f;
    public float runAcceleration = 14.0f;
    public float runDeacceleration = 10.0f;
    public float airAcceleration = 2.0f;
    public float airDecceleration = 2.0f;
    public float airControl = 0.5f;
    public float sideStrafeAcceleration = 50.0f;
    public float sideStrafeSpeed = 1.0f;
    public float jumpSpeed = 8.0f;
    public bool holdJumpToBhop = false;

    private CharacterController _controller;
    private float rotX = 0.0f;
    private float rotY = 0.0f;
    private Vector3 playerVelocity = Vector3.zero;
    private bool wishJump = false;
    private bool isCrouching = false;
    private bool isSliding = false;
    private bool isSprinting = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _controller = GetComponent<CharacterController>();

        if (playerView == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
                playerView = mainCamera.gameObject.transform;
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Maussteuerung
        rotX -= Input.GetAxisRaw("Mouse Y") * xMouseSensitivity * 0.02f;
        rotY += Input.GetAxisRaw("Mouse X") * yMouseSensitivity * 0.02f;
        rotX = Mathf.Clamp(rotX, -90, 90);

        transform.rotation = Quaternion.Euler(0, rotY, 0);
        playerView.rotation = Quaternion.Euler(rotX, rotY, 0);

        // Bewegung
        HandleCrouchAndSlide();
        HandleSprintFOV();
        QueueJump();

        if (_controller.isGrounded)
            GroundMove();
        else
            AirMove();

        _controller.Move(playerVelocity * Time.deltaTime);

        // Kamera anpassen
        playerView.position = new Vector3(transform.position.x, transform.position.y + playerViewYOffset, transform.position.z);
    }

    private void HandleSprintFOV()
    {
        if (playerCamera == null) return;

        float targetFOV = isSprinting ? sprintFOV : defaultFOV;
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

    private void HandleCrouchAndSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_controller.isGrounded && playerVelocity.magnitude > slideThreshold)
            {
                isSliding = true;
                isCrouching = false;
                playerVelocity += transform.forward * slideSpeed;
            }
            else
            {
                isCrouching = true;
                isSliding = false;
            }
            _controller.height = 1.0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (isSliding)
            {
                isSliding = false;
            }
            else
            {
                isCrouching = false;
                _controller.height = 2.0f;
            }
        }
    }

    private void AirMove()
    {
        Vector3 wishdir;
        float accel;

        float forwardMove = Input.GetAxisRaw("Vertical");
        float rightMove = Input.GetAxisRaw("Horizontal");

        wishdir = new Vector3(rightMove, 0, forwardMove);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();

        float currentMoveSpeed = moveSpeed;
        isSprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching && !_controller.isGrounded;
        if (isSprinting) currentMoveSpeed = sprintSpeed;
        if (isCrouching) currentMoveSpeed = crouchSpeed;

        float wishspeed = wishdir.magnitude * currentMoveSpeed;

        if (Vector3.Dot(playerVelocity, wishdir) < 0)
            accel = airDecceleration;
        else
            accel = airAcceleration;

        if (forwardMove == 0 && rightMove != 0)
        {
            if (wishspeed > sideStrafeSpeed)
                wishspeed = sideStrafeSpeed;
            accel = sideStrafeAcceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        playerVelocity.y -= gravity * Time.deltaTime;
    }

    private void GroundMove()
    {
        if (!wishJump)
            ApplyFriction(isSliding ? slideFriction : 1.0f);
        else
            ApplyFriction(0);

        Vector3 wishdir;
        float forwardMove = Input.GetAxisRaw("Vertical");
        float rightMove = Input.GetAxisRaw("Horizontal");

        wishdir = new Vector3(rightMove, 0, forwardMove);
        wishdir = transform.TransformDirection(wishdir);
        wishdir.Normalize();

        float currentMoveSpeed = moveSpeed;
        isSprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching && _controller.isGrounded;
        if (isSprinting) currentMoveSpeed = sprintSpeed;
        if (isCrouching) currentMoveSpeed = crouchSpeed;
        if (isSliding) currentMoveSpeed = slideSpeed;

        float wishspeed = wishdir.magnitude * currentMoveSpeed;

        Accelerate(wishdir, wishspeed, runAcceleration);
        playerVelocity.y = -gravity * Time.deltaTime;

        if (wishJump)
        {
            playerVelocity.y = jumpSpeed;
            wishJump = false;
        }
    }

    private void ApplyFriction(float t)
    {
        Vector3 vec = playerVelocity;
        float speed = vec.magnitude;
        float drop = 0.0f;

        if (_controller.isGrounded)
        {
            float control = (speed < runDeacceleration) ? runDeacceleration : speed;
            drop = control * friction * Time.deltaTime * t;
        }

        float newspeed = Mathf.Max(speed - drop, 0);
        if (speed > 0) newspeed /= speed;

        playerVelocity.x *= newspeed;
        playerVelocity.z *= newspeed;
    }

    private void Accelerate(Vector3 wishdir, float wishspeed, float accel)
    {
        float currentspeed = Vector3.Dot(playerVelocity, wishdir);
        float addspeed = wishspeed - currentspeed;
        if (addspeed <= 0)
            return;

        float accelspeed = Mathf.Min(accel * Time.deltaTime * wishspeed, addspeed);
        playerVelocity += accelspeed * wishdir;
    }
}