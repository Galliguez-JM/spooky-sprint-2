using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    void Start()
    {
        // Make sure the mouse is visible so they can click
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Connect this to a Button's OnClick() in the Inspector
    public void ContinueToTown()
    {
        // After the intro, go to the Town (Scene 7)
        SceneManager.LoadScene(7);
    }

    // Optional: If you want them to be able to just press ANY key or Click anywhere
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        {
            ContinueToTown();
        }
    }
}
