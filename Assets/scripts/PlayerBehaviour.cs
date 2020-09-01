using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Bolt;

public class PlayerBehaviour : EntityBehaviour<IPlayerState>
{
    public float speed;
    [HideInInspector]
    public bool right = false, left = false, forward = false, back = false,kine = false;
    public bool rotRight = false, rotLeft = false, sit = false,ask = false;

    public GameObject promptWindow, exitPrompt, sitPrompt, standPrompt, playPrompt,ControlCanvas;

    [HideInInspector]
    public GameObject sitAnchor,standAnchor;
    public static string scenename;

    public Text debugText;
    VideoStreaming videoStreaming;
 
    //  MindWoxGoogleSignIn googleSignIn;


    public override void Attached()
    {
       // googleSignIn = FindObjectOfType<MindWoxGoogleSignIn>();

        //update transforms over network
        state.SetTransforms(state.PlayerTransform, gameObject.transform);
        if (entity.IsOwner)
        {
            //enable camera and controls 
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            ControlCanvas.SetActive(true);
            //  state.Username = googleSignIn.username;
            state.isKinematic = false;
        }

      //  state.AddCallback("Username", AddUser);
        state.SetAnimator(GetComponent<Animator>());
        state.Animator.applyRootMotion = entity.IsOwner;
        state.OnPlayVideo = playVideo;
    }

    private void Update()
    {
        GetComponent<Rigidbody>().isKinematic = state.isKinematic;
        if (VideoStreaming.playvideoNow)
        {
            state.PlayVideo();
            VideoStreaming.playvideoNow = false;
        }
    }

    public override void SimulateOwner()
    {
        if (entity.IsOwner)
        {
            //get input from buttons
            if (right) { state.MoveInput = -1;}
            else if (left) { state.MoveInput = 1;}
            else if (forward) { state.MoveInputFor = 1; state.movement = 1;}
            else if (back) { state.MoveInputFor = -1; state.backmove = 1;}

            if (!right && !left) { state.MoveInput = 0;}
            if (!forward) { state.MoveInputFor = 0; state.movement = -1;}
            if (!back) { state.MoveInputFor = 0; state.backmove = -1;}

            if (sit)
            {
                state.sit = 1;
                standPrompt.SetActive(true);
            }
            if (!sit)
            {
                state.sit = -1;
                standPrompt.SetActive(false);
            }

            if (ask)
            {
                state.ask = true;
                StartCoroutine(ResetAnim());
                standPrompt.SetActive(false);
            }
            if (!ask)
            {
                state.ask = false;
            }

            if (state.MoveInput != 0)
            {
                transform.Translate(Vector3.right * state.MoveInput * speed * BoltNetwork.FrameDeltaTime);
            }
            if (state.MoveInputFor != 0)
            {
                transform.Translate(Vector3.forward* state.MoveInputFor* speed *BoltNetwork.FrameDeltaTime);
            }
            Debug.Log($"{right},{left},{back},{forward},{state.MoveInput}");

            if (rotLeft) { state.RotInput = -1; }
            else if (rotRight) { state.RotInput = 1; }

            if (!rotRight && !rotLeft) { state.RotInput = 0; }

            if (state.RotInput != 0)
            {
                gameObject.transform.Rotate(Vector3.up * state.RotInput * 50 * BoltNetwork.FrameDeltaTime);
            }            

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.tag == "Door")
        {
            promptWindow.SetActive(true);
            promptWindow.transform.GetChild(0).GetComponent<Text>().text = "Enter " + other.name + " class";
            scenename = other.name;
            Debug.Log("Generated prompt window");
        }

        if (other.tag == "Exit")
        {
            exitPrompt.SetActive(true);
            scenename = other.name;
            exitPrompt.transform.GetChild(0).GetComponent<Text>().text = "Exit to lobby";
        }

        if (other.tag == "Chair")
        {
            sitPrompt.SetActive(true);
            sitPrompt.transform.GetChild(0).GetComponent<Text>().text = "Sit";
            sitAnchor = other.gameObject.transform.GetChild(0).gameObject;
            standAnchor = other.gameObject.transform.GetChild(1).gameObject;
            Debug.Log(standAnchor.name +" "+ standAnchor.transform.position);
        }

        if (other.tag == "Board")
        {
            playPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Door")
        {
            if (promptWindow.activeInHierarchy)
            {
                promptWindow.SetActive(false);
            }
        }

        if (other.tag == "Exit")
        {
            if (promptWindow.activeInHierarchy)
            {
                exitPrompt.SetActive(false);
            }
        }

        if (other.tag == "Chair")
        {
            if (sitPrompt.activeInHierarchy)
            {
                sitPrompt.SetActive(false);
            }
        }

        if (other.tag == "Board")
        {
            playPrompt.SetActive(false);
        }
    }

    IEnumerator ResetAnim()
    {
        yield return new WaitForSeconds(5f);
        standPrompt.SetActive(true);
        ask = false;
    }


    //BUTTON CALLS
    public void LoadScene()
    {
        string classname = scenename + "Class";
        gameObject.transform.position = GameObject.Find(classname).transform.GetChild(0).transform.position;

        if (entity.IsOwner)
        {
        }
    }

    public void MoveForward(bool f)
    {
        if (entity.IsOwner)
        {
            forward = f; 
        }
    }

    public void MoveBack(bool b)
    {
        if (entity.IsOwner)
        {
            back = b; 
        }
    }

    public void RotateRight(bool r)
    {
        if (entity.IsOwner)
        {
            rotRight = r; 
        }
    }

    public void RotateLeft(bool l)
    {
        if (entity.IsOwner)
        {
            rotLeft = l; 
        }
    }

    public void SitDown()
    {
        if (entity.IsOwner)
        {
            sit = true;
            gameObject.transform.position = sitAnchor.transform.position;
            gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
            state.isKinematic = true;
        }
    }

    public void StandUp()
    {
        if (entity.IsOwner)
        {
            sit = false;
            gameObject.transform.position = standAnchor.transform.position;
            state.isKinematic = false;
        }
    }

    public void Ask()
    {
        if (entity.IsOwner)
        {
            ask = true; 
        }
    }

    public void TriggerPlayVideo()
    {
        var playevent = PlayVideoEvent.Create();
        playevent.playNow = true;
        playevent.Send();
      //  videoStreaming = FindObjectOfType<VideoStreaming>();
       // videoStreaming.UpdateFrame();
    }

    public void playVideo()
    {
        videoStreaming = FindObjectOfType<VideoStreaming>();
        videoStreaming.UpdateFrame();
        debugText.text = "Playing Video";
    }

    void AddUser()
    {
        //gameObject.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = state.Username;
    }
}


