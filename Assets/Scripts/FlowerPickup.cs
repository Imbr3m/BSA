using UnityEngine;

public class FlowerPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // When Sampaguita touches the flower...
        if (other.CompareTag("Player"))
        {
            ObjectiveManager.Instance.CollectFlower();
            
            Destroy(gameObject);
        }
    }
}