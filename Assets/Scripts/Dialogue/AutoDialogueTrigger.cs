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
        if (other.CompareTag("Player"))
        {
            if (playOnlyOnce && hasTriggered) return;

            hasTriggered = true;
            
            DialogueManager.Instance.StartDialogue(myLines);
        }
    }
}