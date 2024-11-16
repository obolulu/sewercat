using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    [Required]
    public string itemID;
        
    public string itemName;
        
    public string itemDescription;
        
    public Sprite itemIcon;
        
    public int itemValue;
    
    
}