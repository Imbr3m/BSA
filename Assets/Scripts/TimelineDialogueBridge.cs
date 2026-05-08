using UnityEngine;
using UnityEngine.Playables;

public class TimelineDialogueBridge : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public DialogueManager.DialogueSegment[] introLines;
    public PlayerController realPlayer; 

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

    public void EndCutscene()
    {
        if (realPlayer != null)
        {
            realPlayer.isAiming = false; 
        }
    }
}