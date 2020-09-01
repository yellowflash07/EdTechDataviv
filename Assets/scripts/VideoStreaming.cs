using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Bolt;

public class VideoStreaming : EntityBehaviour<IPlayerState>
{

    VideoPlayer vp;
    public static string url;
    public static bool playvideoNow = false;

    private void Start()
    {
        vp = GetComponent<VideoPlayer>();
    }


    public void Update()
    {
    //    if (playvideoNow)
    //    {
    //        state.PlayVideo();
    //        //UpdateFrame();
    //        playvideoNow = false;
    //    }
    }

    public void UpdateFrame()
    {
        Debug.Log("Playing video..");
        vp.url = url;
        vp.Play();
    }

}
