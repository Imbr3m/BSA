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

            if (!qteTriggered)
            {
                qteManager.HideQTEPanel();
            }
        }
    }

    void Update()
    {
        if (inZone && !qteTriggered)
        {
            if (player.isCrouching)
            {
                if (!qteManager.qtePanel.activeSelf)
                {
                    qteManager.ShowQTEPanel();
                }

                if (Input.GetMouseButtonDown(0))
                {
                    qteTriggered = true; 
                    if (visualIndicator != null) visualIndicator.SetActive(false);
                    
                    player.isAiming = true; // Freeze her!
                    qteManager.StartQTE();  // Start the minigame!
                }
            }
            else 
            {
                if (qteManager.qtePanel.activeSelf)
                {
                    qteManager.HideQTEPanel();
                }
            }
        }
    }

    public void OnThrowSuccess()
    {
        Debug.Log("EVENT FIRED: (Success)");
        player.isAiming = false; 
    }

    public void OnThrowFail()
    {
        Debug.Log("EVENT FIRED: (Fail)");
        player.isAiming = false; 
        qteTriggered = false; 
        
        if (inZone && visualIndicator != null) 
        {
            visualIndicator.SetActive(true);
        }
    }
}