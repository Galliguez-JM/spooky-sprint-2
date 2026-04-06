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

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] initialAudioClips;
    public AudioClip[] hintAudioClips;
    [Tooltip("How many seconds of silence to skip at the start of the audio?")]
    public float audioStartTime = 0.5f;

    [Header("NPC Info")]
    public string npcName = "MR. PEEK N. BOO";
    public string houseSceneName = "Level 1";
    public int thisNPCHouseID = 1;

    [Header("Dialogue Content")]
    [TextArea(3, 10)]
    public string[] initialDialogue;
    [TextArea(3, 10)]
    public string[] hintDialogue;

    private string[] currentDialogueSet;
    private AudioClip[] currentAudioSet;
    private int currentLine = 0;
    private bool isPlayerNear = false;
    private bool isDialogueActive = false;

    void Start()
    {
        if (enterButton == null && choiceButtons != null)
            enterButton = choiceButtons.transform.Find("Enter").GetComponent<Button>();
        if (stayButton == null && choiceButtons != null)
            stayButton = choiceButtons.transform.Find("Stay").GetComponent<Button>();

        if (audioSource == null) audioSource = GetComponent<AudioSource>();
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
        CasperController player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CasperController>();
        if (player != null) player.canLook = false;

        currentLine = 0;
        dialoguePanel.SetActive(true);
        if (dialoguePanel.TryGetComponent(out CanvasGroup cg)) cg.alpha = 1f;
        choiceButtons.SetActive(false);
        nameText.text = npcName;

        int nextHouseNeeded = PlayerPrefs.GetInt("CorrectHouse", 1);
        if (nextHouseNeeded > thisNPCHouseID)
        {
            currentDialogueSet = hintDialogue;
            currentAudioSet = hintAudioClips;
        }
        else
        {
            currentDialogueSet = initialDialogue;
            currentAudioSet = initialAudioClips;
        }

        DisplayLine();
        PlayCurrentLineAudio();

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
            PlayCurrentLineAudio();
        }
        else
        {
            choiceButtons.SetActive(true);
            dialogueText.text = "Will you go inside?";
        }
    }

    void PlayCurrentLineAudio()
    {
        if (audioSource != null && currentAudioSet != null && currentLine < currentAudioSet.Length)
        {
            AudioClip clip = currentAudioSet[currentLine];
            if (clip != null)
            {
                audioSource.Stop();
                audioSource.clip = clip;
                if (audioStartTime < clip.length)
                    audioSource.time = audioStartTime;
                else
                    audioSource.time = 0f;

                audioSource.Play();
            }
        }
    }

    public void EnterHouse()
    {
        if (!string.IsNullOrEmpty(houseSceneName))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 pos = player.transform.position;
                PlayerPrefs.SetFloat("PlayerX", pos.x);
                PlayerPrefs.SetFloat("PlayerY", pos.y + 0.2f);
                PlayerPrefs.SetFloat("PlayerZ", pos.z);
                PlayerPrefs.SetInt("HasSavedPos", 1);
                PlayerPrefs.Save();

                Debug.Log("Saved Player Position: " + pos + " before entering " + houseSceneName);
            }

            SceneManager.LoadScene(houseSceneName);
        }
    }

    public void CloseDialogue()
    {
        isDialogueActive = false;
        CasperController player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CasperController>();
        if (player != null) player.canLook = true;

        if (dialoguePanel != null)
        {
            if (dialoguePanel.TryGetComponent(out CanvasGroup cg)) cg.alpha = 0f;
            dialoguePanel.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) { isPlayerNear = false; if (isDialogueActive) CloseDialogue(); } }
}
