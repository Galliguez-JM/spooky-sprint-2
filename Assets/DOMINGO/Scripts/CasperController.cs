using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // Added for scene checking

public class CasperController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;
    public float stopThreshold = 0.1f;

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public Transform cameraTransform;

    private Rigidbody rb;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        // --- NEW: LOAD POSITION LOGIC ---
        // If we are in the Town (Index 7) and have a saved position, teleport Casper
        if (SceneManager.GetActiveScene().buildIndex == 7 && PlayerPrefs.GetInt("HasSavedPos", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            float z = PlayerPrefs.GetFloat("PlayerZ");

            Vector3 savedPos = new Vector3(x, y, z);

            // Move both the transform and the physics body
            transform.position = savedPos;
            if (rb != null) rb.position = savedPos;

            Debug.Log("Casper returned to saved town position: " + savedPos);
        }
        // --------------------------------

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
