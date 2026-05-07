using UnityEngine;

public class AutoDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public DialogueManager.DialogueSegment[] myLines;
    
    [Tooltip("Check this if you only want the dialogue to play the very first time she walks in.")]
    public bool playOnlyOnce = true;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // 1. Check if the object entering the box is the Player
        if (other.CompareTag("Player"))
        {
            // 2. Stop here if it's already been triggered and we only want it to play once
            if (playOnlyOnce && hasTriggered) return;

            // 3. Mark it as triggered so it doesn't spam
            hasTriggered = true;
            
            // 4. Instantly start the dialogue without waiting for 'E'
            DialogueManager.Instance.StartDialogue(myLines);
        }
    }
}