using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//管理玩家的鼠标交互
public class PlayerInteraction : SingletonMonoBehavior<PlayerInteraction>
{
    Animator animator;
    public LayerMask farmLandLayer;
    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    //---------------种植、浇水、收获、销毁----------------
    public bool PlantCrop(int seedId)
    {
        Collider closeFarmLand = GetClosestFarmLand();
        if (closeFarmLand == null)
        {
            EventHolder.CallonShowAdmireEvent("附近没有土地可种植！");
            return false;
        }
        if (!GridManager.Instance.PlantCrop(closeFarmLand.transform.position, seedId))
        {
            EventHolder.CallonShowAdmireEvent("这个地方已经有作物啦！");
            return false;
        }

        PlayerFaceToFarmLand(closeFarmLand.transform);
        EventHolder.CallonPlantCropEvent(seedId, 1);
        animator.SetTrigger("plant");
        return true;
    }
    public bool WaterCrop()
    {
        Collider closeFarmLand = GetClosestFarmLand();
        if (closeFarmLand == null)
        {
            EventHolder.CallonShowAdmireEvent("附近没有土地可浇水！");
            return false;
        }
        if (!GridManager.Instance.WaterCrop(closeFarmLand.transform.position))
        {
            return false;
        }

        PlayerFaceToFarmLand(closeFarmLand.transform);
        EventHolder.CallonWaterOrDestoryCropEvent();
        animator.SetTrigger("water");
        Debug.Log("成功浇水");
        return true;
    }
    public bool HarvestCrop()
    {
        Collider closeFarmLand = GetClosestFarmLand();
        if (closeFarmLand == null)
        {
            EventHolder.CallonShowAdmireEvent("附近没有土地可交互！");
            return false;
        }
        GridProperties grid = GridManager.Instance.HarvestCrop(closeFarmLand.transform.position);
        if (grid == null)
        {
            EventHolder.CallonShowAdmireEvent("作物还没准备好哦！");
            return false;
        }

        PlayerFaceToFarmLand(closeFarmLand.transform);

        int itemId = grid.cropDetails.harvestItemId;
        int itemNum = Random.Range(grid.cropDetails.harvestMinNum, grid.cropDetails.harvestMaxNum);
        EventHolder.CallonHarvestCropEvent(itemId, itemNum);

        animator.SetTrigger("harvest");
        Debug.Log("成功收割");
        return true;
    }
    public bool DestoryCrop()
    {
        Collider closeFarmLand = GetClosestFarmLand();
        if (closeFarmLand == null)
        {
            EventHolder.CallonShowAdmireEvent("附近没有土地可交互！");
            return false;
        }
        if (!GridManager.Instance.DestoryCrop(closeFarmLand.transform.position))
        {
            return false;
        }

        PlayerFaceToFarmLand(closeFarmLand.transform);
        EventHolder.CallonDestoryCropEvent();
        animator.SetTrigger("destory");
        Debug.Log("成功销毁");
        return true;
    }

    //-------------------其他方法----------------------
    private void PlayerFaceToFarmLand(Transform farmLand) //人物转向土地
    {
        Vector3 targetRotation =
        new Vector3(farmLand.position.x, transform.position.y, farmLand.position.z);
        transform.LookAt(targetRotation);
    }
    private Collider GetClosestFarmLand() //获取最近土地
    {
        Vector3 playerPosition = Player.Instance.GetPlayerPosition();
        Collider[] farmLands = Physics.OverlapSphere(playerPosition, Setting.playerInteractionRadius, farmLandLayer);
        if (farmLands.Count() == 0)
        {
            return null;
        }
        float distance = 10;
        Collider ans = null;
        foreach (Collider farmLand in farmLands)
        {
            float d = Vector3.Distance(farmLand.transform.position, playerPosition);
            if (d < distance)
            {
                ans = farmLand;
                distance = d;
            }
        }
        return ans;
    }
}
