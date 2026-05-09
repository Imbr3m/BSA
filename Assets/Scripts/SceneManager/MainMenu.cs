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
    [SerializeField] private float waitBeforeFade = 0.5f; 
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
        blackFadeImage.gameObject.SetActive(true); // Ensure it's turned on!

        // 2. Wait right here for half a second!
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
        blackFadeImage.gameObject.SetActive(false); // Turn it off so we can click buttons
    }

    private IEnumerator FadeToBlackAndLoad(int sceneIndex)
    {
        blackFadeImage.gameObject.SetActive(true);
        Color fadeColor = blackFadeImage.color;
        fadeColor.a = 0f;
        blackFadeImage.color = fadeColor;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            blackFadeImage.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 1f;
        blackFadeImage.color = fadeColor;

        SceneManager.LoadSceneAsync(sceneIndex);
    }


    public void PlayGame()
    {
        if (blackFadeImage != null)
        {
            StartCoroutine(FadeToBlackAndLoad(2));
        }
        else
        {
            SceneManager.LoadSceneAsync(2);
        }
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