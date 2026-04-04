using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CandyManager : MonoBehaviour
{
    [Header("UI References")]
    public Image fillImage;
    public CanvasGroup fadeOverlay;

    [Header("House Settings")]
    public int totalCandiesInHouse = 1;
    public int houseID;
    public int totalHousesInGame = 6;

    private int currentCandies = 0;
    private float targetProgress = 0f;
    private bool isFinishing = false;

    void Update()
    {
        if (fillImage != null)
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetProgress, Time.deltaTime * 5f);

        if (currentCandies >= totalCandiesInHouse && !isFinishing)
        {
            StartCoroutine(FinishLevel());
        }
    }

    public void AddCandy()
    {
        currentCandies++;
        targetProgress = (float)currentCandies / (float)totalCandiesInHouse;
    }

    IEnumerator FinishLevel()
    {
        isFinishing = true;
        yield return new WaitForSeconds(1f);

        if (fadeOverlay != null)
        {
            while (fadeOverlay.alpha < 1)
            {
                fadeOverlay.alpha += Time.deltaTime;
                yield return null;
            }
        }

        // 1. Get current data (Start at 17)
        int currentTime = PlayerPrefs.GetInt("CurrentHour", 17);
        int nextRequiredHouse = PlayerPrefs.GetInt("CorrectHouse", 1);

        // 2. Logic: Correct (+1) or Wrong (+2)
        if (houseID == nextRequiredHouse)
        {
            currentTime += 1;
            PlayerPrefs.SetInt("CorrectHouse", nextRequiredHouse + 1);
        }
        else
        {
            currentTime += 2;
        }

        PlayerPrefs.SetInt("CurrentHour", currentTime);
        PlayerPrefs.Save();

        // 3. Scene Transition Logic
        if (PlayerPrefs.GetInt("CorrectHouse") > totalHousesInGame && currentTime <= 24)
        {
            SceneManager.LoadScene("WinScene");
        }
        else if (currentTime >= 24)
        {
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            SceneManager.LoadScene(7);
        }
    }
}
