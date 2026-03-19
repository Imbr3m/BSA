using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    // The NPC holds their own specific lines here!
    public DialogueManager.DialogueSegment[] myLines; 
    private bool playerInRange;

    void Update()
    {
        // If player is close, presses E, and dialogue IS NOT already running
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !DialogueManager.Instance.PlayingDialogue)
        {
            DialogueManager.Instance.StartDialogue(myLines);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}