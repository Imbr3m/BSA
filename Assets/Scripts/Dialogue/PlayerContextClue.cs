using System.Collections;
using UnityEngine;

public class PlayerContextClue : MonoBehaviour
{
    public static PlayerContextClue Instance;
    
    public enum ClueType 
    { 
        Exclamation, Question, Dots, Heart, HeartBreak, Music, Scrambled
    }
    
    [Header("Visuals")]
    [SerializeField] private GameObject clueBubble;
    [SerializeField] private Animator bubbleAnim; 
    [SerializeField] private SpriteRenderer bubbleSprite; // NEW: Needed to change transparency

    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.2f; // How fast it fades
    [SerializeField] private float moveDistance = 0.3f; // How far down it starts

    private Vector3 originalLocalPos;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        if (clueBubble != null) 
        {
            // Remember exactly where the bubble is supposed to float
            originalLocalPos = clueBubble.transform.localPosition;
            clueBubble.SetActive(false);
        }
    }

    public void ShowClue(ClueType type)
    {
        if (clueBubble == null || bubbleSprite == null) return;

        clueBubble.SetActive(true);

        if (bubbleAnim != null)
        {
            switch (type)
            {
                case ClueType.Exclamation: bubbleAnim.Play("ExclamationMark"); break;
                case ClueType.Question: bubbleAnim.Play("QuestionMark"); break;
                case ClueType.Dots: bubbleAnim.Play("Dotted"); break;
                case ClueType.Heart: bubbleAnim.Play("Heart"); break;
                case ClueType.HeartBreak: bubbleAnim.Play("HeartBreak"); break;
                case ClueType.Music: bubbleAnim.Play("Music"); break;
                case ClueType.Scrambled: bubbleAnim.Play("Scrambled"); break;
            }
        }

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeRoutine(true));
    }

    public void HideClue()
    {
        if (clueBubble != null && clueBubble.activeSelf)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeRoutine(false));
        }
    }

    private IEnumerator FadeRoutine(bool isShowing)
    {
        if (isShowing) clueBubble.SetActive(true);

        float elapsedTime = 0f;
        Color currentColor = bubbleSprite.color;
        float targetAlpha = isShowing ? 1f : 0f;
        
        Vector3 startPos = clueBubble.transform.localPosition;
        Vector3 targetPos = isShowing ? originalLocalPos : originalLocalPos - new Vector3(0, moveDistance, 0);

        if (isShowing && currentColor.a <= 0.01f)
        {
            startPos = originalLocalPos - new Vector3(0, moveDistance, 0);
            clueBubble.transform.localPosition = startPos;
        }

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            
            float curveT = isShowing ? Mathf.Sin(t * Mathf.PI * 0.5f) : t; 

            bubbleSprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, Mathf.Lerp(currentColor.a, targetAlpha, curveT));
            clueBubble.transform.localPosition = Vector3.Lerp(startPos, targetPos, curveT);
            
            yield return null;
        }

        bubbleSprite.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        clueBubble.transform.localPosition = targetPos;

        if (!isShowing) clueBubble.SetActive(false);
    }
}