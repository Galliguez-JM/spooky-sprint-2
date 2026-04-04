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
    public int houseID; // Set to 1, 2, 3, 4, 5, or 6 in the Inspector
    public int totalHousesInGame = 6;

    private int currentCandies = 0;
    private float targetProgress = 0f;
    private bool isFinishing = false; // Prevents the script from running multiple times

    void Update()
    {
        // Smoothly animate the candy bar
        if (fillImage != null)
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetProgress, Time.deltaTime * 5f);

        // Check if house is done and not already finishing
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
        isFinishing = true; // Lock the coroutine immediately

        yield return new WaitForSeconds(1f);

        // Fade to black
        if (fadeOverlay != null)
        {
            while (fadeOverlay.alpha < 1)
            {
                fadeOverlay.alpha += Time.deltaTime;
                yield return null;
            }
        }

        // 1. Get current progress
        int currentTime = PlayerPrefs.GetInt("CurrentHour", 19);
        int nextRequiredHouse = PlayerPrefs.GetInt("CorrectHouse", 1);

        // 2. Logic: Was this the right house?
        if (houseID == nextRequiredHouse)
        {
            currentTime += 1; // Right house: +1 hour
            PlayerPrefs.SetInt("CorrectHouse", nextRequiredHouse + 1);
            Debug.Log("Correct House! Next is: " + (nextRequiredHouse + 1));
        }
        else
        {
            currentTime += 2; // Wrong house: +2 hours
            Debug.Log("Wrong House! Still looking for house: " + nextRequiredHouse);
        }

        // 3. Save the new time
        PlayerPrefs.SetInt("CurrentHour", currentTime);
        PlayerPrefs.Save();

        // 4. Scene Transition Logic
        // If the player just finished house 6 correctly, they win!
        if (PlayerPrefs.GetInt("CorrectHouse") > totalHousesInGame)
        {
            SceneManager.LoadScene("WinScene"); // Make sure you have a scene with this name
        }
        else
        {
            SceneManager.LoadScene(7); // Back to Town
        }
    }
}
