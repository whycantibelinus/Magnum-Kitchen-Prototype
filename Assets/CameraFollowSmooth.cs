using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbitFollow : MonoBehaviour
{
    public Transform target;

    // Camera positioning
    public Vector3 offset = new Vector3(0f, 2f, -4f);

    public float followSmooth = 12f;

    // Rotation
    public float rotateSpeed = 120f;
    public float pitchSpeed = 80f;

    public float minPitch = 10f;
    public float maxPitch = 40f;

    private Vector2 lookInput;
    private float yaw;
    private float pitch;

    void Start()
    {
        if (target != null)
        {
            yaw = target.eulerAngles.y;
        }

        pitch = 25f; // starting tilt
    }

    void LateUpdate()
    {
        if (!target) return;

        // Apply input to yaw and pitch
        yaw += lookInput.x * rotateSpeed * Time.deltaTime;
        pitch -= lookInput.y * pitchSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Build rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Orbit position
        Vector3 desiredPosition = target.position + rotation * offset;

        // Smooth follow
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSmooth * Time.deltaTime
        );

        // Always look at player
        transform.LookAt(target.position + Vector3.up * 1.0f);
    }

    // Called from PlayerMove via Send Messages forwarding
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}