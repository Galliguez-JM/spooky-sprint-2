using UnityEngine;

public class PositionLoader : MonoBehaviour
{
    void Start()
    {
        // Check if we have a saved position
        if (PlayerPrefs.GetInt("HasSavedPos", 0) == 1)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float x = PlayerPrefs.GetFloat("PlayerX");
                float y = PlayerPrefs.GetFloat("PlayerY");
                float z = PlayerPrefs.GetFloat("PlayerZ");

                // Move the player to the saved coordinates
                player.transform.position = new Vector3(x, y, z);
            }
        }
    }
}
