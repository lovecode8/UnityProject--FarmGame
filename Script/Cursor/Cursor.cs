using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

//处理鼠标事件
public class Cursor : SingletonMonoBehavior<Cursor>
{
    public GameObject storePanel;
    public ItemDetails selectedItemDetails;
    private bool isStoreOpen;
    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {

    }
    void Update()
    {
        PlayerKeyBoradInput();
    }

    private void PlayerKeyBoradInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ProcessMouseClick();
        }
    }
    private void ProcessMouseClick()
    {
        if (selectedItemDetails == null)
        {
            return;
        }
        
        ItemType selectedItemType = selectedItemDetails.itemType;
        switch (selectedItemType)
        {
            case ItemType.seed:
                PlayerInteraction.Instance.PlantCrop(selectedItemDetails.itemId);
                break;
            case ItemType.waterTool:
                PlayerInteraction.Instance.WaterCrop();
                break;
            case ItemType.harvestTool:
                PlayerInteraction.Instance.HarvestCrop();
                break;
            case ItemType.destoryTool:
                PlayerInteraction.Instance.DestoryCrop();
                break;
            default:
                break;
        }
    }
    public void SetSelectedItem(ItemDetails itemDetails)
    {
        selectedItemDetails = itemDetails;
        //更换鼠标样式

    }
}
