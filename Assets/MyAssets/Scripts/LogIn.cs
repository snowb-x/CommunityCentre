using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;


public class LogIn : MonoBehaviour
{

    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _textUserInfo;
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
