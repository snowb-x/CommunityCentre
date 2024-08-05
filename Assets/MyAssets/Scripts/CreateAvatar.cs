using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FirebaseWebGL.Scripts.FirebaseBridge;
using UnityEngine.SceneManagement;


//Avatar struct object to be converted to JSON and post to database.



public class CreateAvatar : MonoBehaviour
{
    [Header ("Avatar")]
    //AVATAR Sprite variables
    [SerializeField] private Image _avatarSprite;//the sprite/image display in creator screen
    [SerializeField] private int _spriteID = 0; //the ID of the sprite the player chooses to use
    [SerializeField] private Sprite[] _avatarSpriteList;
    
    [Header("Field Text Input")]
    //FORM SECTION variables
    [SerializeField] private TMP_InputField _textInputFieldName;// holds the player's created avatar name
    [SerializeField] private TMP_InputField _colourPicker;//?????? not used, can be repurposed
    [SerializeField] private TMP_Text _textLog;//debug log to game screen
   
    [Header("Data Base Path")]
    [SerializeField] private String _collectionPath = "avatar";// firebase firestore collection path name
    
    [Header("Colour Picker")]
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

        GameManager.Instance.DataBaseCollectionPath = _collectionPath;
        _avatarSpriteList = GameManager.Instance.AvatarSpriteList;
        SetSpriteColour();
        //COLOUR PICKER SLIDERS LISTENER
        SetSliderListeners();
    }

    // GET and ADD Request to Firebase firestore database methods calling plugIn methods in firebase package.

    public void AddDocument() => FirebaseFirestore.AddDocument(_collectionPath, SetJSONStringValue(),
        gameObject.name,
        "OnRequestSuccess", "OnRequestFailed");

    private void OnRequestSuccess(string data)
    {
        _textLog.color = Color.green;
        _textLog.text = data;
        LoadScene("CommuntityCentre");
    }

    private void OnRequestFailed(string error)
    {
        _textLog.color = Color.red;
        _textLog.text = error;
    }
    
    private void PrintMessageOnScreen(string msg)
    {
        _textLog.color = Color.yellow;
        _textLog.text = msg;
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

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    //COLOUR PICKER--------------------------------
    private void SetSpriteColour()
    {
        _avatarColour = new Color(_red, _green, _blue,1.0f);// values RGB 0 -> 1.0f, Alpha = 1.0f
        _avatarSprite.color = _avatarColour;
        PrintMessageOnScreen(_avatarColour.ToString());
        GameManager.Instance.UserColour = _avatarColour;
    }

    private void SetSliderListeners()
    {
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
    
    //AVATAR sprite picker-------------------------
    // ToDo: sprite picker and tint with avatar colour
    public void SetSpriteID(int id)
    {
        _spriteID = id;
        GameManager.Instance.UserSpriteID = id;
        _avatarSprite.sprite = _avatarSpriteList[id];
        PrintMessageOnScreen("sprite ID is: "+ id);
    }
}
