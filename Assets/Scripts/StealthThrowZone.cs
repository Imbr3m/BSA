using UnityEngine;

public class StealthThrowZone : MonoBehaviour
{
    [Header("References")]
    public RockThrowQTE qteManager;
    public GameObject visualIndicator; 
    public PlayerController player;
    
    private bool inZone = false;
    private bool qteTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !qteTriggered)
        {
            inZone = true;
            if (visualIndicator != null) visualIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
            if (visualIndicator != null) visualIndicator.SetActive(false);
        }
    }

    void Update()
    {
        // If they are in the zone, are crouching, and press Left Click
        if (inZone && !qteTriggered && player.isCrouching && Input.GetMouseButtonDown(0))
        {
            qteTriggered = true; 
            if (visualIndicator != null) visualIndicator.SetActive(false);
            
            // Freeze the player so they can't walk away during the throw!
            player.enabled = false; 

            // Start the bar minigame
            qteManager.StartQTE();
        }
    }
}