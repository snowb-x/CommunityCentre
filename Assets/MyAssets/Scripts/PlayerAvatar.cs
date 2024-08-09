using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAvatar : MonoBehaviour
{
    [SerializeField]private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _textName;

    private void Start()
    {
        _spriteRenderer.sprite = GameManager.Instance.AvatarSpriteList[GameManager.Instance.UserSpriteID];
        _spriteRenderer.color = GameManager.Instance.UserColour;
        _spriteRenderer.gameObject.AddComponent(typeof(CapsuleCollider));
        _textName.text = GameManager.Instance.UserAvatarName;
    }
}
