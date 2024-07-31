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
    public int spriteId;
}


public class CreateAvatar : MonoBehaviour
{
    //AVATAR Sprite variables
    [SerializeField] private Image _avatarSprite;
    
    //FORM SECTION variables
    [SerializeField] private TMP_InputField _textInputFieldName;
    [SerializeField] private TMP_InputField _colourPicker;//??????
    [SerializeField] private TMP_Text _textLog;
    [SerializeField] private String _collectionPath = "avatar";
   
    // COLOUR PICKER variables 
    [SerializeField] private Slider _colourSliderR;
    [SerializeField] private Slider _colourSliderG;
    [SerializeField] private Slider _colourSliderB;
    private Color _avatarColour = new Color(0,0,0,1);//black
    [SerializeField] private TMP_Text _textRed; 
    [SerializeField] private TMP_Text _textGreen;
    [SerializeField] private TMP_Text _textBlue;
    //Colour RBG values
    private float _red = 0;
    private float _green = 0;
    private float _blue = 0;

    private void Start()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            _textLog.text =
                "The code is not running on a WebGL build; as such, the Javascript functions will not be recognized."; };

        SetSpriteColour();
        //COLOUR PICKER SLIDERS LISTENER
        _colourSliderR.onValueChanged.AddListener((v) =>
        {
            _red = v;
            _textRed.text = _red.ToString();
            SetSpriteColour();
        });
        _colourSliderG.onValueChanged.AddListener((v) =>
        {
            _green = v;
            _textGreen.text = _green.ToString();
            SetSpriteColour();
        });
        _colourSliderB.onValueChanged.AddListener((v) =>
        {
            _blue = v;
            _textBlue.text = _blue.ToString();
            SetSpriteColour();
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
        newAvatar.spriteId = 0;
        string json = JsonUtility.ToJson(newAvatar);
        return json;
    }
    
    //COLOUR PICKER--------------------------------
    private void SetSpriteColour()
    {
        _avatarColour = new Color(_red, _green, _blue,1.0f);
        _avatarSprite.color = _avatarColour;
        OnRequestSuccess(_avatarColour.ToString());
    }
}
