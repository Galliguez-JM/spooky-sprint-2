using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate() // LateUpdate is smoother for camera following
    {
        if (mainCamera != null)
        {
            // Make the PNG look at the camera
            transform.LookAt(mainCamera);

            // Optional: If the PNG is tilted, unlock this line to keep it standing straight up
            // transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        }
    }
}