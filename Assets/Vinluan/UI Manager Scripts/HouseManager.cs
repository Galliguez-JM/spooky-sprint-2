using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HouseManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI candyTrackerText;

    [Header("Midnight Chase Settings")]
    public GameObject monsterObject;

    void Start()
    {
        int currentHour = PlayerPrefs.GetInt("CurrentHour", 17);
        UpdateTimerUI(currentHour);
        UpdateCandyUI();

        if (currentHour >= 24)
        {
            TriggerMidnightEvent();
        }
        else
        {
            if (monsterObject != null) monsterObject.SetActive(false);
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

    void UpdateCandyUI()
    {
        // Pull the actual number of unique houses cleared
        int candiesCollected = PlayerPrefs.GetInt("TotalCandies", 0);

        if (candiesCollected > 6) candiesCollected = 6;

        if (candyTrackerText != null)
        {
            candyTrackerText.text = "CANDIES: " + candiesCollected + " / 6";
        }
    }

    void TriggerMidnightEvent()
    {
        if (monsterObject != null)
        {
            monsterObject.SetActive(true);
            MonsterFollow chase = monsterObject.GetComponent<MonsterFollow>();
            if (chase != null) chase.enabled = true;
        }
        else
        {
            SceneManager.LoadScene("GameOverScene");
        }
    }
}
