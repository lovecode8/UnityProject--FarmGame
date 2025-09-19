using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public void Init(string itemName, string itemDescription)
    {
        itemNameText.SetText(itemName);
        itemDescriptionText.SetText(itemDescription);
    }
}
