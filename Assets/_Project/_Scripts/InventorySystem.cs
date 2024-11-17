using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<Item> items;
    public Dictionary<string,Item> itemList;
    
    public List<string> keys;
    public List<ItemData> itemsDatas;

    [SerializeField] private List<EditableKeyValuePair<string,ItemData>> keyvaluepair;
    private void Awake()
    {

    }

}
