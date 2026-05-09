using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Destination")]
    [Tooltip("The exact name of the scene you want to load (e.g., 'Level_2')")]
    public string sceneToLoad;

    [Header("Fade Settings")]
    public Image fadeScreen; 
    public float fadeDuration = 1f;

    [Header("Audio Settings")]
    [Tooltip("Optional: Drop a sound here to play when the transition starts!")]
    public AudioClip transitionSound;
    
    [Tooltip("Drop your background music or ambient AudioSource here to fade it out")]
    public AudioSource audioToFade; 

    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) player.isAiming = true; 

            if (transitionSound != null && SoundFXManager.instance != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(transitionSound, transform, 1f);
            }

            StartCoroutine(FadeAndLoad());
        }
    }

    private IEnumerator FadeAndLoad()
    {
        isTransitioning = true;

        float startVolume = 0f;
        if (audioToFade != null) 
        {
            startVolume = audioToFade.volume;
        }

        if (fadeScreen != null)
        {
            fadeScreen.gameObject.SetActive(true);
            Color fadeColor = fadeScreen.color;
            fadeColor.a = 0f; 
            fadeScreen.color = fadeColor;

            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                
                float fadeProgress = elapsedTime / fadeDuration;

                fadeColor.a = Mathf.Clamp01(fadeProgress);
                fadeScreen.color = fadeColor;

                if (audioToFade != null)
                {
                    audioToFade.volume = Mathf.Lerp(startVolume, 0f, fadeProgress);
                }

                yield return null; 
            }

            fadeColor.a = 1f;
            fadeScreen.color = fadeColor;
            
            if (audioToFade != null) audioToFade.volume = 0f;
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}