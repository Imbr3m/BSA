using UnityEngine;

public class FlowerPickup : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private PlayerContextClue playerClueScript; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            
            playerClueScript = other.GetComponentInChildren<PlayerContextClue>();
            if (playerClueScript != null)
            {
                playerClueScript.ShowClue(PlayerContextClue.ClueType.Exclamation); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            
            if (playerClueScript != null)
            {
                playerClueScript.HideClue(); 
            }
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerClueScript != null)
            {
                playerClueScript.HideClue();
            }

            ObjectiveManager.Instance.CollectFlower();
            
            Destroy(gameObject);
        }
    }
}