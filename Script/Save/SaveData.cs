using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//存储的信息类
public class SaveData
{
    public Dictionary<string, GridSaveData> gridPropertiesData; //土地信息
    public InventorySaveData inventoryItemsData; //库存信息
    public Dictionary<int, AnimalSlotSaveData> animalSoltDetailsData; //动物圈信息
    public int gameDay; //时间信息
}
