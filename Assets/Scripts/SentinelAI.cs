using UnityEngine;
using System.Collections;

public class SentinelAI : MonoBehaviour
{
    [Header("Timers")]
    [SerializeField] private float greenLightDuration = 4f; 
    [SerializeField] private float redLightDuration = 3f;   
    [SerializeField] private float turnDuration = 0.5f; 
    
    [Header("Combat")]
    [SerializeField] private float instantDamage = 30f; 
    
    private PlayerController playerInZone; // Track if player is inside the big trigger
    public bool isLooking { get; private set; }

    void Start()
    {
        StartCoroutine(WatchLoop());
    }

    IEnumerator WatchLoop()
    {
        while (true)
        {
            // 1. GREEN LIGHT: Looking Away
            isLooking = false;
            yield return StartCoroutine(SmoothTurn(180)); 
            yield return new WaitForSeconds(greenLightDuration);

            // 2. RED LIGHT: Turning Back
            yield return StartCoroutine(SmoothTurn(0)); 
            isLooking = true;

            // INSTANT CHECK: If player is in this specific zone AND not hidden
            if (playerInZone != null && !playerInZone.isHidden)
            {
                Debug.Log("SAMPAGUITA SPOTTED IN ZONE!");
                playerInZone.TakeDamage(instantDamage);
            }

            yield return new WaitForSeconds(redLightDuration);
        }
    }

    // This detects if Sampaguita is inside the huge trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = null;
        }
    }

    IEnumerator SmoothTurn(float targetY)
    {
        float time = 0;
        Quaternion startRotation = transform.localRotation;
        Quaternion endRotation = Quaternion.Euler(0, targetY, 0);
        while (time < turnDuration)
        {
            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, time / turnDuration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localRotation = endRotation;
    }
}