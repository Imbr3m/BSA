using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections; 

public class RockThrowQTE : MonoBehaviour
{
    [Header("UI References")]
    public GameObject qtePanel;
    public Image fillBar;
    
    [Header("Transitions")]
    public CanvasGroup canvasGroup; 
    public RectTransform panelRect; 
    
    [Tooltip("How fast it fades IN")]
    public float transitionInTime = 0.25f; 
    [Tooltip("How fast it fades OUT")]
    public float transitionOutTime = 0.1f; 
    
    public float slideDistance = 50f; 

    [Header("Mechanics")]
    public float fillSpeed = 0.5f; 
    [Range(0f, 1f)] public float winZoneMin = 0.6f;
    [Range(0f, 1f)] public float winZoneMax = 0.8f;

    [Header("Events")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;

    private bool isPlaying = false;
    private float currentFill = 0f;
    private float fillDirection = 1f; 

    private Vector2 restingPosition;
    private Coroutine currentTransition;

    void Start()
    {
        if (panelRect != null)
        {
            restingPosition = panelRect.anchoredPosition;
        }
    }

    void Awake()
    {
        if (panelRect != null)
        {
            restingPosition = panelRect.anchoredPosition;
        }
    }

    public void ShowQTEPanel()
    {
        qtePanel.SetActive(true);
        currentFill = 0f;
        fillBar.fillAmount = 0f;

        canvasGroup.alpha = 0f;
        panelRect.anchoredPosition = restingPosition - new Vector2(0, slideDistance);

        if (currentTransition != null) StopCoroutine(currentTransition);
        
        currentTransition = StartCoroutine(FadePanel(1f, restingPosition, transitionInTime));
    }

    public void HideQTEPanel()
    {
        if (currentTransition != null) StopCoroutine(currentTransition);
        
        Vector2 hiddenPosition = restingPosition - new Vector2(0, slideDistance);
        
        currentTransition = StartCoroutine(FadePanel(0f, hiddenPosition, transitionOutTime, true));
    }

    public void StartQTE()
    {
        fillDirection = 1f; 
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying) return;

        if (Input.GetMouseButton(0))
        {
            currentFill += fillSpeed * fillDirection * Time.deltaTime;

            if (currentFill >= 1f)
            {
                currentFill = 1f; 
                fillDirection = -1f;
            }
            else if (currentFill <= 0f)
            {
                currentFill = 0f; 
                fillDirection = 1f;
            }

            fillBar.fillAmount = currentFill;
        }
        else if (Input.GetMouseButtonUp(0)) 
        {
            EvaluateThrow();
        }
    }

    private void EvaluateThrow()
    {
        isPlaying = false;

        if (currentFill >= winZoneMin && currentFill <= winZoneMax)
        {
            Debug.Log("Rock Thrown Successfully!");
            OnSuccess.Invoke(); 
        }
        else
        {
            Debug.Log("Fumbled the rock!");
            OnFail.Invoke(); 
        }

        HideQTEPanel(); 
    }

    private IEnumerator FadePanel(float targetAlpha, Vector2 targetPosition, float duration, bool disableAfter = false)
    {
        float startAlpha = canvasGroup.alpha;
        Vector2 startPosition = panelRect.anchoredPosition;
        
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float percentage = timer / duration;

            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, percentage);
            panelRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, percentage);

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        panelRect.anchoredPosition = targetPosition;

        if (disableAfter)
        {
            qtePanel.SetActive(false);
        }
    }
}