using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        // Clear old data so the clock resets to 7 PM
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentHour", 17);
        PlayerPrefs.SetInt("CorrectHouse", 1);
        PlayerPrefs.SetInt("HasSavedPos", 0);
        PlayerPrefs.SetInt("TotalCandies", 0);
        PlayerPrefs.Save(); // Makes sure it writes to the disk immediately

        SceneManager.LoadScene(10); // Load your Hub
    }



    public void QuitGame()
    {
        Application.Quit();
    }
        
    
}
