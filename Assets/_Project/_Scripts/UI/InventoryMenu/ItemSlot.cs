// Separate class for individual item slots

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image           itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Button          button;

    public event System.Action<ItemData> OnSlotClicked;
    private ItemData                     item;

    public void Initialize(ItemData itemData)
    {
        item            = itemData;
        itemIcon.sprite = item.itemIcon;
        itemName.text   = item.itemName;
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnSlotClicked?.Invoke(item));
    }
}