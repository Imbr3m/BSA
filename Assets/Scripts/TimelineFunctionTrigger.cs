using UnityEngine;
using UnityEngine.Playables; // You need this to talk to Timelines!

public class TimelineFunctionTrigger : MonoBehaviour
{
    [Header("The Timeline to Start")]
    public PlayableDirector timelineToPlay;

    // You can call this function from a button, an interact prompt, or an Event!
    public void StartTimeline()
    {
        if (timelineToPlay != null)
        {
            Debug.Log("Playing Timeline: " + timelineToPlay.gameObject.name);
            timelineToPlay.Play();
        }
        else
        {
            Debug.LogWarning("You forgot to assign a Timeline to the TimelineTrigger script!");
        }
    }
}