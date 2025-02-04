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
    /// Creates a user with email and password
    /// </summary>
    /// <param name="objectName"> Name of the gameobject to call the callback/fallback of </param>
    /// <param name="callback"> Name of the method to call when the operation was successful. Method must have signature: void Method(string output) </param>
    /// <param name="fallback"> Name of the method to call when the operation was unsuccessful. Method must have signature: void Method(string output). Will return a serialized FirebaseError object </param>
    [DllImport("__Internal")]
    public static extern void SignInAnonymously(string objectName, string callback, string fallback);
  private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            Debug.LogError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            return;
        }
        SignInAnonymously(gameObject.name, "OnRequestLogInSuccess","OnRequestLogInFailed");
        FirebaseAuth.OnAuthStateChanged(gameObject.name, "OnRequestSignInSuccess","OnRequestSignInFailed");
    }
    
    /// <summary>
    /// Create new User EMAIL and PASSWORD
    /// </summary>
    public void CreateUserWithEmailAndPassword() =>
        FirebaseAuth.CreateUserWithEmailAndPassword(_emailInputField.text, _passwordInputField.text, gameObject.name, "OnRequestCreateNewUserSuccess", "OnRequestCreateNewUserFailed");
    /// <summary>
    /// LogIn with EMAIL and PASSWORD
    /// </summary>
    public void SignInWithEmailAndPassword() =>
        FirebaseAuth.SignInWithEmailAndPassword(_emailInputField.text, _passwordInputField.text, gameObject.name, "OnRequestCreateNewUserSuccess", "OnRequestCreateNewUserFailed");
 
    private void OnRequestLogInSuccess(string data)
    {
        _text.color = Color.green;
        _text.text = data;
    }

    private void OnRequestLogInFailed(string error)
    {
        _text.color = Color.red;
        _text.text = error;
    }
    
    private void OnRequestCreateNewUserSuccess(string user)
    {
        var parsedUser = StringSerializationAPI.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        string data = $"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}";
        GameManager.Instance.UserID = parsedUser.uid;
        _text.color = Color.green;
        _text.text = data;
    }

    private void OnRequestCreateNewUserFailed(string error)
    {
        _text.color = Color.red;
        _text.text = error;
        Debug.Log("ERROR: "+error);
    } 
    
    private void OnRequestSignInSuccess(string data)
    {
        _text.color = Color.green;
        _textUserInfo.text = data;

        User currentUser = JsonUtility.FromJson<User>(data);
        GameManager.Instance.UserID = currentUser.uid;
        
        _textUserInfo.text = currentUser.uid;
    }

    private void OnRequestSignInFailed(string error)
    {
        _text.color = Color.red;
        _textUserInfo.text = error;
    }
    
}
