using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [System.Serializable]
    public class DialogueSegment
    {
        public string Name;
        public AudioClip VoiceSound;     
        [TextArea] public string DialogueToPrint;
        public float LettersPerSecond = 20f;
    }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text bodyText;

    private DialogueSegment[] currentSegments;
    private int dialogueIndex;
    public bool PlayingDialogue { get; private set; }
    private bool skip;
    private bool isTyping;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (!PlayingDialogue) return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (isTyping) skip = true;
            else PlayNextSegment();
        }
    }

    public void StartDialogue(DialogueSegment[] segments)
    {
        currentSegments = segments;
        dialogueIndex = 0;
        PlayingDialogue = true;
        
        // Ensure it's visible just in case you manually turned it off in the Inspector
        if (dialoguePanel != null) dialoguePanel.SetActive(true); 
        
        PlayNextSegment();
    }

    private void PlayNextSegment()
    {
        if (dialogueIndex < currentSegments.Length)
        {
            StartCoroutine(PlayDialogue(currentSegments[dialogueIndex]));
        }
        else
        {
            PlayingDialogue = false;
            
            // Clear the text so the box is empty!
            bodyText.text = ""; 
            nameText.text = "";

            // No SetActive(false) here anymore. The box stays forever!
        }
    }

    private IEnumerator PlayDialogue(DialogueSegment segment)
    {
        isTyping = true;
        skip = false;
        
        bodyText.text = ""; 
        nameText.text = segment.Name; 

        float delay = 1f / segment.LettersPerSecond;
        
        for (int i = 0; i < segment.DialogueToPrint.Length; i++)
        {
            if (skip)
            {
                bodyText.text = segment.DialogueToPrint;
                break; 
            }

            bodyText.text += segment.DialogueToPrint[i];

            if (segment.VoiceSound != null && SoundFXManager.instance != null)
            {
                SoundFXManager.instance.PlayUIBeep(segment.VoiceSound, 0.1f);
            }

            yield return new WaitForSeconds(delay);
        }

        isTyping = false;
        dialogueIndex++;
    }
}