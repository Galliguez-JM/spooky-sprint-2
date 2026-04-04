using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HouseManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI candyTrackerText; // NEW: Drag your "0/6" text here

    void Start()
    {
        int currentHour = PlayerPrefs.GetInt("CurrentHour", 17);
        UpdateTimerUI(currentHour);
        UpdateCandyUI(); // NEW: Update the candy count

        if (currentHour >= 24)
        {
            TriggerGameOver();
        }
    }

    void UpdateTimerUI(int hour)
    {
        if (hour >= 24)
        {
            timerText.text = "MIDNIGHT";
        }
        else
        {
            int displayHour = hour;
            string suffix = " PM";
            if (hour > 12) displayHour = hour - 12;
            if (hour == 12) suffix = " PM";
            timerText.text = displayHour + ":00" + suffix;
        }
    }

    // --- NEW FUNCTION: UPDATES CANDY COUNT ---
    void UpdateCandyUI()
    {
        // Since CorrectHouse starts at 1, we subtract 1 to show 0/6 at the start
        int candiesCollected = PlayerPrefs.GetInt("CorrectHouse", 1) - 1;

        // Clamp it so it doesn't show 7/6 if they win
        if (candiesCollected > 6) candiesCollected = 6;

        if (candyTrackerText != null)
        {
            candyTrackerText.text = "CANDIES: " + candiesCollected + " / 6";
        }
    }

    void TriggerGameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
