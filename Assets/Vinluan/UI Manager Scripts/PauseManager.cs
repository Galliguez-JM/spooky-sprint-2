using UnityEngine;
using UnityEngine.SceneManagement; // Added this so you can switch scenes

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    private bool isPaused = false;

    void Update()
    {
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
        Time.timeScale = 0f;
        isPaused = true;

        // This unlocks your mouse so you can click the buttons!
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // This hides the mouse again for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Add this so your "Main Menu" button has a function to call
    public void ReturnToMenu()
    {
        Time.timeScale = 1f; // IMPORTANT: Unpause time before leaving!
        SceneManager.LoadScene(0); // Loads the scene at index 0 (Title Screen)
    }
}
