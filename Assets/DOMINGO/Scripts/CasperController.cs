using UnityEngine;
using UnityEngine.InputSystem;

public class CasperController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float stopThreshold = 0.1f; // Helps stop "drifting"

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        // IMPORTANT: Set Interpolation to "Interpolate" in the Inspector Rigidbody settings!
        if (rb == null) Debug.LogWarning("CasperController needs a Rigidbody.");
    }

    private void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseDelta.x);

        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void FixedUpdate()
    {
        float moveX = 0;
        float moveZ = 0;

        if (Keyboard.current.wKey.isPressed) moveZ = 1f;
        if (Keyboard.current.sKey.isPressed) moveZ = -1f;
        if (Keyboard.current.aKey.isPressed) moveX = -1f;
        if (Keyboard.current.dKey.isPressed) moveX = 1f;

        Vector3 moveInput = (transform.forward * moveZ + transform.right * moveX).normalized;

        if (moveInput.magnitude > stopThreshold)
        {
            // Move using velocity so physics (gravity/walls) works naturally
            Vector3 targetVelocity = moveInput * speed;
            targetVelocity.y = rb.linearVelocity.y; // Keep current gravity/falling speed
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            // Force a full stop on X/Z so you don't slide like soap in corners
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
}
