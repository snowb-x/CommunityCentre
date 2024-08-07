using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using Unity.VisualScripting;

/*
 * JSON => objects class list
 * JsonUtility Docs IMPORTANT, Need to added [Serializable] to the object class/struct.
 * https://docs.unity3d.com/2020.3/Documentation/Manual/JSONSerialization.html
 * Read '2. MULTIPLE DATA(ARRAY JSON)' & 'If this is a Json array from the server and you did not create it by hand:'
 * https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
 * How does wrappers works?
 * https://stackoverflow.com/questions/47652604/how-does-a-wrapper-work-for-jsonutility
 * 
 */

/// <summary>
/// MyAvatar array Wrapper so I can parse from Json to class objects
/// JSON = {"Items":[{MyAvatarObj},{"name":"Steve","Colour":{"r":0.1f,"g":0.2f,"b":0.3f,"a":1.0f},"spriteId":0,"userID":"noUser"},...]}
/// </summary>
[Serializable]//Important to add this so the JsonUtility can serialize objects with this class
class Population
{
    public MyAvatar[] Items;
    
    public string toString()
    {
        string printString = "";
        foreach (var a in this.Items)
        {
            printString += "\n" + a.toString();
        }
        return printString;
    }
}

public class LoadCommunity : MonoBehaviour
{
    private string _collectionPath;
    private string _allPopulationAvatarsJSON;
    private string _testJsonData;
    [SerializeField] private TMP_Text _textLog;
    [SerializeField] private TMP_InputField _inputFieldDebug;
    private Population _avatarPopulation;//wrapper 
    

    private void Start()
    {
        _avatarPopulation = new Population();
        _collectionPath = GameManager.Instance.DataBaseCollectionPath;
        StartCoroutine(LoadPopulation());
    }

    //GET Request to get all the avatar in the data base. returned as a json file
    public void GetDocumentsInCollection() =>
        FirebaseFirestore.GetDocumentsInCollection(_collectionPath, gameObject.name, "OnGetDocumentsRequestSuccess",
            "OnGetDocumentsRequestFailed");
    
    private void OnGetDocumentsRequestSuccess(string data)
    {
        _textLog.color = Color.green;
        //_textLog.text = data;
        _allPopulationAvatarsJSON = "{\"Items\":" + data + "}";
    }
    private MyAvatar[] FromJson (string json)
    {
        Population theAvatars = new Population();
        theAvatars = JsonUtility.FromJson<Population>(json);
        //JsonUtility.FromJsonOverwrite(json, theAvatars);
        return theAvatars.Items;
    }
    
    private void OnGetDocumentsRequestFailed(string data)
    {
        _textLog.color = Color.red;
        _textLog.text = data;
    }

    private void LoadJsonDataToObject(string data)
    {
        _textLog.text = _allPopulationAvatarsJSON;
        //_avatarPopulation.Items = FromJson(data);
    }

    public void DebugPrintOneAvatar()
    {
        int id = 0;
        int.TryParse(_inputFieldDebug.text, out id);
        foreach (var item in _avatarPopulation.Items)
        {
            _textLog.text += "\n The avatar name is : "+item.name;
        }
    }

    /// <summary>
    /// DEBUG button for the json data parse to and from JSON <--> Objects
    /// </summary>
    /// <param name="id">The index or ID of the avatar name you want to print</param>
    public void GetAvatar()
    {
        int id = 0;
        if (_inputFieldDebug.text != "")
        {
            int.TryParse(_inputFieldDebug.text, out id);
        }
        _textLog.color = Color.yellow;
        //MyAvatar[] pop = null;
        MyAvatar[] here = null;
        MyAvatar a = new MyAvatar();
        MyAvatar b = new MyAvatar();
        a.name = "steve";
        a.colour = Color.yellow;
        a.spriteId = 0;
        a.userID = "noUser";
        b.name = "Mary";
        b.colour = Color.magenta;
        b.spriteId = 1;
        b.userID = "noUser";
        Population pop = new Population();
        pop.Items = new MyAvatar[2];
        pop.Items[0] = a;
        pop.Items[1] = b;
        string sampleDataJson = JsonUtility.ToJson(pop);
        _testJsonData = sampleDataJson;
        here = FromJson(_testJsonData);
        _inputFieldDebug.text = _allPopulationAvatarsJSON;
        _avatarPopulation = JsonUtility.FromJson<Population>(_allPopulationAvatarsJSON);
        if (here == null)
        {
            _textLog.text = "array is null";
        }
        else
        {
            //_textLog.text = "array is not null";
            _textLog.text = "array is not null "+here[0].name +"  ---- " +_allPopulationAvatarsJSON;
        }
    }

    IEnumerator LoadPopulation()
    {
        GetDocumentsInCollection();
        yield return new WaitForSeconds(1f);
        LoadJsonDataToObject(_allPopulationAvatarsJSON);
    }
}
