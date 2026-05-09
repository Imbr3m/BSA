using UnityEngine;
using UnityEngine.Playables; // You need this to talk to Timelines!

public class StartTimeline : MonoBehaviour
{
    [Header("The Timeline to Start")]
    public PlayableDirector timelineToPlay;

    // You will link this function to your UI Button's OnClick event!
    public void BeginTimeline()
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