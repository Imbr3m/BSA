using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class IntroManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    // Update is called once per frame
    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
