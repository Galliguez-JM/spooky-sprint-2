using UnityEngine;
using UnityEngine.UI;

public class CandyManager : MonoBehaviour
{
    public Image fillImage; // Drag 'CandyBar_Fill' here
    public int totalCandiesInHouse = 1; // Change this per level in Inspector
    private int currentCandies = 0;
    private float targetProgress = 0f;

    void Start()
    {
        // Start the bar at 0
        fillImage.fillAmount = 0;
    }

    void Update()
    {
        // This makes the bar SLIDE smoothly instead of jumping
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetProgress, Time.deltaTime * 5f);
    }

    public void AddCandy()
    {
        currentCandies++;
        // Calculate progress (e.g. 1 candy / 4 total = 0.25 fill)
        targetProgress = (float)currentCandies / totalCandiesInHouse;

        Debug.Log("Candy Collected! Total: " + currentCandies);
    }
}
