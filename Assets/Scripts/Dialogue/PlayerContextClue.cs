using UnityEngine;

public class PlayerContextClue : MonoBehaviour
{
    public static PlayerContextClue Instance;
    
    // UPDATED: All of your new bubble types are here!
    public enum ClueType 
    { 
        Exclamation, 
        Question, 
        Dots,
        Heart,
        HeartBreak,
        Music,
        Scrambled
    }
    
    [Header("Visuals")]
    [SerializeField] private GameObject clueBubble;
    [SerializeField] private Animator bubbleAnim; 

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (clueBubble != null) clueBubble.SetActive(false);
    }

    public void ShowClue(ClueType type)
    {
        if (clueBubble != null) clueBubble.SetActive(true);

        if (bubbleAnim != null)
        {
            // UPDATED: This perfectly matches the names in your Animator screenshot!
            switch (type)
            {
                case ClueType.Exclamation:
                    bubbleAnim.Play("ExclamationMark");
                    break;
                case ClueType.Question:
                    bubbleAnim.Play("QuestionMark");
                    break;
                case ClueType.Dots:
                    bubbleAnim.Play("Dotted");
                    break;
                case ClueType.Heart:
                    bubbleAnim.Play("Heart");
                    break;
                case ClueType.HeartBreak:
                    bubbleAnim.Play("HeartBreak");
                    break;
                case ClueType.Music:
                    bubbleAnim.Play("Music");
                    break;
                case ClueType.Scrambled:
                    bubbleAnim.Play("Scrambled");
                    break;
            }
        }
    }

    public void HideClue()
    {
        if (clueBubble != null) clueBubble.SetActive(false);
    }
}