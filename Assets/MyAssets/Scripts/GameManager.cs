using System;
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
    public static event Action OnSignin;
    public static event Action OnSignout;
    public static event Action OnFailedSignin; 
    //USER LOGIN INFO 
    //Currently anonymous login
    private User _currentUser = null;
    public User CurrentUser { get => _currentUser; set => _currentUser = value; }
    private string _userID;
    public string UserID { get => _userID; set => _userID = value; }
    private int _userSpriteID;
    public int UserSpriteID { get => _userSpriteID; set => _userSpriteID = value; }
    private Color _userColour = new Color(1,1,1,1);

    public Color UserColour { get => _userColour; set => _userColour = value;}
    [SerializeField] private Sprite[] _avatarSpriteList;
    private string _userAvatarName;

    public string UserAvatarName { get => _userAvatarName; set => _userAvatarName = value; }
    [SerializeField] private int _lastTallSpriteID;

    public int LastTallSpriteID => _lastTallSpriteID; 

    public Sprite[] AvatarSpriteList => _avatarSpriteList;
    [SerializeField] private string _dataBaseCollectionPath;
  
    public string DataBaseCollectionPath { get => _dataBaseCollectionPath; set => _dataBaseCollectionPath = value;}

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

    public void InvokeSignIn()
    {
        OnSignin?.Invoke();
    }

    public void InvokeSignOut()
    {
        OnSignout?.Invoke();
    }

    public void InvokeFailedSignIn()
    {
        OnFailedSignin?.Invoke();
    }
}
