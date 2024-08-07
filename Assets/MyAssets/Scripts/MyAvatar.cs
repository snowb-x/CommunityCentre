using System;
using UnityEngine;

    [Serializable]//Important to add this so the JsonUtility can serialize objects with this class
    public class MyAvatar
    {
        public string name;
        public Color colour;
        public int spriteId;
        public string userID;
        
        public string toString() {
            return this.name + ", " + this.spriteId + ", " + this.userID + ", " + colour.ToString();
        }
    }
