using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class MainMenu : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private Image blackFadeImage;
    [Tooltip("How long to stay pitch black before fading in")]
    [SerializeField] private float waitBeforeFade = 0.5f; // NEW: The delay timer!
    [SerializeField] private float fadeDuration = 1.5f;

    private void Start()
    {
        if (blackFadeImage != null)
        {
            StartCoroutine(FadeFromBlack());
        }
    }

    private IEnumerator FadeFromBlack()
    {
        Color fadeColor = blackFadeImage.color;
        
        // 1. Ensure it starts pitch black
        fadeColor.a = 1f; 
        blackFadeImage.color = fadeColor;

        // 2. NEW: Wait right here for half a second!
        yield return new WaitForSeconds(waitBeforeFade);

        // 3. Now start the smooth fade to reveal the menu
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            blackFadeImage.color = fadeColor;
            yield return null;
        }

        // Lock in the final transparent color
        fadeColor.a = 0f;
        blackFadeImage.color = fadeColor;
        blackFadeImage.gameObject.SetActive(false);
    }

    // --- Your original button functions below[cite: 28] ---

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void OpenCredits()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=uW3BXm2eQ6M");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT GAME TRIGGERED!"); 
        
        Application.Quit();
    }
}