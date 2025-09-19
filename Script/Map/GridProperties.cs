using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

//土地信息类
public class GridProperties
{
    public float gridX;
    public float gridY;
    public int daysSinceWatered = -1; //距离上次浇水的时间
    public int growedDays = -1; //有作物且生长了的时间
    public int growState = 0;
    public CropDetails cropDetails = null; //生长在网格上的作物信息
    public GameObject growingCrop = null; //在网格上生长的作物
    public bool canRegrowAndProcessing = false;

    public GridProperties() { }
    public bool CanBeHarvest()
    {
        return growState == cropDetails.growDays.Length - 1;
    }
}
public class GridSaveData
{
    public float gridX;
    public float gridY;
    public int daysSinceWatered = -1; //距离上次浇水的时间
    public int growedDays = -1; //有作物且生长了的时间
    public int growState = 0;
    public int cropId;
    public bool canRegrowAndProcessing = false;
}
