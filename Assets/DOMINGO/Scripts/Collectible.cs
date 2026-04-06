using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public GameObject onCollectEffect;

    [Header("Audio Settings")]
    public AudioClip collectSound; // Drag your candy pickup sound here
    [Range(0, 1)] public float volume = 1f;

    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. Tell CandyManager to add candy and update UI
            CandyManager manager = FindObjectOfType<CandyManager>();
            if (manager != null) manager.AddCandy();

            // 2. Play the sound at the candy's position
            // We use PlayClipAtPoint because the candy object is about to be destroyed
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, volume);
            }

            // 3. Instantiate the particle effect
            if (onCollectEffect != null)
            {
                Instantiate(onCollectEffect, transform.position, transform.rotation);
            }

            // 4. Destroy the collectible
            Destroy(gameObject);
        }
    }
}
