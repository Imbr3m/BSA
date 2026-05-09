using UnityEngine;
using UnityEngine.Playables;

public class TimelineDialogueBridge : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public DialogueManager.DialogueSegment[] introLines;
    
    [Header("Assign ONE of these Players!")]
    public PlayerController realPlayer; 
    public ReversedPlayerController reversedPlayer; 
    
    public Animator playerAnimator; 

    [Header("Walking Parameters")]
    public string walkX = "moveX"; 
    public string walkY = "moveY";
    
    [Header("Idle Memory Parameters")]
    public string idleX = "lastMoveX"; 
    public string idleY = "lastMoveY";

    [Header("Direction to Face")]
    public float faceX = 1f; 
    public float faceY = 0f;

    private void Start()
    {
        if (realPlayer != null) realPlayer.isAiming = true;
        else if (reversedPlayer != null) reversedPlayer.isAiming = true;
    }

    public void StartCutsceneDialogue()
    {
        // THE MAGIC FIX: We freeze the timeline's speed to 0 instead of dropping the character!
        if (timelineDirector.playableGraph.IsValid())
        {
            timelineDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        }
        else 
        {
            timelineDirector.Pause(); // Fallback just in case
        }
        
        DialogueManager.Instance.StartDialogue(introLines); 
    }

    void Update()
    {
        // Check if the dialogue is finished typing out
        if (!DialogueManager.Instance.PlayingDialogue)
        {
            // If we froze time, unfreeze it!
            if (timelineDirector.playableGraph.IsValid() && timelineDirector.playableGraph.GetRootPlayable(0).GetSpeed() == 0d)
            {
                timelineDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
            }
            // If we used the standard pause, unpause it!
            else if (timelineDirector.state != PlayState.Playing && timelineDirector.time > 0 && timelineDirector.time < timelineDirector.duration)
            {
                timelineDirector.Play();
            }
        }
    }

    void LateUpdate()
    {
        if (DialogueManager.Instance.PlayingDialogue && playerAnimator != null)
        {
            playerAnimator.SetFloat(walkX, faceX);
            playerAnimator.SetFloat(walkY, faceY);
            
            if (!string.IsNullOrEmpty(idleX))
            {
                playerAnimator.SetFloat(idleX, faceX);
                playerAnimator.SetFloat(idleY, faceY);
            }
        }
    }

    public void EndCutscene()
    {
        if (realPlayer != null) realPlayer.isAiming = false; 
        else if (reversedPlayer != null) reversedPlayer.isAiming = false; 
    }

    public void TurnRight() { faceX = 1f; faceY = 0f; }
    public void TurnLeft() { faceX = -1f; faceY = 0f; }
    public void TurnUp() { faceX = 0f; faceY = 1f; }
    public void TurnDown() { faceX = 0f; faceY = -1f; }
}