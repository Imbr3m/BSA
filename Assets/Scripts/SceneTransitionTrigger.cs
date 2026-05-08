using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required to load levels!
using UnityEngine.UI; // Required to talk to your black image

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Destination")]
    [Tooltip("The exact name of the scene you want to load (e.g., 'Level_2')")]
    public string sceneToLoad;

    [Header("Fade Settings")]
    public Image fadeScreen; 
    public float fadeDuration = 1f;

    private bool isTransitioning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) player.isAiming = true; 

            StartCoroutine(FadeAndLoad());
        }
    }

    private IEnumerator FadeAndLoad()
    {
        isTransitioning = true;

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
                fadeColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
                fadeScreen.color = fadeColor;
                yield return null; 
            }

            fadeColor.a = 1f;
            fadeScreen.color = fadeColor;
        }

        // Load the next level!
        SceneManager.LoadScene(sceneToLoad);
    }
}