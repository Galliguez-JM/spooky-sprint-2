using UnityEngine;
using TMPro; // This lets the script see TextMeshPro
using UnityEngine.SceneManagement;

public class HouseManager : MonoBehaviour
{
    [Header("UI References")]
    // CHANGE 'Text' TO 'TextMeshProUGUI' BELOW:
    public TextMeshProUGUI timerText;

    void Start()
    {
        int currentHour = PlayerPrefs.GetInt("CurrentHour", 19);
        UpdateTimerUI(currentHour);

        if (currentHour >= 24)
        {
            TriggerMonsterChase();
        }
    }

    void UpdateTimerUI(int hour)
    {
        timerText.text = hour >= 24 ? "MIDNIGHT" : hour + ":00";
    }

    void TriggerMonsterChase()
    {
        Debug.Log("THE MONSTER IS COMING!");
        // SceneManager.LoadScene("MonsterScene"); 
    }
}
