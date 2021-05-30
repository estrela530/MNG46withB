using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSkip : MonoBehaviour
{
    public VideoClip videoClip;
    public GameObject screen;

    // Start is called before the first frame update
    void Start()
    {
        var videoPlayer = screen.GetComponent<VideoPlayer>();

        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = videoClip;

        videoPlayer.isLooping = false;

    }

    // Update is called once per frame
    void Update()
    {
        VPControl();
    }

    void VPControl()
    {
        var videoPlayer = GetComponent<VideoPlayer>();

        if (!videoPlayer.isPlaying)
        {
            videoPlayer.loopPointReached += FinishVideo;
            videoPlayer.Play();
        }
    }

    void FinishVideo(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene("Title");
    }

}
