using UnityEngine;
using UnityEngine.UI;
using QTEPack; 
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections; // NEW: Required for Coroutines!

public class SanityManager : MonoBehaviour
{
    [Header("Sanity Settings")]
    public float maxSanity = 100f;
    public float currentSanity;
    public float sanityDrainRate = 2f; 
    
    [Range(0f, 1f)]
    public float colorDrainThreshold = 0.2f; 
    
    // NEW: How many seconds it takes to fade the color/flower back in
    public float recoveryDuration = 1.5f; 

    [Header("Flower UI")]
    public Image flowerUI;
    public Sprite[] flowerFrames; 

    [Header("Panic Attack QTE")]
    public QuickTimeEvent panicQTE;
    public PlayerController player;
    public float panicDamage = 15f; 
    
    [Header("Visual Effects")]
    public Volume postProcessingVolume; 
    private ColorAdjustments colorAdjustments;
    private float startingSaturation; 

    private bool isPanicking = false;
    private bool isRecovering = false; // NEW: Tells the game not to drain sanity while recovering

    void Start()
    {
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out colorAdjustments))
        {
            startingSaturation = colorAdjustments.saturation.value;
        }

        currentSanity = maxSanity;
        UpdateFlowerUI();
    }

    void Update()
    {
        // NEW: Only drain sanity if she isn't panicking AND isn't currently recovering
        if (!isPanicking && !isRecovering)
        {
            currentSanity -= sanityDrainRate * Time.deltaTime;
            currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
            
            UpdateFlowerUI();

            if (currentSanity <= 0)
            {
                TriggerPanicAttack();
            }
        }
    }

    void UpdateFlowerUI()
    {
        if (flowerFrames.Length == 0) return;

        float sanityPercent = currentSanity / maxSanity;

        if (colorAdjustments != null)
        {
            if (sanityPercent <= colorDrainThreshold)
            {
                float dangerPercent = sanityPercent / colorDrainThreshold;
                colorAdjustments.saturation.value = Mathf.Lerp(-70f, startingSaturation, dangerPercent);
            }
            else
            {
                colorAdjustments.saturation.value = startingSaturation;
            }
        }

        int frameIndex = Mathf.FloorToInt((1f - sanityPercent) * (flowerFrames.Length - 1));
        frameIndex = Mathf.Clamp(frameIndex, 0, flowerFrames.Length - 1);

        flowerUI.sprite = flowerFrames[frameIndex];
    }

    public void TriggerPanicAttack()
    {
        isPanicking = true;
        player.SetPanicPortrait(true);
        player.enabled = false; 
        StartPanicQTE();
    }

    private void StartPanicQTE()
    {
        panicQTE.OnSuccess.RemoveAllListeners();
        panicQTE.OnFail.RemoveAllListeners();

        panicQTE.OnSuccess.AddListener(OnPanicSuccess);
        panicQTE.OnFail.AddListener(OnPanicFail);

        panicQTE.ShowQTE(new Vector2(0, 0), 1f, 0); 
    }

    private void OnPanicSuccess()
    {
        isPanicking = false;
        
        // NEW: Start the smooth recovery Coroutine instead of an instant snap!
        StartCoroutine(RecoverSanity(maxSanity * 0.5f)); 
        
        player.SetPanicPortrait(false);
        player.enabled = true;
        panicQTE.Hide();
    }

    private void OnPanicFail()
    {
        player.TakeDamage(panicDamage);
        panicQTE.Hide(); 
        
        if (player.currentHealth > 0) 
        {
            StartPanicQTE();
        }
    }

    // NEW: The Coroutine that smoothly animates the sanity going back up
    private IEnumerator RecoverSanity(float targetSanity)
    {
        isRecovering = true;
        float startSanity = currentSanity;
        float timer = 0f;

        while (timer < recoveryDuration)
        {
            timer += Time.deltaTime;
            
            // Smoothly slide the number from 0 to 50
            currentSanity = Mathf.Lerp(startSanity, targetSanity, timer / recoveryDuration);
            
            // This naturally updates the UI color and flower frame every single millisecond!
            UpdateFlowerUI(); 
            
            yield return null;
        }

        // Lock in the final exact number just to be safe
        currentSanity = targetSanity;
        UpdateFlowerUI();
        
        // Let the normal sanity drain resume
        isRecovering = false; 
    }
}