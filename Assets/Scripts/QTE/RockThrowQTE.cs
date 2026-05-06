using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RockThrowQTE : MonoBehaviour
{
    [Header("UI References")]
    public GameObject qtePanel;
    public Image fillBar;

    [Header("Mechanics")]
    [Tooltip("How fast the bar fills (1 = 1 second to fill)")]
    public float fillSpeed = 0.5f; 
    
    [Tooltip("Match these to where your green box is! (0.0 is bottom, 1.0 is top)")]
    [Range(0f, 1f)] public float winZoneMin = 0.6f;
    [Range(0f, 1f)] public float winZoneMax = 0.8f;

    [Header("Events")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;

    private bool isPlaying = false;
    private float currentFill = 0f;
    private float fillDirection = 1f; 

    public void ShowQTEPanel()
    {
        qtePanel.SetActive(true);
        currentFill = 0f;
        fillBar.fillAmount = 0f;
    }

    public void HideQTEPanel()
    {
        qtePanel.SetActive(false);
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

        qtePanel.SetActive(false);
    }
}