using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float gravity = -20f;
    public float jumpHeight = 0.8f;

    // Drag the object that has CameraOrbitFollow on it into this field (usually Main Camera or CameraRig)
    public CameraOrbitFollow cameraRig;

    public Transform cameraTransform;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector2 moveInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Camera-relative movement (Zelda style)
Vector3 camForward = cameraTransform.forward;
Vector3 camRight = cameraTransform.right;

// Flatten so looking up/down doesn't affect movement speed
camForward.y = 0f;
camRight.y = 0f;
camForward.Normalize();
camRight.Normalize();

Vector3 move = (camRight * moveInput.x + camForward * moveInput.y).normalized;
controller.Move(move * moveSpeed * Time.deltaTime);
        // Keep grounded so gravity doesn't accumulate weirdly
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Called automatically by Player Input (Send Messages) from action "Move"
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Called automatically by Player Input (Send Messages) from action "Jump"
    public void OnJump(InputValue value)
    {
        if (!value.isPressed) return;

        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // Called automatically by Player Input (Send Messages) from action "Look"
    // We forward it to the camera script.
    public void OnLook(InputValue value)
    {
        if (cameraRig != null)
            cameraRig.OnLook(value);
    }
}
