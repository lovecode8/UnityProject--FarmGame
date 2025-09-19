using System;
using System.IO;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public static class Setting
{
    //Num数据
    public const int animalSlotMaxCount = 10; //动物圈的最大容量
    public const float animalChangeStateMinTime = 2.5f; //动物改变状态的最小时间间隔
    public const float animalChangeStateMaxTime = 5f; //动物改变状态的最大时间间隔
    public const float animalChangeStateMinDistance = 0.1f; //动物改变状态的距离
    public const float animalwalkMaxDistance = 5f; //动物一次移动的最大距离
    public const int animalSlotCount = 3; //动物圈的数量
    public const int animalMinId = 4001; //动物的最小Id
    public static List<Vector3> animalSlotPosition = new List<Vector3>() //3个动物圈的位置
    {
        new Vector3(-5, 1, 10),
        new Vector3(-5, 1, 2),
        new Vector3(5, 1, -3)
    };

    //几个面板移动的参数
    public const float panelMoveDuration = 0.5f;
    public const float animalPanelMoveDistance = 400f;
    public const float storePanelMoveDistance = 800f;
    public const float orderPanelMoveDistance = 800f;
    public const float admirePanelMoveDistance = 167f;
    public const float saveDataPanelMoveDistance = 650f;
    public const int orderItemCount = 3; //订单的物品数量


    //Player
    public const float playerRunningSpeed = 5f;
    public const float playerWalkingSpeed = 3f;
    public const float rotateSpeed = 10f;

    //PlayerInteraction
    public const float playerInteractionRadius = 1f;

    //Time
    public const float gameSecond = 0.012f; //游戏1秒的时间

    //UI
    public static Color itemSlotHighlightColor = new Color(1, 0, 0, 1);
    public static Color itemSlotNotHighlightColor = new Color(0, 0, 0, 0);
    public static Color itemSlotEmptyColor = new Color(1, 1, 1, 0);
    public static Color itemSlotColor = new Color(1, 1, 1, 1);

    //name
    public static string animalFolkName = "animals";

    //Tag
    public const string playerTag = "Player";

    //Text
    public const string countText = "当前数量：";
    public const string feedText = "饲料剩余：";
    public const string harvestCountText = "产品数量：";
    public const string rawText = "成熟数量：";
    public const string goodName = "商品名称：";
    public const string sellPrice = "售价：";
    public const string goodDesc = "描述：";
    public const string restTime = "剩余时间：";
    public const string orderReward = "订单奖励：";
    public const string carDestination = "carDestination";
    public const string carEndTransfrom = "carEndPosition";

    //Animation
    public const string animalWalk = "isWalk";

    //Light
    public static Quaternion dayLightOriginalRotation = Quaternion.Euler(50, 180, 0); //太阳原始角度
    public const float dayLightOriginalColorG = 0.94f; //太阳原始的颜色G
    public const float dayLightOriginalColorB = 0.78f; //太阳原始的颜色B
    public const float moonLightOriginalPosZ = 50; //月亮原始的位置Z

    public const float dayLightRotatePerMintue = 0.25f; //太阳每分钟旋转的角度
    public const float dayLightChangeMintueColor = 0.004f; //每分钟太阳改变的颜色
    public const float dayLightChangeMintueCountB = 12; //太阳多少分钟改变一次B
    public const float dayLightChangeMintueCountG = 8; //太阳多少分钟改变一次G
    public const float moonLightChangeMintuePosZ = 0.075f; //月亮每分钟改变的位置Z

    public const float SwitchDayLightPerNum = 0.016f; //太阳昼夜交替每分钟改变的值
    public const float SwitchNightLightPerNum = 0.013f; //黑灯昼夜交替每分钟改变的值
    public const float SwitchMoonLightPerNum = 0.005f; //月亮昼夜交替每分钟改变的值

    //协程
    public static WaitForSeconds playerPlantCropSecond = new WaitForSeconds(2.5f);
    public static WaitForSeconds effectExistTime = new WaitForSeconds(3f);
    public static WaitForSeconds carDisappearTime = new WaitForSeconds(10f);
    public static WaitForSeconds admirePanelShowTime = new WaitForSeconds(1.5f);

    //path
    public const string saveDataPath = "saveData.json";
}
