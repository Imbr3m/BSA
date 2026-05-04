using UnityEngine;
// You MUST include this line, or Unity won't know what a "QuickTimeEvent" is![cite: 8, 9]
using QTEPack; 

public class QTETestTrigger : MonoBehaviour
{
    [Header("QTE Settings")]
    [SerializeField] public QuickTimeEvent QTE; // Drag your QTE prefab here
    
    // Difficulty goes from 0 to 4 based on the package settings[cite: 10]
    [SerializeField] private int difficultyLevel = 2; 

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // Check if Sampaguita walked into the box
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Make sure it only fires once
            StartQTE();
        }
    }

    public void StartQTE()
    {
        // 1. Tell the QTE what to do when Sampaguita wins or loses
        QTE.OnSuccess.AddListener(OnQTESuccess);
        QTE.OnFail.AddListener(OnQTEFail);

        // 2. Show the QTE on screen
        // Parameters: Screen Position (Vector2), Scale (float), Difficulty (int)
        QTE.ShowQTE(new Vector2(0f, 0f), 1f, difficultyLevel); 
    }

    private void OnQTESuccess()
    {
        Debug.Log("Sampaguita passed the QTE!");
        FinishQTE();
    }

    private void OnQTEFail()
    {
        Debug.Log("Sampaguita failed the QTE!");
        FinishQTE();
    }

    private void FinishQTE()
    {
        // 1. Clean up the listeners so they don't cause bugs later
        QTE.OnSuccess.RemoveAllListeners();
        QTE.OnFail.RemoveAllListeners();

        // 2. Hide the QTE UI[cite: 9]
        QTE.Hide();
    }
}