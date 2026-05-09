using UnityEngine;

public class FlowerPickup : MonoBehaviour
{
    [Header("Audio Settings")]
    [Tooltip("Drop your pickup sound effect here!")]
    public AudioClip pickupSound; 
    [Range(0f, 1f)] public float soundVolume = 1f;

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

            if (pickupSound != null && SoundFXManager.instance != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(pickupSound, transform, soundVolume);
            }
            
            Destroy(gameObject);
        }
    }
}