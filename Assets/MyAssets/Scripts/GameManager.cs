using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //SINGLETON 
    private static GameManager _instance;

    public static GameManager Instance
    {
        get {
            if (_instance == null)
            {
                Debug.LogError("game manager instance is null.");
            }
            return _instance;
        }
    }

    //USER LOGIN INFO 
    //Currently anonymous login
    private string _userID;
    public string UserID { get => _userID; set => _userID = value; }
    private int _userSpriteID;
    public int UserSpriteID { get => _userSpriteID; set => _userSpriteID = value; }
    private Color _userColour = new Color(1,1,1,1);

    public Color UserColour { get => _userColour; set => _userColour = value;}
    [SerializeField] private Sprite[] _avatarSpriteList;

    public Sprite[] AvatarSpriteList => _avatarSpriteList;

    private void Awake()
    {
        if (_instance != null)
        {
            //destroy duplicates
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
