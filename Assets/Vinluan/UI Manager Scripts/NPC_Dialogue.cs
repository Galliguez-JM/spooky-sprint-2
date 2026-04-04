using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NPC_Dialogue : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject choiceButtons;
    public Button enterButton;
    public Button stayButton;

    [Header("NPC Info")]
    public string npcName = "MR. PEEK N. BOO";
    public string houseSceneName = "Level 1";
    public int thisNPCHouseID = 1; // Set this to 1 for NPC1, 2 for NPC2, etc.

    [Header("Dialogue Content")]
    [TextArea(3, 10)]
    public string[] initialDialogue; // First time talking
    [TextArea(3, 10)]
    public string[] hintDialogue;    // Dialogue after their house is finished

    private string[] currentDialogueSet; // The set we are currently using
    private int currentLine = 0;
    private bool isPlayerNear = false;
    private bool isDialogueActive = false;

    void Start()
    {
        if (enterButton == null && choiceButtons != null)
            enterButton = choiceButtons.transform.Find("Enter").GetComponent<Button>();
        if (stayButton == null && choiceButtons != null)
            stayButton = choiceButtons.transform.Find("Stay").GetComponent<Button>();
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E) && !isDialogueActive)
        {
            StartDialogue();
        }
        else if (isDialogueActive && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)))
        {
            NextLine();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        if (dialoguePanel.TryGetComponent(out CanvasGroup cg)) cg.alpha = 1f;

        choiceButtons.SetActive(false);
        nameText.text = npcName;

        // --- NEW: LOGIC TO PICK DIALOGUE ---
        int nextHouseNeeded = PlayerPrefs.GetInt("CorrectHouse", 1);

        // If the player has already finished THIS NPC's house, show the hint instead
        if (nextHouseNeeded > thisNPCHouseID)
        {
            currentDialogueSet = hintDialogue;
        }
        else
        {
            currentDialogueSet = initialDialogue;
        }

        DisplayLine();

        if (enterButton != null)
        {
            enterButton.onClick.RemoveAllListeners();
            enterButton.onClick.AddListener(EnterHouse);
        }
        if (stayButton != null)
        {
            stayButton.onClick.RemoveAllListeners();
            stayButton.onClick.AddListener(CloseDialogue);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void DisplayLine()
    {
        dialogueText.text = currentDialogueSet[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < currentDialogueSet.Length)
        {
            DisplayLine();
        }
        else
        {
            choiceButtons.SetActive(true);
            dialogueText.text = "Will you go inside?";
        }
    }

    public void EnterHouse()
    {
        if (!string.IsNullOrEmpty(houseSceneName))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
                PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
                PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
                PlayerPrefs.SetInt("HasSavedPos", 1);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene(houseSceneName);
        }
    }

    public void CloseDialogue()
    {
        isDialogueActive = false;
        if (dialoguePanel.TryGetComponent(out CanvasGroup cg)) cg.alpha = 0f;
        dialoguePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) isPlayerNear = false; }
}
