using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;


public class LogIn : MonoBehaviour
{

    [SerializeField] private TMP_Text _text;
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
        SignInAnonymously(gameObject.name, "OnRequestSuccess","OnRequestFailed");
    }
    
    private void OnRequestSuccess(string data)
    {
        _text.color = Color.green;
        _text.text = data;
    }

    private void OnRequestFailed(string error)
    {
        _text.color = Color.red;
        _text.text = error;
    }
}
