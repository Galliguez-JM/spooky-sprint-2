using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Drag your Panel here in the Inspector
    private bool isPaused = false;

    void Update()
    {
        // Listen for the Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Freezes the game world
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f; // Unfreezes the game world
        isPaused = false;
    }
}
