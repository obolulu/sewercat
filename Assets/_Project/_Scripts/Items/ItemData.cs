using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public abstract class ItemData : ScriptableObject
{
    [Title("Item ID")]
    [Required]
    public string itemID;
        
    [Title("Item Name")]
    [Required]
    public string itemName;
        
    [Title("Item Description")]
    [Required]
    public string itemDescription;
        
    [Title("Item Icon")]
    [Required]
    public Sprite itemIcon;
        
    [Title("Item Value")]
    [Required]
    public int itemValue;
    
}