using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [System.Serializable]
    public class DialogueSegment
    {
        public string Name;
        public Sprite CharacterPortrait; 
        public AudioClip VoiceSound;     
        [TextArea] public string DialogueToPrint;
        public float LettersPerSecond = 20f;
    }

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private Image portraitHolder; 

    private DialogueSegment[] currentSegments;
    private int dialogueIndex;
    public bool PlayingDialogue { get; private set; }
    private bool skip;
    private bool isTyping;

    void Awake()
    {
        if (Instance == null) Instance = this;

        // FIX 1: Removed dialoguePanel.SetActive(false); 
        // The box will now stay visible when the game starts.
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
        
        // Ensure it's active just in case it was hidden manually
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
            
            // clears the dialogue after finish
            bodyText.text = ""; 
            nameText.text = "";

            if (portraitHolder != null) 
            {
                portraitHolder.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator PlayDialogue(DialogueSegment segment)
    {
        isTyping = true;
        skip = false;
        
        // FIX 2: Removed the Undertale asterisk
        bodyText.text = ""; 
        
        nameText.text = segment.Name; 

        // FIX 3: Portrait Safety Check
        // If CharacterPortrait is empty in the Inspector, it simply hides the Image 
        // component so you don't see a white square. It won't crash your game!
        if (portraitHolder != null)
        {
            if (segment.CharacterPortrait != null)
            {
                portraitHolder.gameObject.SetActive(true);
                portraitHolder.sprite = segment.CharacterPortrait;
            }
            else
            {
                portraitHolder.gameObject.SetActive(false); 
            }
        }

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