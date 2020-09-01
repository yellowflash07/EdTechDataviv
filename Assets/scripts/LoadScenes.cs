using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    PlayerBehaviour playerBehaviour;
    VideoStreaming videoStreaming;

    private void Start()
    {
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();      
    }


}
