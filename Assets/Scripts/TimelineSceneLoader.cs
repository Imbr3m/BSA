using UnityEngine;
using UnityEngine.SceneManagement; // You need this line to load scenes!

public class TimelineSceneLoader : MonoBehaviour
{
    [Header("Where are we going?")]
    public string sceneToLoad;

    // The Timeline Signal will press this exact "button"
    public void LoadNextScene()
    {
        Debug.Log("Timeline is loading scene: " + sceneToLoad);
        SceneManager.LoadScene(sceneToLoad);
    }
}