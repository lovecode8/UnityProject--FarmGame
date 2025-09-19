using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CropDetails
{
    public int seedId;
    public string cropName;
    public Sprite seedSprite;//种子的图片
    public int[] growDays;//各个阶段的生长日期
    public GameObject[] growPrefab;//各个阶段的展示作物
    public int harvestItemId;//产品的物品id
    public Sprite harvestSprite;//产品的图片
    public int harvestMinNum;//产出最小值
    public int harvestMaxNum;//最大值
    public bool canRegrow;//收获后是否可以继续生长(从第二阶段开始)
}
