using UnityEngine;
using UnityEngine.Playables; // We need this to talk to Timelines!

public class TimelineTrigger : MonoBehaviour
{
    [Header("Timeline Settings")]
    public PlayableDirector timelineToPlay;
    
    [Tooltip("Check this if you only want the cutscene to happen the first time she walks through.")]
    public bool playOnlyOnce = true;
    
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playOnlyOnce && hasPlayed) return;

            hasPlayed = true;
            
            if (timelineToPlay != null)
            {
                timelineToPlay.Play();
            }
        }
    }
}