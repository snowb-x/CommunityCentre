using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirebaseWebGL.Scripts.FirebaseBridge;
using TMPro;

/*
 * JSON => objects class list
 * Read '2. MULTIPLE DATA(ARRAY JSON)' & 'If this is a Json array from the server and you did not create it by hand:'
 * https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity
 * How does wrappers works?
 * https://stackoverflow.com/questions/47652604/how-does-a-wrapper-work-for-jsonutility
 * 
 */



class Population
{
    public MyAvatar[] population;
    public string toString()
    {
        string printString = "";
        foreach (var a in this.population)
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
    [SerializeField] private TMP_Text _textLog;
    private Population _avatarPopulation;//wrapper 

    private void Start()
    {
        _collectionPath = GameManager.Instance.DataBaseCollectionPath;
        LoadPopulation();
    }

    //GET Request to get all the avatar in the data base. returned as a json file
    public void GetDocumentsInCollection() =>
        FirebaseFirestore.GetDocumentsInCollection(_collectionPath, gameObject.name, "OnGetDocumentsRequestSuccess",
            "OnGetDocumentsRequestFailed");

    private void LoadPopulation()
    {
        GetDocumentsInCollection();
    }
    
    private void OnGetDocumentsRequestSuccess(string data)
    {
        _textLog.color = Color.green;
        //_textLog.text = data;
        _allPopulationAvatarsJSON = "{\"population\":" + data + "}";
        _textLog.text = _allPopulationAvatarsJSON;
        Population pop = new Population();
        pop = FromJson(_allPopulationAvatarsJSON);
        int count = pop.population.Length;
        _textLog.text = pop.population[0].toString();
    }
    private static Population FromJson (string json)
    {
        Population theAvatars = JsonUtility.FromJson<Population>(json);
        return theAvatars;
    }
    
    private void OnGetDocumentsRequestFailed(string data)
    {
        _textLog.color = Color.red;
        _textLog.text = data;
    }
}
