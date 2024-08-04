using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] private Sprite _playerSprite;
    [SerializeField] private Color _playerColour;
    
    private SpriteRenderer _spriteRenderer;
    

    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _spriteRenderer.sprite = GameManager.Instance.AvatarSpriteList[GameManager.Instance.UserSpriteID];
        _spriteRenderer.color = _playerColour;
    }
}
