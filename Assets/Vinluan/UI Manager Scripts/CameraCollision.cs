using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Camera Setup")]
    public Transform pivot;          // Drag your "CameraPivot" object here
    public Transform playerBody;     // Drag your Player object here

    [Header("Collision Settings")]
    public float maxDistance = 3.0f; // Normal 3rd person distance
    public float minDistance = 0.5f; // How close it gets when hitting a wall
    public float smoothSpeed = 10f;
    public LayerMask obstacleLayer;  // Set this to "Default" in the Inspector

    private float currentDistance;

    void Start()
    {
        currentDistance = maxDistance;
    }

    void LateUpdate()
    {
        if (pivot == null) return;

        // 1. Calculate the ideal position (behind the pivot)
        Vector3 desiredPos = pivot.TransformPoint(Vector3.back * maxDistance);

        // 2. Check for walls between the Pivot and the Camera
        RaycastHit hit;
        if (Physics.Linecast(pivot.position, desiredPos, out hit, obstacleLayer))
        {
            // If we hit a wall, move camera to that hit point (slightly offset)
            currentDistance = Mathf.Clamp(hit.distance * 0.85f, minDistance, maxDistance);
        }
        else
        {
            // Otherwise, stay at max distance
            currentDistance = maxDistance;
        }

        // 3. Apply the final position smoothly
        Vector3 finalPos = pivot.position - pivot.forward * currentDistance;
        transform.position = Vector3.Lerp(transform.position, finalPos, Time.deltaTime * smoothSpeed);

        // 4. Ensure camera is always looking at the pivot
        transform.LookAt(pivot.position);
    }
}
