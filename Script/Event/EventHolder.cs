using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.PlayerLoop;

//事件管理器
public static class EventHolder
{
    //---------------------------------作物事件-----------------------------------

    //种植事件
    public static Action<int, int> onPlantCropEvent;
    public static void CallonPlantCropEvent(int itemId, int itemNum)
    {
        Debug.Log("种植作物");
        onPlantCropEvent?.Invoke(itemId, itemNum);
    }

    //浇水事件
    public static Action onWaterCropEvent;
    public static void CallonWaterOrDestoryCropEvent()
    {
        onWaterCropEvent?.Invoke();
    }

    //销毁作物事件
    public static Action onDestoryCropEvent;
    public static void CallonDestoryCropEvent()
    {
        onDestoryCropEvent?.Invoke();
    }

    //收获作物事件--静止、库存增加
    public static Action<int, int> onHarvestCropEvent;
    public static void CallonHarvestCropEvent(int itemId, int itemNum)
    {
        Debug.Log("收获作物");
        onHarvestCropEvent?.Invoke(itemId, itemNum);
    }

    //-------------------------动物事件------------------------------------------
    public static Action onBuyAnimalEvent; //购买动物
    public static void CallonBuyAnimalEvent()
    {
        onBuyAnimalEvent?.Invoke();
    }

    public static Action onBuyAnimalFeedEvent; //购买饲料
    public static void CallonBuyAnimalFeedEvent()
    {
        onBuyAnimalFeedEvent?.Invoke();
    }

    public static Action onHarvestAnimalEvent; //收获产品
    public static void CallonHarvestAnimalEvent()
    {
        onHarvestAnimalEvent?.Invoke();
    }

    public static Action onKillAnimalEvent; //宰杀动物
    public static void CallonKillAnimalEvent()
    {
        onKillAnimalEvent?.Invoke();
    }

    //-------------------------特效事件-------------------------------------------

    public static Action<Effect, Vector3> onDisplayEffect;
    public static void CallonDisplayEffect(Effect effect, Vector3 pos)
    {
        onDisplayEffect?.Invoke(effect, pos);
    }

    //-----------------------------UI事件-------------------------------------------

    public static Action<List<InventoryItem>> onInventoryUpdateEvent; //库存物品更新
    public static void CallonInventoryUpdateEvent(List<InventoryItem> inventoryList)
    {
        onInventoryUpdateEvent?.Invoke(inventoryList);
    }

    public static Action<int> onCoinCountUpdateEvent; //金币数量更新
    public static void CallonCoinCountUpdateEvent(int addCount)
    {
        onCoinCountUpdateEvent?.Invoke(addCount);
    }

    public static Action<ItemDetails> onGoodInfoPanelUpdateEvent; //更新商品信息面板
    public static void CallonGoodInfoPanelUpdateEvent(ItemDetails itemDetails)
    {
        onGoodInfoPanelUpdateEvent?.Invoke(itemDetails);
    }

    public static Action onClearGoodInfoPanelEvent; //清空商品信息面板
    public static void CallonClearGoodInfoPanelEvent()
    {
        onClearGoodInfoPanelEvent?.Invoke();
    }

    public static Action<string> onShowAdmireEvent; //提醒事件
    public static void CallonShowAdmireEvent(string context)
    {
        onShowAdmireEvent?.Invoke(context);
    }

    //---------------------------------时间事件------------------------------------

    public static Action<int, Season, int, int, int> onGameMintueEvent; //分钟事件
    public static void CallonGameMintueEvent(int year, Season season, int day, int hour, int mintue)
    {
        onGameMintueEvent?.Invoke(year, season, day, hour, mintue);
    }

    public static Action<int, Season, int, int, int> onGameHourEvent; //小时事件
    public static void CallonGameHourEvent(int year, Season season, int day, int hour, int mintue)
    {
        onGameHourEvent?.Invoke(year, season, day, hour, mintue);
    }

    public static Action<int, Season, int, int, int> onGameDayEvent; //天事件
    public static void CallonGameDayEvent(int year, Season season, int day, int hour, int mintue)
    {
        onGameDayEvent?.Invoke(year, season, day, hour, mintue);
    }

    public static Action<int, Season, int, int, int> onGameSeasonEvent; //季节事件
    public static void CallonGameSeasonEvent(int year, Season season, int day, int hour, int mintue)
    {
        onGameSeasonEvent?.Invoke(year, season, day, hour, mintue);
    }

    public static Action<int, Season, int, int, int> onGameYearEvent; //年事件
    public static void CallonGameYearEvent(int year, Season season, int day, int hour, int mintue)
    {
        onGameYearEvent?.Invoke(year, season, day, hour, mintue);
    }

    //--------------------------加载存档事件----------------------------------
    public static Action<SaveData> onLoadDataEvent;
    public static void CallonLoadDataEvent(SaveData data)
    {
        onLoadDataEvent?.Invoke(data);
    }
}
