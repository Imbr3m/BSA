using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueManager.DialogueSegment[] myLines; 
    
    // NEW: This creates a beautiful dropdown in your Unity Inspector!
    [Tooltip("Which icon should float over Sampaguita's head?")]
    public PlayerContextClue.ClueType bubbleType = PlayerContextClue.ClueType.Exclamation;

    private bool playerInRange;

    void Update()
    {
        if (playerInRange)
        {
            if (DialogueManager.Instance.PlayingDialogue)
            {
                PlayerContextClue.Instance.HideClue();
            }
            else
            {
                // FIX: Tell the Singleton which specific bubble to turn on!
                PlayerContextClue.Instance.ShowClue(bubbleType);
                
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlayerContextClue.Instance.HideClue(); 
                    DialogueManager.Instance.StartDialogue(myLines);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            playerInRange = false;
            PlayerContextClue.Instance.HideClue();
        }
    }
}