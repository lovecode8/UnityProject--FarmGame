using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//动物基本信息类
[Serializable]
public class AnimalDetails
{
    public int animalId;
    public string animalName;
    public int animalSellPrice; //购买一只动物的价格
    public GameObject animalPrefab; //预制体
    public int harvestItemId; //产出物品信息
    public int beKillItemId; //被宰杀后的产品
    public int feedItemId; //动物所需饲料的id
    public int daysToProduce; //产出间隔时间
    public int daysToBeKilled; //产出肉的时间
}
