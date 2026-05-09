using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float damageInterval = 1.0f;
    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= nextDamageTime)
            {
                PlayerController normalPlayer = other.GetComponent<PlayerController>();
                if (normalPlayer != null)
                {
                    normalPlayer.TakeDamage(damageAmount); 
                    nextDamageTime = Time.time + damageInterval;
                    return;
                }

                ReversedPlayerController reversedPlayer = other.GetComponent<ReversedPlayerController>();
                if (reversedPlayer != null)
                {
                    reversedPlayer.TakeDamage(damageAmount); 
                    nextDamageTime = Time.time + damageInterval;
                }
            }
        }
    }
}