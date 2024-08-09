using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PeepTest1 : MonoBehaviour
{
   // [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _peepName;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform _nameTextHeight;
    [SerializeField] private float _tallNameHeight = 5.5f;
    [SerializeField] private float _shortNameHeight = 3.5f;
    private int _lastTallSpriteID = 0;
    private int _spriteID = -1;

    private void Start()
    {
        //IMPORTANT Uncomment before building
        _lastTallSpriteID = GameManager.Instance.LastTallSpriteID;
        setNameTextHeight(_tallNameHeight);
        gameObject.AddComponent(typeof(CapsuleCollider));
        
    }

    private void setNameTextHeight(float height)
    {
        _nameTextHeight.transform.localPosition = new Vector3(0, height,0); 
        
    }
    public void SetSprite(int spriteID, Sprite sprite, Color color)
    {
        // _spriteRenderer.sprite = sprite;
        // _spriteRenderer.color = color;
        // gameObject.AddComponent(typeof(CapsuleCollider));
         _spriteID = spriteID;
    }

    public void SetNameOfObject(string newName)
    {
        gameObject.name = newName;
        _peepName.text = newName;
        if (_spriteID > _lastTallSpriteID)
         {
            setNameTextHeight(_shortNameHeight);  
         }

        if (_spriteID == -1)
        {
            setNameTextHeight(_shortNameHeight);  
            Debug.LogError("spriteID has not been given");
        }
    }

    public void SetParent(GameObject parent)
    {
        gameObject.transform.parent = parent.transform;
    }

    public void SetLocation(Vector3 location)
    {
        gameObject.transform.position = location;
    }
}
