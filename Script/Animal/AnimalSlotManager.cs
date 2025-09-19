using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

//动物圈管理
public class AnimalSlotManager : SingletonMonoBehavior<AnimalSlotManager>
{
    public SO_AnimalDetails so_AnimalDetails;
    public GameObject animalPanel;
    public List<Transform> animalParents;
    public Dictionary<int, AnimalSlotDetails> animalSlotDetailsDictionary; //管理3个圈 鸡、流、猪
    public AnimalSlotDetails currentAnimalSlotDetails;
    RectTransform animalPanelRectTransform;
    protected override void Awake()
    {
        animalPanelRectTransform = animalPanel.GetComponent<RectTransform>();
        base.Awake();
    }
    void Start()
    {
        animalSlotDetailsDictionary = new Dictionary<int, AnimalSlotDetails>();
        InitAnimalSlotDetails();
    }

    void OnEnable()
    {
        EventHolder.onBuyAnimalEvent += BuyAnimal;
        EventHolder.onBuyAnimalFeedEvent += AddAnimalFeed;
        EventHolder.onHarvestAnimalEvent += HarvestAnimal;
        EventHolder.onKillAnimalEvent += KillAnimal;

        EventHolder.onGameDayEvent += AnimalDayEvent;

        EventHolder.onLoadDataEvent += LoadAnimalSlotData;
    }

    void OnDisable()
    {
        EventHolder.onBuyAnimalEvent -= BuyAnimal;
        EventHolder.onBuyAnimalFeedEvent -= AddAnimalFeed;
        EventHolder.onHarvestAnimalEvent -= HarvestAnimal;
        EventHolder.onKillAnimalEvent -= KillAnimal;

        EventHolder.onGameDayEvent -= AnimalDayEvent;

        EventHolder.onLoadDataEvent -= LoadAnimalSlotData;
    }

    private void InitAnimalSlotDetails()
    {
        for (int i = 0; i < Setting.animalSlotCount; i++)
        {
            AnimalDetails animalDetails = so_AnimalDetails.GetAnimalDetails(i + Setting.animalMinId);
            AnimalSlotDetails animalSlotDetails = new AnimalSlotDetails();
            animalSlotDetails.animalDetails = animalDetails;
            animalSlotDetails.animalCount = 0;
            animalSlotDetails.feedCount = 0;
            animalSlotDetails.harvestCount = 0;
            animalSlotDetails.canKillAnimalsCount = 0;
            animalSlotDetails.animalGrowDays = new List<int>();
            animalSlotDetailsDictionary.Add(i + Setting.animalMinId, animalSlotDetails);
        }
    }

    private void AnimalDayEvent(int year, Season season, int day, int hour, int mintue) //日更新函数
    {
        foreach (AnimalSlotDetails animalSlotDetails in animalSlotDetailsDictionary.Values)
        {
            int canGrowAnimalCount =
            Mathf.Min(animalSlotDetails.animalCount, animalSlotDetails.feedCount);
            animalSlotDetails.feedCount -= canGrowAnimalCount;
            Debug.Log(canGrowAnimalCount);
            for (int i = 0; i < canGrowAnimalCount; i++)
            {
                int animalGrowDays = ++animalSlotDetails.animalGrowDays[i];
                Debug.Log(animalGrowDays);

                if (animalGrowDays % animalSlotDetails.animalDetails.daysToProduce == 0)
                {
                    animalSlotDetails.harvestCount++;
                }

                if (animalGrowDays == animalSlotDetails.animalDetails.daysToBeKilled)
                {
                    animalSlotDetails.canKillAnimalsCount++;
                }
            }
        }
        ShowAnimalPanel();
    }

    private void BuyAnimal()
    {
        if (currentAnimalSlotDetails.animalCount >= Setting.animalSlotMaxCount)
        {
            EventHolder.CallonShowAdmireEvent("该圈动物数量已满！");
            return;
        }
        int animalSellPrice = currentAnimalSlotDetails.animalDetails.animalSellPrice;
        if (InventoryManager.Instance.coinCount < animalSellPrice)
        {
            EventHolder.CallonShowAdmireEvent("金币不足！");
            return;
        }

        currentAnimalSlotDetails.animalCount++;
        InventoryManager.Instance.RemoveCoin(animalSellPrice);

        AnimalDetails animalDetails = currentAnimalSlotDetails.animalDetails;
        currentAnimalSlotDetails.animalGrowDays.Add(0);

        int animalSlotIndex = animalDetails.animalId - Setting.animalMinId;

        GameObject.Instantiate(animalDetails.animalPrefab, Setting.animalSlotPosition
        [animalSlotIndex], Quaternion.identity, animalParents[animalSlotIndex]);
        AudioManager.Instance.PlaySound("buyItem");
        ShowAnimalPanel();
    }


    private void AddAnimalFeed()
    {
        //减少库存
        int animalFeedId = currentAnimalSlotDetails.animalDetails.feedItemId;
        if (InventoryManager.Instance.FindItemInInventory(animalFeedId) == -1)
        {
            EventHolder.CallonShowAdmireEvent("库存饲料不足！");
            return;
        }
        currentAnimalSlotDetails.feedCount++;
        InventoryManager.Instance.RemoveItem(animalFeedId);
        ShowAnimalPanel();
    }

    private void HarvestAnimal()
    {
        if (currentAnimalSlotDetails.harvestCount == 0)
        {
            EventHolder.CallonShowAdmireEvent("没有可收获的产品！");
            return;
        }

        //增加库存
        InventoryManager.Instance.AddItem(currentAnimalSlotDetails.animalDetails.harvestItemId,
        currentAnimalSlotDetails.harvestCount);

        currentAnimalSlotDetails.harvestCount = 0;
        ShowAnimalPanel();
    }

    private void KillAnimal()
    {
        if (currentAnimalSlotDetails.canKillAnimalsCount == 0)
        {
            EventHolder.CallonShowAdmireEvent("没有可以宰杀的动物！");
            return;
        }

        currentAnimalSlotDetails.canKillAnimalsCount--;
        currentAnimalSlotDetails.animalCount--;
        currentAnimalSlotDetails.animalGrowDays.RemoveAt(0);

        GameObject animalPrefab =
        transform.GetChild(currentAnimalSlotDetails.animalDetails.animalId -
        Setting.animalMinId).Find(Setting.animalFolkName).GetChild(0).gameObject;

        EventHolder.CallonDisplayEffect(Effect.animalBeKilled, animalPrefab.transform.position);

        Destroy(animalPrefab);
        //增加库存
        InventoryManager.Instance.AddItem(currentAnimalSlotDetails.animalDetails.beKillItemId);
        ShowAnimalPanel();
    }

    public void ShowAnimalPanel(int animalId)
    {
        currentAnimalSlotDetails = animalSlotDetailsDictionary[animalId];

        animalPanelRectTransform.DOAnchorPosX(animalPanelRectTransform.anchoredPosition.x +
        Setting.animalPanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);

        animalPanel.GetComponent<AnimalPanel>().ShowAnimalPanel(currentAnimalSlotDetails);
    }
    private void ShowAnimalPanel()
    {
        if (currentAnimalSlotDetails != null)
        {
            animalPanel.GetComponent<AnimalPanel>().ShowAnimalPanel(currentAnimalSlotDetails);
        }
    }

    public void ClearAnimalPanel()
    {
        animalPanelRectTransform.DOAnchorPosX(animalPanelRectTransform.anchoredPosition.x -
        Setting.animalPanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);
    }

    #region 存储
    public Dictionary<int, AnimalSlotSaveData> GetAnimalslotSaveData() //获取动物圈信息
    {
        Dictionary<int, AnimalSlotSaveData> animalSlotData = new Dictionary<int, AnimalSlotSaveData>();
        foreach (KeyValuePair<int, AnimalSlotDetails> animalSlot in animalSlotDetailsDictionary)
        {
            AnimalSlotDetails slot = animalSlot.Value;
            AnimalSlotSaveData data = new AnimalSlotSaveData();
            data.animalId = slot.animalDetails.animalId;
            data.animalCount = slot.animalCount;
            data.feedCount = slot.feedCount;
            data.harvestCount = slot.harvestCount;
            data.canKillAnimalsCount = slot.canKillAnimalsCount;
            data.animalGrowDays = new List<int>();
            data.animalGrowDays = slot.animalGrowDays;
            animalSlotData.Add(animalSlot.Key, data);
        }
        return animalSlotData;
    }

    private void LoadAnimalSlotData(SaveData saveData)
    {
        Dictionary<int, AnimalSlotSaveData> animalSlotData = new Dictionary<int, AnimalSlotSaveData>();
        animalSlotData = saveData.animalSoltDetailsData;

        foreach (KeyValuePair<int, AnimalSlotSaveData> animalSlot in animalSlotData)
        {
            AnimalSlotSaveData slot = animalSlot.Value;
            AnimalSlotDetails data = animalSlotDetailsDictionary[animalSlot.Key];
            data.animalDetails = so_AnimalDetails.GetAnimalDetails(slot.animalId);
            data.animalCount = slot.animalCount;
            data.feedCount = slot.feedCount;
            data.harvestCount = slot.harvestCount;
            data.canKillAnimalsCount = slot.canKillAnimalsCount;
            data.animalGrowDays = slot.animalGrowDays;
        }

        RestoreAnimalSlot();
    }

    private void RestoreAnimalSlot()
    {
        foreach (KeyValuePair<int, AnimalSlotDetails> animalSlotPair in animalSlotDetailsDictionary)
        {
            AnimalSlotDetails animalSlotDetails = animalSlotPair.Value;
            if (animalSlotDetails.animalCount != 0)
            {
                int count = 0;
                int animalSlotIndex = animalSlotPair.Key - Setting.animalMinId;
                while (count < animalSlotDetails.animalCount && count < Setting.animalSlotMaxCount)
                {
                    GameObject.Instantiate(animalSlotDetails.animalDetails.animalPrefab,
                    Setting.animalSlotPosition[animalSlotIndex], Quaternion.identity, animalParents[animalSlotIndex]);
                    count++;
                }
            }
        }
    }
    #endregion
}
