using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FirebaseWebGL.Examples.Utils;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.Objects;

public class MyAvatar
{
    public string name;
    public Color colour;
}


public class CreateAvatar : MonoBehaviour
{
    [SerializeField] private TMP_InputField _textInputFieldName;
    [SerializeField] private TMP_InputField _colourPicker;
    [SerializeField] private TMP_Text _textLog;
    [SerializeField] private String _collectionPath = "avatar";
    [SerializeField] private Slider _colourSliderR;
    private Color _avatarColour = new Color(0,0,0,1);//black
    [SerializeField] 
    
    //Colour RBG values
        private float _red = 0;
        private float _blue = 0;
        private float _green = 0;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            _textLog.text =
                "The code is not running on a WebGL build; as such, the Javascript functions will not be recognized."; };
        _colourSliderR.onValueChanged.AddListener((v) =>
        {
            _red = v;
            _avatarColour = new Color(_red, _blue, _green);
        });
    }

    public void GetDocument() =>
        FirebaseFirestore.GetDocument(_collectionPath, _textInputFieldName.text, gameObject.name, "OnRequestSuccess",
            "OnRequestFailed");

    public void GetDocumentsInCollection() =>
        FirebaseFirestore.GetDocumentsInCollection(_collectionPath, gameObject.name, "OnRequestSuccess",
            "OnRequestFailed");

    public void AddDocument() => FirebaseFirestore.AddDocument(_collectionPath, SetJSONStringValue(),
        gameObject.name,
        "OnRequestSuccess", "OnRequestFailed");

    private void OnRequestSuccess(string data)
    {
        _textLog.color = Color.green;
        _textLog.text = data;
    }

    private void OnRequestFailed(string error)
    {
        _textLog.color = Color.red;
        _textLog.text = error;
    }

    private String SetJSONStringValue()
    {
        MyAvatar newAvatar = new MyAvatar();
        newAvatar.name = _textInputFieldName.text;
        newAvatar.colour = _avatarColour;
        string json = JsonUtility.ToJson(newAvatar);
        return json;
    }
}
