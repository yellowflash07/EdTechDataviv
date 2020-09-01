using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptManager : MonoBehaviour
{
   // MindWoxGoogleSignIn googleSignIn;
    PlayerBehaviour playerBehaviour;

    bool setonce = false;

    // Start is called before the first frame update
    void Start()
    {
      //  googleSignIn = FindObjectOfType<MindWoxGoogleSignIn>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void signout()
    {
      //  googleSignIn.googlesignout();
    }
}
