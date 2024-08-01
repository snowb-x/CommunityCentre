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


//Avatar struct object to be converted to JSON and post to database.
public class MyAvatar
{
    public string name;
    public Color colour;
    public int spriteId;
    public string userID;
}


public class CreateAvatar : MonoBehaviour
{
    //AVATAR Sprite variables
    [SerializeField] private Image _avatarSprite;//the sprite/image display in creator screen
    
    //FORM SECTION variables
    [SerializeField] private TMP_InputField _textInputFieldName;// holds the player's created avatar name
    [SerializeField] private TMP_InputField _colourPicker;//?????? not used, can be repurposed
    [SerializeField] private TMP_Text _textLog;//debug log to game screen
    [SerializeField] private String _collectionPath = "avatar";// firebase firestore collection path name
    [SerializeField] private int _spriteID; //the ID of the sprite the player chooses to use
   
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

    // GET and ADD Request to Firebase firestore database methods calling plugIn methods in firebase package.
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
        newAvatar.spriteId = _spriteID;
        newAvatar.userID = GameManager.Instance.UserID;
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
    
    //AVATAR sprite picker-------------------------
    // ToDo: sprite picker and tint with avatar colour
}
