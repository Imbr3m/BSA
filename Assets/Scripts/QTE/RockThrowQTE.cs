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

    public void StartQTE()
    {
        qtePanel.SetActive(true);
        currentFill = 0f;
        fillBar.fillAmount = 0f;
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying) return;

       
        if (Input.GetMouseButton(0))
        {
            currentFill += fillSpeed * Time.deltaTime;
            fillBar.fillAmount = currentFill;

            // If they hold it too long and it hits the very top
            if (currentFill >= 1f)
            {
                EvaluateThrow();
            }
        }
        //releasing Left Click
        else if (Input.GetMouseButtonUp(0) && currentFill > 0)
        {
            EvaluateThrow();
        }
    }

    private void EvaluateThrow()
    {
        isPlaying = false;
        qtePanel.SetActive(false);

        // release the mouse inside the sweet spot?
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
    }
}