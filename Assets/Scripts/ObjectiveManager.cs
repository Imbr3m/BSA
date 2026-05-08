using UnityEngine;
using TMPro; // Needed to talk to your TextMeshPro UI!

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance;

    [Header("UI References")]
    [Tooltip("Drag your 'Objective text' object here!")]
    public TMP_Text objectiveText;

    [Header("Flower Quest Settings")]
    public int totalFlowersNeeded = 4;
    private int flowersCollected = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SetObjective(string newObjective)
    {
        if (objectiveText != null)
        {
            objectiveText.text = "" + newObjective; 
        }
    }

    public void CollectFlower()
    {
        flowersCollected++;
        
        if (flowersCollected < totalFlowersNeeded)
        {
            // Updates the text to show progress! e.g., "Collect all Flowers (1/3)"
            SetObjective("Collect all Flowers (" + flowersCollected + "/" + totalFlowersNeeded + ")");
        }
        else
        {
            // The quest is done!
            SetObjective("Go back and talk to Nanay!");
        }
    }
}