using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CasperController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float stopThreshold = 0.1f;
    public bool canLook = true;

    [Header("Ghost Floating Settings")]
    public float stepHeight = 0.4f;   // How high Casper can "float" over a ledge
    public float floatPower = 4.0f;  // The strength of the upward nudge

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        if (SceneManager.GetActiveScene().buildIndex == 7 && PlayerPrefs.GetInt("HasSavedPos", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");
            Vector3 savedPos = new Vector3(x, y, z);

            RigidbodyInterpolation oldInterpolation = rb.interpolation;
            rb.interpolation = RigidbodyInterpolation.None;

            transform.position = savedPos;
            rb.position = savedPos;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.interpolation = oldInterpolation;
            Debug.Log("Casper successfully teleported to: " + savedPos);
        }

        if (rb == null) Debug.LogWarning("CasperController needs a Rigidbody.");
    }

    private void Update()
    {
        if (!canLook) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseDelta.x);

        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void FixedUpdate()
    {
        if (!canLook)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        // --- NEW: LEDGE BYPASS / GHOST FLOAT ---
        // Shoot a raycast forward at feet level to see if we hit a small ledge
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, out hit, 0.6f))
        {
            // If we hit a ledge, check if there is empty space at Casper's 'knee' level
            if (!Physics.Raycast(transform.position + Vector3.up * stepHeight, transform.forward, 0.7f))
            {
                // Lift Casper up slightly to glide over the ledge
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, floatPower, rb.linearVelocity.z);
            }
        }
        // --------------------------------------

        float moveX = 0;
        float moveZ = 0;

        if (Keyboard.current.wKey.isPressed) moveZ = 1f;
        if (Keyboard.current.sKey.isPressed) moveZ = -1f;
        if (Keyboard.current.aKey.isPressed) moveX = -1f;
        if (Keyboard.current.dKey.isPressed) moveX = 1f;

        Vector3 moveInput = (transform.forward * moveZ + transform.right * moveX).normalized;

        if (moveInput.magnitude > stopThreshold)
        {
            Vector3 targetVelocity = moveInput * speed;
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = targetVelocity;
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }
}
