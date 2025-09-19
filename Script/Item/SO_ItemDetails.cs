using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetailsList", menuName = "ScriptableObjects/ItemDetailsList")]
public class SO_ItemDetails : ScriptableObject
{
    public List<ItemDetails> itemDetailsList;
    public ItemDetails GetItemDetails(int id)
    {
        return itemDetailsList.Find(x => x.itemId == id);
    }
}
