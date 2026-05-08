using UnityEngine;

public class HoverItem : MonoBehaviour
{
    [Header("Hover Settings")]
    [Tooltip("How fast the item bobs up and down")]
    public float hoverSpeed = 2f; 
    
    [Tooltip("How high the item floats from its starting point")]
    public float hoverHeight = 0.25f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; 
    }

    void Update()
    {
        float newY = startPosition.y + (Mathf.Sin(Time.time * hoverSpeed) * hoverHeight);
        
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}