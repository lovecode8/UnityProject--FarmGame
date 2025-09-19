using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public int itemId;
    public int itemNum;
}

public class InventorySaveData
{
    public List<InventoryItem> inventoryItemList;
    public int coinCount;
}
