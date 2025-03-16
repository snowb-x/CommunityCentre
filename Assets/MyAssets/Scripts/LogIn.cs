using UnityEngine;
using System.Runtime.InteropServices;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;
using TMPro;


public class LogIn : MonoBehaviour
{
    //===========Debug=============
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _textUserInfo;
    //==========INPUT FIELDS TEXT ==============
    [SerializeField] private TMP_InputField _emailInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
 
    /// <summary>
    /// Creates a user in guess mode
    /// </summary>
    /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
    /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
    /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
    [DllImport("__Internal")]
    public static extern void SignInAnonymously(string objectName, string callback, string fallback);
    
    /// <summary>
    /// Sign out the user
    /// </summary>
    /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
    /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
    /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
    [DllImport("__Internal")]
    public static extern void SignOutUser(string objectName, string callback, string fallback);
  
    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Debug.LogError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }
        FirebaseAuth.OnAuthStateChanged(gameObject.name, "DisplayUserInfo","DisplayInfo");
    }
    
    /// <summary>
    /// Login ANONYMOUSLY 
    /// </summary>
    public void SignInAnonymously() =>
        SignInAnonymously(gameObject.name, "OnRequestGuestModeSuccess","OnRequestGuestModeFailed");
    
    /// <summary>
    /// Create new User EMAIL and PASSWORD
    /// </summary>
    public void CreateUserWithEmailAndPassword() =>
        FirebaseAuth.CreateUserWithEmailAndPassword(_emailInputField.text, _passwordInputField.text, gameObject.name, "OnRequestCreateNewUserSuccess", "OnRequestCreateNewUserFailed");
    /// <summary>
    /// LogIn with EMAIL and PASSWORD
    /// </summary>
    public void SignInWithEmailAndPassword() =>
        FirebaseAuth.SignInWithEmailAndPassword(_emailInputField.text, _passwordInputField.text, gameObject.name, "OnRequestSignInSuccess", "OnRequestSignInFailed");
    /// <summary>
    /// Login with GOOGLE
    /// </summary>
    public void SignInWithGoogle() =>
        FirebaseAuth.SignInWithGoogle(gameObject.name, "OnRequestSignInSuccess", "OnRequestSignInFailed");
        
   /// <summary>
   /// Login with FACEBOOK
   /// </summary>
    public void SignInWithFacebook() =>
        FirebaseAuth.SignInWithFacebook(gameObject.name, "OnRequestSignInSuccess", "OnRequestSignInFailed");

   /// <summary>
   /// Sign Out User 
   /// </summary>
    public void SignOutUser() =>
        SignOutUser(gameObject.name, "OnSignOutUserSuccess", "OnSignOutUserFailed");
    
    private void OnRequestCreateNewUserSuccess(string user)
    {
        var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        string data = $"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}";
        GameManager.Instance.UserID = parsedUser.uid;
        User currentUser = JsonUtility.FromJson<User>(data);
        GameManager.Instance.CurrentUser = currentUser;
        _text.color = Color.green;
        _text.text = data;
        Debug.Log("New User Created!");
        GameManager.Instance.InvokeSignIn();
    }

    private void OnRequestCreateNewUserFailed(string error)
    {
        _text.color = Color.red;
        _text.text = error;
        Debug.Log("ERROR: "+error);
        GameManager.Instance.InvokeFailedSignIn();
    } 
    
    private void OnRequestSignInSuccess(string data)
    {
        _text.color = Color.green;
        _textUserInfo.text = data;

        User currentUser = JsonUtility.FromJson<User>(data);
        Debug.Log("User is "+ data);
        GameManager.Instance.UserID = currentUser.uid;
        GameManager.Instance.CurrentUser = currentUser;
        GameManager.Instance.InvokeSignIn();
    }

    private void OnRequestSignInFailed(string error)
    {
        _text.color = Color.red;
        _textUserInfo.text = error;
        DisplayInfo("SignIn Failed error -- "+error);
        GameManager.Instance.InvokeFailedSignIn();
    }

    private void OnRequestGuestModeSuccess()
    {
        Debug.Log("Signed In as Anonymously");
        GameManager.Instance.CurrentUser = new User();
        GameManager.Instance.InvokeSignIn();
    }
    
    private void OnRequestGuestModeFailed()
    {
        Debug.Log("Signed In as Anonymously Failed");
        GameManager.Instance.CurrentUser = null;
        GameManager.Instance.InvokeFailedSignIn();
    }

    private void OnSignOutUserSuccess(string data)
    {
        Debug.Log(data);
        GameManager.Instance.CurrentUser = null;
        GameManager.Instance.InvokeSignOut();
    }
    private void OnSignOutUserFailed(string data)
    {
        Debug.Log(data);
    }
    //TODO: Bug create new user bug. 
    public void DisplayUserInfo(string user)
    {
        var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        DisplayData(
            $"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");
        User currentUser = JsonUtility.FromJson<User>(user);
        GameManager.Instance.CurrentUser = currentUser; // this is not null
        if (currentUser.email == "")
        {
            Debug.Log("No user logged in!!");
            GameManager.Instance.CurrentUser = null;
        }
        else
        {
            GameManager.Instance.InvokeSignIn();
        }
    }
    public void DisplayData(string data)
    {
        _text.color = _text.color == Color.green ? Color.blue : Color.green;
        _text.text = data;
        Debug.Log("Display user data ---- "+data);
    }
    
    public void DisplayInfo(string info)
    {
        _text.color = Color.white;
        _text.text = info;
        Debug.Log("Display information here "+info);
    }
    
    //todo: sign out methods and call back
    
}
