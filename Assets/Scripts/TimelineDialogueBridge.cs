using UnityEngine;
using UnityEngine.Playables;

public class TimelineDialogueBridge : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public DialogueManager.DialogueSegment[] introLines;
    public PlayerController realPlayer; 
    
    public Animator playerAnimator; 

    [Header("Walking Parameters")]
    public string walkX = "moveX"; 
    public string walkY = "moveY";
    
    [Header("Idle Memory Parameters")]
    [Tooltip("Check your Idle Blend Tree! What are these called? (e.g., lastMoveX)")]
    public string idleX = "lastMoveX"; 
    public string idleY = "lastMoveY";

    [Header("Direction to Face")]
    public float faceX = 1f; 
    public float faceY = 0f;

    private void Start()
    {
        if (realPlayer != null) realPlayer.isAiming = true;
    }

    public void StartCutsceneDialogue()
    {
        timelineDirector.Pause(); 
        DialogueManager.Instance.StartDialogue(introLines); 
    }

    void Update()
    {
        if (timelineDirector.state != PlayState.Playing && !DialogueManager.Instance.PlayingDialogue)
        {
            if (timelineDirector.time > 0 && timelineDirector.time < timelineDirector.duration)
            {
                timelineDirector.Play();
            }
        }
    }

    void LateUpdate()
    {
        // The Shotgun Method: Force both the active movement and the idle memory!
        if (DialogueManager.Instance.PlayingDialogue && playerAnimator != null)
        {
            playerAnimator.SetFloat(walkX, faceX);
            playerAnimator.SetFloat(walkY, faceY);
            
            // If the idle parameters exist, force those too!
            if (!string.IsNullOrEmpty(idleX))
            {
                playerAnimator.SetFloat(idleX, faceX);
                playerAnimator.SetFloat(idleY, faceY);
            }
        }
    }

    public void EndCutscene()
    {
        if (realPlayer != null)
        {
            realPlayer.isAiming = false; 
        }
    }
}