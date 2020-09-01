using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bolt;

[BoltGlobalBehaviour]
public class GameManager : GlobalEventListener
{
    GameObject player;
    bool instOnce = false;

    private void Start()
    {

    }

    public override void SceneLoadLocalBegin(string scene)
    {
        var spawnpos = new Vector3(Random.Range(1.8f, -1.8f), 0.5f, Random.Range(1.8f, -1.8f));
        player = BoltNetwork.Instantiate(BoltPrefabs.remy, spawnpos, Quaternion.identity);

    }

    public override void OnEvent(videoplayer evnt)
    {
        VideoStreaming.url = evnt.url;
    }

    public override void OnEvent(PlayVideoEvent evnt)
    {
        Debug.Log(evnt.playNow);
        if (evnt.playNow)
        {
            VideoStreaming.playvideoNow = true;
            evnt.playNow = false;
        }
        Debug.Log(evnt.playNow);
    }
}
