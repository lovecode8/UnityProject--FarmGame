using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveSystem : SingletonMonoBehavior<SaveSystem>
{
    //存档字典--密码对应存档
    private Dictionary<int, SaveData> saveDataDictionary;
    private string path;
    void Start()
    {
        saveDataDictionary = new Dictionary<int, SaveData>();
        path = Path.Combine(Application.persistentDataPath, Setting.saveDataPath);
    }
    public void SaveGameData(int password)
    {
        SaveData gameData = FindSaveData(password);
        if (gameData == null)
        {
            gameData = new SaveData();
        }

        //获取信息
        gameData.gridPropertiesData = GridManager.Instance.GetGridSaveData();
        gameData.inventoryItemsData = InventoryManager.Instance.GetInventorySaveData();
        gameData.animalSoltDetailsData = AnimalSlotManager.Instance.GetAnimalslotSaveData();
        gameData.gameDay = TimeManager.Instance.GetCurrentDay();

        saveDataDictionary[password] = gameData;
        string jsonData = JsonConvert.SerializeObject(saveDataDictionary, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        Debug.Log(jsonData);
        File.WriteAllText(path, jsonData);
        EventHolder.CallonShowAdmireEvent("数据存储成功!");
    }
    public void LoadGameData(int password)
    {
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            saveDataDictionary = JsonConvert.DeserializeObject<Dictionary<int, SaveData>>(jsonData);
        }
        SaveData data = FindSaveData(password);
        if (data == null)
        {
            EventHolder.CallonShowAdmireEvent("这个密码还没有存储数据哦!");
            return;
        }
        //加载
        EventHolder.CallonLoadDataEvent(data);
        EventHolder.CallonShowAdmireEvent("存档加载成功!");
    }

    private SaveData FindSaveData(int password)
    {
        if (saveDataDictionary.ContainsKey(password))
        {
            return saveDataDictionary[password];
        }
        return null;
    }
}
