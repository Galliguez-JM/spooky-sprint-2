using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CandyManager : MonoBehaviour
{
    [Header("UI References")]
    public Image fillImage;
    public CanvasGroup fadeOverlay;

    // AUDIO SETTINGS REMOVED - Logic moved to Collectible.cs

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
        // SOUND LOGIC REMOVED FROM HERE
    }

    IEnumerator FinishLevel()
    {
        if (isFinishing) yield break;
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

        int currentTime = PlayerPrefs.GetInt("CurrentHour", 17);
        int nextRequiredHouse = PlayerPrefs.GetInt("CorrectHouse", 1);
        int currentTotalCandies = PlayerPrefs.GetInt("TotalCandies", 0);

        string houseKey = "House_" + houseID + "_Cleared";
        if (PlayerPrefs.GetInt(houseKey, 0) == 0)
        {
            currentTotalCandies++;
            PlayerPrefs.SetInt("TotalCandies", currentTotalCandies);
            PlayerPrefs.SetInt(houseKey, 1);
        }

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

        bool finishedAllHouses = PlayerPrefs.GetInt("CorrectHouse") > totalHousesInGame;
        if (finishedAllHouses && currentTime < 24)
        {
            SceneManager.LoadScene("WinScene");
        }
        else
        {
            SceneManager.LoadScene(7);
        }
    }
}
