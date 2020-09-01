using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bolt;

public class VideoURL : MonoBehaviour
{
    public InputField url;

    public void SubmitUrl()
    {
        var urlevent = videoplayer.Create();
        string urltext = url.text.Replace("www", "dl").Replace("dl=0", "dl=1");
        urlevent.url = urltext;
        urlevent.Send();
    }
}
