using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//动物圈信息类
public class AnimalSlotDetails
{
    public AnimalDetails animalDetails; //养殖动物信息
    public int animalCount; //动物数量
    public int feedCount; //饲料数量
    public int harvestCount; //可以收获的产品数量
    public int canKillAnimalsCount; //可以宰杀的动物数量
    public List<int> animalGrowDays; //各个动物的生长天数
}

public class AnimalSlotSaveData
{
    public int animalId; //养殖的动物id
    public int animalCount; //动物数量
    public int feedCount;
    public int harvestCount;
    public int canKillAnimalsCount;
    public List<int> animalGrowDays;
}
