using UnityEngine;
using UnityEngine.Playables;

public class QuestTurnInTrigger : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [Tooltip("Drag the Timeline you want to play when you give her the flowers here!")]
    public PlayableDirector turnInCutscene; 
    
    private bool isPlayerInRange = false;
    private PlayerContextClue playerClueScript;
    private bool hasTurnedIn = false; // Stops you from playing the cutscene twice!

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the player AND if the quest is actually finished!
        if (other.CompareTag("Player") && ObjectiveManager.Instance.IsQuestComplete() && !hasTurnedIn)
        {
            isPlayerInRange = true;
            playerClueScript = other.GetComponentInChildren<PlayerContextClue>();
            
            if (playerClueScript != null)
            {
                // Let's use the 'Dots' or 'Exclamation' bubble for talking!
                playerClueScript.ShowClue(PlayerContextClue.ClueType.Dots); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            if (playerClueScript != null)
            {
                playerClueScript.HideClue();
            }
        }
    }

    private void Update()
    {
        // If she's close, has the flowers, presses E, and hasn't turned it in yet...
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !hasTurnedIn)
        {
            hasTurnedIn = true; // Lock it so she can't spam E
            
            // 1. Hide the interaction bubble
            if (playerClueScript != null)
            {
                playerClueScript.HideClue();
            }

            if (turnInCutscene != null)
            {
                turnInCutscene.Play();
            }
        }
    }
}