using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

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
    private string _allPopulationAvatarsJSON = "";
    private string _testJsonData;
    [SerializeField] private TMP_Text _textLog;
    [SerializeField] private TMP_Text _textInfo;
    [SerializeField] private TMP_InputField _inputFieldDebug;
    private Population _avatarPopulation;//wrapper
    [SerializeField] private Peep _peepPrefab;//the prefab for avatars in the data base
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _spawnOffset = 2.0f; // offset max, used to randomise the spawn location
    [SerializeField] private Sprite[] _listOfSprites; 
    [SerializeField] private Sprite[] _listOfSpritesDebug;
    [SerializeField] private string _gameInfoTextFormat = "Walk WASD/Arrows \n Population count: {0} / {1}";
    private int _spawnCount=0;
    private void Start()
    {
        _avatarPopulation = new Population();
        _avatarPopulation.Items = null;
        _collectionPath = GameManager.Instance.DataBaseCollectionPath;
        _listOfSprites = GameManager.Instance.AvatarSpriteList;
        StartCoroutine(LoadPopulation());
        _textInfo.text = string.Format(_gameInfoTextFormat, _spawnCount,0);
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
        //theAvatars = JsonUtility.FromJson<Population>(json);
        JsonUtility.FromJsonOverwrite(json, theAvatars);
        return theAvatars.Items;
    }
    
    private void OnGetDocumentsRequestFailed(string data)
    {
        _textLog.color = Color.red;
        _textLog.text = data;
    }

    private bool doneLoadingObject = false;

    private void LoadJsonDataToObject(string data)
    {
        _textLog.text = _allPopulationAvatarsJSON;
        _avatarPopulation.Items = FromJson(data);
        doneLoadingObject = true;
    }
    
    
    /// <summary>
    /// Load the population first but getting the json string from the firestore database
    /// then Parse the JSON to unity Object array
    /// next Instantiate all the avatars except the player
    /// </summary>
    /// <returns>load and instantiate the avatar population</returns>
    IEnumerator LoadPopulation()
    {
        GetDocumentsInCollection(); //set the _allPopulationAvatarsJSON string with the data from the database. Data has been JSON.stringify
        yield return new WaitUntil(()=>_allPopulationAvatarsJSON != "");
        LoadJsonDataToObject(_allPopulationAvatarsJSON); //set the _avatarPopulation [the wrapper] with the MyAvatar[] = Items, convert json to object
        yield return new WaitUntil(()=>doneLoadingObject);
        StartCoroutine(InstantiateAvatarPopulation(_avatarPopulation.Items));
    }
    float timePast = 0.0f;
    float maxWaitTime = 10.0f;//sec
    private bool breakFree = false;

    IEnumerator InstantiateAvatarPopulation(MyAvatar[] avatarPopulationData)
    {
        StartCoroutine(timer());
        int count = _spawnCount;
        _textLog.text = avatarPopulationData.Length.ToString();
        _inputFieldDebug.text = avatarPopulationData.Length.ToString();
        
        foreach (var avatar in avatarPopulationData)
        {
            timePast = 0.0f;
            yield return new WaitForSeconds(0.3f);
            if (GameManager.Instance.UserID != avatar.userID)
            {
                _spawnCount= ++count;
                _inputFieldDebug.text = count.ToString();
                _textInfo.text = string.Format(_gameInfoTextFormat, count, avatarPopulationData.Length);
                InstantiateOneAvatar(avatar);   
            }
            
        }

        StopCoroutine(timer());
    }

    IEnumerator timer()
    {
        while (timePast<maxWaitTime)
        {
            timePast += Time.deltaTime*1.0f;
        }
        yield return new WaitUntil(()=>timePast > maxWaitTime);
        
        if (_spawnCount != _avatarPopulation.Items.Length)
        {
            StopCoroutine(InstantiateAvatarPopulation(_avatarPopulation.Items));

            for (int i = _spawnCount-1; i < _avatarPopulation.Items.Length; i++)
            {
                _spawnCount++;
                _textInfo.text = string.Format(_gameInfoTextFormat, _spawnCount, _avatarPopulation.Items.Length);
                InstantiateOneAvatar(_avatarPopulation.Items[i]);                
            }
        }
    }

   
    private void InstantiateOneAvatar(MyAvatar avatar)
    {
        Peep aPeep = Instantiate(_peepPrefab);
        aPeep.SetSprite(avatar.spriteId,_listOfSprites[avatar.spriteId],avatar.colour);
        aPeep.SetParent(gameObject);
        Vector3 spawnLocation = _spawnPoint.transform.position + new Vector3(Random.Range(-1*_spawnOffset,_spawnOffset),0,Random.Range(-1*_spawnOffset,_spawnOffset));
        aPeep.SetLocation(spawnLocation);
        aPeep.SetNameOfObject(avatar.name);//must SetSprite() before SetNameOfObject, to set the peep's spriteID, to determine the name height;
    }
    
    //----------------------------------------------------------------------------
   // DEBUG STUFF
    
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
    /// Left in code for future debug if needed.
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

    public void LoadSamplePopulation()
    {
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
        _avatarPopulation.Items = new[] { a, b };
        _listOfSprites = _listOfSpritesDebug;
        StartCoroutine(InstantiateAvatarPopulation(_avatarPopulation.Items));
    }
}
