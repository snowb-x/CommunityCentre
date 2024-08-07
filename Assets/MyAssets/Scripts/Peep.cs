using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peep : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Rigidbody rb;
    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    
    public void SetSprite(Sprite sprite, Color color)
    {
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.color = color;
        gameObject.AddComponent(typeof(BoxCollider));
    }
}
