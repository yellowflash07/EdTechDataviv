using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using System;

public class Menu : GlobalEventListener
{
    public InputField matchName;
    public Text usernameText;

   // MindWoxGoogleSignIn googleSignIn;
    private void Start()
    {
       // googleSignIn = FindObjectOfType<MindWoxGoogleSignIn>();
    }

    private void Update()
    {
       // usernameText.text = "Welcome," + googleSignIn.username;
    }

    public void StartServer()
    {
        BoltLauncher.StartServer();
    }

    public void StartClient()
    {
        BoltLauncher.StartClient();
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            BoltMatchmaking.CreateSession(sessionID: matchName.text, sceneToLoad: "Zoom");
        }
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            if (photonSession.Source == UdpSessionSource.Photon)
            {
                BoltMatchmaking.JoinSession(sessionID: matchName.text);
            }
        }
    }
}
