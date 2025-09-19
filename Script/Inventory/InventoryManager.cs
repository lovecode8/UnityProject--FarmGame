using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : SingletonMonoBehavior<InventoryManager>
{
    public int addItemTestId;
    public SO_ItemDetails so_ItemDetails;
    public List<InventoryItem> inventoryItemList;
    public int coinCount;
    private Dictionary<int, ItemDetails> itemDictionary;
    void Start()
    {
        inventoryItemList = new List<InventoryItem>();
        CreateItemDictionary();
    }
    void OnEnable()
    {
        EventHolder.onPlantCropEvent += RemoveItem;
        EventHolder.onHarvestCropEvent += AddItem;
        EventHolder.onLoadDataEvent += LoadInventoryData;
    }
    void OnDisable()
    {
        EventHolder.onPlantCropEvent -= RemoveItem;
        EventHolder.onHarvestCropEvent -= AddItem;
        EventHolder.onLoadDataEvent -= LoadInventoryData;
    }
    private void AddOriginItem()
    {
        AddCoin(1500);
    }
    void Update()
    {

    }

    private void CreateItemDictionary()
    {
        itemDictionary = new Dictionary<int, ItemDetails>();
        foreach (ItemDetails itemDetails in so_ItemDetails.itemDetailsList)
        {
            if (!itemDictionary.ContainsKey(itemDetails.itemId))
            {
                itemDictionary.Add(itemDetails.itemId, itemDetails);
            }
        }
        AddOriginItem();
    }

    public ItemDetails GetItemDetails(int itemId)
    {
        if (itemDictionary.ContainsKey(itemId))
        {
            return itemDictionary[itemId];
        }
        return null;
    }

    //---------------------------库存操作-------------------------
    #region 增加
    public void AddItem(int itemId, int itemNum = 1)
    {
        int itemIndex = FindItemInInventory(itemId);

        if (itemIndex == -1)
        {
            AddItemAtPosition(itemId, itemNum);
        }
        else
        {
            AddItemAtPosition(itemId, itemIndex, itemNum);
        }

        EventHolder.CallonInventoryUpdateEvent(inventoryItemList);
    }
    private void AddItemAtPosition(int itemId, int itemNum)
    {
        InventoryItem item = new InventoryItem();
        item.itemId = itemId;
        item.itemNum = itemNum;
        inventoryItemList.Add(item);
    }
    private void AddItemAtPosition(int itemId, int index, int itemNum)
    {
        InventoryItem item = inventoryItemList[index];
        item.itemId = itemId;
        item.itemNum += itemNum;
        inventoryItemList[index] = item;
    }
    public void AddCoin(int addCount)
    {
        coinCount += addCount;
        EventHolder.CallonCoinCountUpdateEvent(coinCount);
    }
    #endregion

    #region 减少

    public void RemoveItem(int itemId, int itemNum = 1)
    {
        int index = FindItemInInventory(itemId);
        if (index == -1)
        {
            return;
        }
        inventoryItemList[index].itemNum -= itemNum;

        if (inventoryItemList[index].itemNum == 0)
        {
            inventoryItemList.RemoveAt(index);
            //选择的物品清空
            InventoryBar.Instance.ClearAllHighlightItem();
        }
        EventHolder.CallonInventoryUpdateEvent(inventoryItemList);
    }

    public void RemoveCoin(int removeCount)
    {
        coinCount -= removeCount;
        EventHolder.CallonCoinCountUpdateEvent(coinCount);
    }
    #endregion

    public int FindItemInInventory(int itemId)
    {
        for (int i = 0; i < inventoryItemList.Count; i++)
        {
            if (inventoryItemList[i].itemId == itemId)
            {
                return i;
            }
        }
        return -1;
    }

    public bool FindItemInInventoryWithCount(int itemId, int itemCount)
    {
        int index = FindItemInInventory(itemId);
        if (index != -1)
        {
            return inventoryItemList[index].itemNum >= itemCount;
        }
        return false;
    }

    #region 存档
    public InventorySaveData GetInventorySaveData()
    {
        InventorySaveData data = new InventorySaveData();
        data.inventoryItemList = inventoryItemList;
        data.coinCount = coinCount;
        return data;
    }
    private void LoadInventoryData(SaveData data)
    {
        InventorySaveData inventoryData = data.inventoryItemsData;
        inventoryItemList = inventoryData.inventoryItemList;
        coinCount = inventoryData.coinCount;
        EventHolder.CallonInventoryUpdateEvent(inventoryItemList);
        EventHolder.CallonCoinCountUpdateEvent(coinCount);
    }
    #endregion
}
