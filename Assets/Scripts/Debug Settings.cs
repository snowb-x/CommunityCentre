using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugSettings : MonoBehaviour
{
    //Text game Objects
    [Header("Text Game Objects")] 
    [SerializeField] private TMP_Text _textOnScreenLog;
    [SerializeField]private TMP_Text _textColourPickerRed;    
    [SerializeField]private TMP_Text _textColourPickerGreen;
    [SerializeField] private TMP_Text _textColourPickerBlue;
    [SerializeField] private TMP_Text _textUserInfo;

    [Space(10)]
    [Header("Show Debug Text?")] [Header("On screen Log")] 
    [SerializeField] private bool _flagLog;
    [SerializeField] private bool _flagUserInfo;
    [Header("Colour Picker")]
    [SerializeField] private bool _flagColourPickerRed;
    [SerializeField] private bool _flagColourPickerGreen;
    [SerializeField] private bool _flagColourPickerBlue;

    // Start is called before the first frame update
    void Start()
    {
        _textOnScreenLog.gameObject.SetActive(_flagLog);
        _textColourPickerRed.gameObject.SetActive(_flagColourPickerRed);
        _textColourPickerGreen.gameObject.SetActive(_flagColourPickerGreen);
        _textColourPickerBlue.gameObject.SetActive(_flagColourPickerBlue);
        _textUserInfo.gameObject.SetActive(_flagUserInfo);
    }
    
}
