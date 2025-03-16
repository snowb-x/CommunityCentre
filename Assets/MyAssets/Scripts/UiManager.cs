using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _userAccountFormUI;
    [SerializeField] private TMP_Text _userEmailText;

    private void OnEnable()
    {
        GameManager.OnSignin += CloseUserAccountFormUI;
        GameManager.OnSignin += DisplayUserInfoInAvatarCreationForm;
    }

    private void OnDisable()
    {
        GameManager.OnSignin -= CloseUserAccountFormUI;
        GameManager.OnSignin += DisplayUserInfoInAvatarCreationForm;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentUser != null && !_userAccountFormUI.activeSelf)
        {
            CloseUserAccountFormUI();
        }
    }

    private void CloseUserAccountFormUI()
    {
        _userAccountFormUI.SetActive(false);
        GameManager.OnSignin -= CloseUserAccountFormUI;
    }

    private void DisplayUserInfoInAvatarCreationForm()
    {
        _userEmailText.text = String.Format("User: {0}", GameManager.Instance.CurrentUser.email);
    }
    
}
