using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Google;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;


//Import GoogleSignIn package
//Import FirebaseSDK
public class MindWoxGoogleSignIn : MonoBehaviour
{

    GoogleSignInConfiguration configuration;

    //Generate OAuth client ID from console.developers.google.com
    public string clientID;
    public Text debtext;
    public Text fbdebtext;
    public string sceneToLoad;

    [HideInInspector]
    public string username;

    //Firebase for database and analytics
    Firebase.Auth.FirebaseAuth auth;
    // Start is called before the first frame update

    bool signedIn = false, alreadySignedIn = false;
    BinaryFormatter binaryFormatter = new BinaryFormatter();
  
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        configuration = new GoogleSignInConfiguration()
        {
            WebClientId = clientID,
            RequestIdToken = true
        };

        // dataManager = FindObjectOfType<UserDataManager>();

        //check for data
        Debug.Log(Application.persistentDataPath);
        if (File.Exists(Application.persistentDataPath + "/userdata.data"))
        {
            googlesignin();
            //alreadySignedIn = true;
            //LoadUserData();
            Debug.Log("Pre existing user..");
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(username);
        if (signedIn || alreadySignedIn)
        {
            Debug.Log("Loading Scene..");
            SceneManager.LoadScene(sceneToLoad);
            signedIn = false;
            alreadySignedIn = false;
        }
    }

    /// <summary>
    /// For signing in
    /// </summary>
    public void googlesignin()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestProfile = true;
        GoogleSignIn.Configuration.RequestEmail = true;
        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }


    /// <summary>
    /// For signing out
    /// </summary>
    public void googlesignout()
    {
        GoogleSignIn.DefaultInstance.SignOut();
        Debug.Log("Signed Out");
    }

    /// <summary>
    /// called in googlesignin() after to check authentication success
    /// </summary>
    /// <param name="task"> Contains all the output returned by google</param>
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }

        else
        {
            //signin
            debtext.text = "Welcome: " + task.Result.DisplayName + "!";
            Debug.Log("Hello!" + task.Result.DisplayName);
            username = task.Result.DisplayName;
            signedIn = true;
            SaveUserData();
        }
        
        Firebase.Auth.Credential credential =
        Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
        auth.SignInWithCredentialAsync(credential).ContinueWith(fbtask =>
        {
            if (fbtask.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (fbtask.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = fbtask.Result;
            fbdebtext.text = ("User signed in successfully:" + newUser.DisplayName + newUser.UserId);
            Debug.Log("Logged firebase:" + newUser.DisplayName + newUser.UserId);
        });
    }

    public void SaveUserData()
    {
        FileStream writer = File.Create(Application.persistentDataPath + "/userdata.data");
        UserData data = new UserData();
        data.username = username;
        binaryFormatter.Serialize(writer, username);
        writer.Close();
        Debug.Log("Data Saved!");
    }

    //public void LoadUserData()
    //{
    //    if (File.Exists(Application.persistentDataPath + "/userdata.data"))
    //    {
    //        FileStream savedata = File.Open(Application.dataPath + "/userdata.data", FileMode.Open);
    //        UserData data = (UserData)binaryFormatter.Deserialize(savedata);
    //        savedata.Close();
    //        username = data.username;
    //        Debug.Log("Retrieved:" + username);

    //    }
    //}
}

class UserData
{
    public string username;   
}

