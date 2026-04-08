using UnityEngine;
using System.Collections;

public class GuideActivator : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject dialogueBox;
    public float dialogueDelay = 60f; // 1 minute

    [Header("Arrow Settings")]
    public GameObject[] guideArrows; // List of all arrows
    public float arrowDelay = 60f;    // 1 more minute after the dialogue

    void Start()
    {
        // Initial setup: Hide everything
        if (dialogueBox != null) dialogueBox.SetActive(false);

        foreach (GameObject arrow in guideArrows)
        {
            if (arrow != null) arrow.SetActive(false);
        }

        StartCoroutine(ActivationTimeline());
    }

    IEnumerator ActivationTimeline()
    {
        // 1. Wait for the first minute
        yield return new WaitForSeconds(dialogueDelay);

        if (dialogueBox != null)
            dialogueBox.SetActive(true);

        Debug.Log("Dialogue Box active!");

        // 2. Wait for the NEXT minute (Total 2 mins)
        yield return new WaitForSeconds(arrowDelay);

        foreach (GameObject arrow in guideArrows)
        {
            if (arrow != null) arrow.SetActive(true);
        }

        Debug.Log("All Arrows active!");
    }
}
