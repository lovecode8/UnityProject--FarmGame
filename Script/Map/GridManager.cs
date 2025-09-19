using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//土地管理类
public class GridManager : SingletonMonoBehavior<GridManager>
{
    public SO_CropDetails so_CropDetails;
    public Transform cropParent;
    public Dictionary<string, GridProperties> gridDic;
    protected override void Awake()
    {
        base.Awake();
        gridDic = new Dictionary<string, GridProperties>();
    }
    private void Start()
    {
        LoadGridProperties();
        // TestFullCrop();
    }
    void OnEnable()
    {
        EventHolder.onGameDayEvent += CropDayEvent;
        EventHolder.onLoadDataEvent += LoadGridSaveData;
    }
    void OnDisable()
    {
        EventHolder.onGameDayEvent -= CropDayEvent;
        EventHolder.onLoadDataEvent -= LoadGridSaveData;
    }

    //-----------------------------初始化------------------------------------

    private void LoadGridProperties() //加载网格信息
    {
        int girdCount = transform.childCount;
        for (int i = 0; i < girdCount; i++)
        {
            Transform child = transform.GetChild(i);
            string key = ("x" + child.position.x + "y" + child.position.z).ToString();
            GridProperties gridProperties = new GridProperties();
            gridProperties.gridX = child.position.x;
            gridProperties.gridY = child.position.z;
            gridDic.Add(key, gridProperties);
        }
    }
    private GridProperties GetGridProperties(string key)
    {
        return gridDic[key];
    }

    private GridProperties GetGridProperties(Vector3 pos)
    {
        string key = ("x" + pos.x + "y" + pos.z).ToString();
        return GetGridProperties(key);
    }

    //--------------------------------作物操作----------------------------------

    //种植作物
    public bool PlantCrop(Vector3 pos, int seedId) //种植作物 外部调用
    {
        //如该地有作物 则返回
        GridProperties gridProperties = GetGridProperties(pos);
        if (gridProperties.cropDetails != null) return false;

        //获取作物信息并赋值
        CropDetails cropDetails = so_CropDetails.GetCropDetails(seedId);
        Debug.Log(cropDetails.cropName);
        gridProperties.growedDays = 0;
        gridProperties.cropDetails = cropDetails;

        //生成植株
        EventHolder.CallonDisplayEffect(Effect.plant, pos);
        InstantiateCrop(gridProperties, pos, gridProperties.cropDetails.growPrefab[gridProperties.growState]);
        //声音
        AudioManager.Instance.PlaySound("plant");
        return true;
    }

    //浇水
    public bool WaterCrop(Vector3 pos)
    {
        GridProperties grid = GetGridProperties(pos);

        if (grid.cropDetails == null)
        {
            EventHolder.CallonShowAdmireEvent("种植了作物才需要浇水哦！");
            return false;
        }
        if (grid.daysSinceWatered == 0)
        {
            EventHolder.CallonShowAdmireEvent("这里今天已经浇过水啦！");
            return false;
        }

        EventHolder.CallonDisplayEffect(Effect.water, pos);
        grid.daysSinceWatered = 0;
        CropGrow(grid);
        AudioManager.Instance.PlaySound("water");
        return true;
    }

    //收获
    public GridProperties HarvestCrop(Vector3 pos)
    {
        //无作物/未成熟
        string key = ("x" + pos.x + "y" + pos.z).ToString();
        GridProperties grid = GetGridProperties(key);
        if (grid.cropDetails == null || !grid.CanBeHarvest())
        {
            return null;
        }

        EventHolder.CallonDisplayEffect(Effect.harvest, pos);

        if (grid.cropDetails.canRegrow && !grid.canRegrowAndProcessing)
        {
            //可以重生且没有重生过
            ReGrowCrop(grid, grid.cropDetails);
            return grid;
        }

        Destroy(grid.growingCrop);
        gridDic[key] = new GridProperties();
        AudioManager.Instance.PlaySound("harvest");
        return grid;
    }

    //销毁
    public bool DestoryCrop(Vector3 pos)
    {
        string key = ("x" + pos.x + "y" + pos.z).ToString();
        GridProperties grid = GetGridProperties(key);

        if (grid.cropDetails == null)
        {
            EventHolder.CallonShowAdmireEvent("附近没有作物可销毁！");
            return false; //没有作物不销毁
        }

        Destroy(grid.growingCrop);

        EventHolder.CallonDisplayEffect(Effect.destory, pos);
        gridDic[key] = new GridProperties();
        AudioManager.Instance.PlaySound("destory");
        return true;
    }

    private void InstantiateCrop(GridProperties grid, Vector3 pos, GameObject cropPrefab)
    {
        GameObject crop = GameObject.Instantiate
        (cropPrefab, pos, Quaternion.identity, cropParent);
        grid.growingCrop = crop;
    }
    private void ReGrowCrop(GridProperties grid, CropDetails crop)
    {
        grid.canRegrowAndProcessing = true;
        grid.growedDays = crop.growDays[0];
        SwitchCropState(grid, 0);
    }
    //-------------------------作物生长------------------------------
    private void CropDayEvent(int year, Season season, int day, int hour, int mintue)
    {
        foreach (var grid in gridDic.ToList())
        {
            GridProperties g = grid.Value;
            if (g.cropDetails != null)
            {
                g.daysSinceWatered++;
                if (!g.CanBeHarvest()) //没到生长最后阶段
                {
                    CropGrow(grid.Value);
                }
            }
        }
    }
    private void CropGrow(GridProperties grid)
    {
        grid.growedDays++;
        Debug.Log(grid.growedDays);
        int state = GetCurrentState(grid);
        if (grid.growState != state)
        {
            SwitchCropState(grid, state);
        }
    }
    private int GetCurrentState(GridProperties grid)
    {
        for (int i = 0; i <= grid.cropDetails.growDays.Length - 1; i++)
        {
            if (grid.growedDays <= grid.cropDetails.growDays[i])
            {
                return i;
            }
        }
        return grid.cropDetails.growDays.Length - 1;
    }
    private void SwitchCropState(GridProperties grid, int state)
    {
        Debug.Log(state);
        //切换生长状态
        Vector3 pos = grid.growingCrop.transform.position;
        Destroy(grid.growingCrop.gameObject);
        grid.growState = state;
        InstantiateCrop(grid, pos, grid.cropDetails.growPrefab[grid.growState]);
    }

    //------------------------创建存档方法--------------------------------
    public Dictionary<string, GridSaveData> GetGridSaveData()
    {
        Dictionary<string, GridSaveData> gridSaveDic = new Dictionary<string, GridSaveData>();
        foreach (KeyValuePair<string, GridProperties> grid in gridDic)
        {
            GridProperties g = grid.Value;
            if (g.cropDetails == null) continue;
            GridSaveData gridData = new GridSaveData();
            gridData.gridX = g.gridX;
            gridData.gridY = g.gridY;
            gridData.daysSinceWatered = g.daysSinceWatered;
            gridData.growedDays = g.growedDays;
            gridData.growState = g.growState;
            gridData.cropId = g.cropDetails.seedId;
            gridData.canRegrowAndProcessing = g.canRegrowAndProcessing;
            gridSaveDic.Add(grid.Key, gridData);
        }
        return gridSaveDic;
    }

    //----------------------加载存档方法----------------------------
    private void LoadGridSaveData(SaveData data)
    {
        Dictionary<string, GridSaveData> gridSaveDic = new Dictionary<string, GridSaveData>();
        gridSaveDic = data.gridPropertiesData;
        //数据+视觉
        foreach (KeyValuePair<string, GridSaveData> gridPair in gridSaveDic)
        {
            GridSaveData g = gridPair.Value;
            GridProperties gridData = new GridProperties();
            gridData.gridX = g.gridX;
            gridData.gridY = g.gridY;
            gridData.daysSinceWatered = g.daysSinceWatered;
            gridData.growedDays = g.growedDays;
            gridData.growState = g.growState;
            gridData.cropDetails = so_CropDetails.GetCropDetails(g.cropId);
            gridData.canRegrowAndProcessing = g.canRegrowAndProcessing;
            gridDic[gridPair.Key] = gridData;
        }
        Debug.Log(gridDic);
        RestoreCrop();
    }

    private void RestoreCrop()
    {
        foreach (KeyValuePair<string, GridProperties> gridPair in gridDic)
        {
            GridProperties grid = gridPair.Value;
            if (grid.cropDetails != null)
            {
                GameObject cropPrefab = grid.cropDetails.growPrefab[grid.growState];
                InstantiateCrop(grid, new Vector3(grid.gridX, 0, grid.gridY), cropPrefab);
            }
        }
    }

    private void TestFullCrop() //尝试把田地铺满作物
    {
        foreach (KeyValuePair<string, GridProperties> gridPair in gridDic)
        {
            GridProperties grid = gridPair.Value;
            CropDetails cropDetails = so_CropDetails.GetCropDetails(Random.Range(1001, 1008));
            GameObject cropPrefab = cropDetails.growPrefab[cropDetails.growPrefab.Length - 1];
            InstantiateCrop(grid, new Vector3(grid.gridX, 0, grid.gridY), cropPrefab);
        }
    }
}
    
                
