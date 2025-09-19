using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

//管理整个物品栏
public class InventoryBar : SingletonMonoBehavior<InventoryBar>
{
    public List<InventorySlot> inventorySlots;
    public TextMeshProUGUI coinCountText;
    [SerializeField] private int selectedSlotIndex = -1;
    void OnEnable()
    {
        EventHolder.onInventoryUpdateEvent += InventoryUpdate;
        EventHolder.onCoinCountUpdateEvent += CoinCountUpdate;
    }
    void OnDisable()
    {
        EventHolder.onInventoryUpdateEvent -= InventoryUpdate;
        EventHolder.onCoinCountUpdateEvent -= CoinCountUpdate;
    }
    void Start()
    {
        inventorySlots = GetComponentsInChildren<InventorySlot>().ToList();
        ClearAllHighlightItem();
    }
    void Update()
    {
        SelectItemWithKeyBoard();
    }

    //----------------------------数据更新---------------------------
    private void InventoryUpdate(List<InventoryItem> inventoryList)
    {
        ClearAllItem();
        for (int i = 0; i < inventoryList.Count; i++)
        {
            InventoryItem item = inventoryList[i];
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(item.itemId);
            inventorySlots[i].itemImage.color = Setting.itemSlotColor;
            inventorySlots[i].itemImage.sprite = itemDetails.itemSprite;
            inventorySlots[i].itemNumText.SetText(item.itemNum.ToString());
            inventorySlots[i].itemDetails = itemDetails;
            inventorySlots[i].itemNum = item.itemNum;
        }
    }

    private void CoinCountUpdate(int coinCount)
    {
        coinCountText.SetText(coinCount.ToString());
    }

    //--------------------------键盘控制--------------------------------
    private void SelectItemWithKeyBoard()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectedSlotIndex = (selectedSlotIndex + 1) % inventorySlots.Count;
            if (inventorySlots[selectedSlotIndex].itemDetails == null)
            {
                if (selectedSlotIndex != inventorySlots.Count - 1)
                    selectedSlotIndex--;
                return;
            }
            inventorySlots[selectedSlotIndex].isSelected = true;
            SetHighlightItemWithIndex(selectedSlotIndex);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectedSlotIndex = (selectedSlotIndex - 1) < 0 ? inventorySlots.Count - 1 : selectedSlotIndex - 1;
            if (inventorySlots[selectedSlotIndex].itemDetails == null)
            {
                if (selectedSlotIndex != 0)
                    selectedSlotIndex++;
                return;
            }
            inventorySlots[selectedSlotIndex].isSelected = true;
            SetHighlightItemWithIndex(selectedSlotIndex);
        }
    }

    //-------------------------------高亮控制-----------------------------------
    public void ClearAllHighlightItem()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.itemSlotHighlight.color = Setting.itemSlotNotHighlightColor;
            slot.isSelected = false;
        }
        selectedSlotIndex = -1;
        Cursor.Instance.SetSelectedItem(null);
    }

    public void SetHighlightItem()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            SetHighlightItemWithIndex(i);
        }
    }

    private void SetHighlightItemWithIndex(int index)
    {
        if (inventorySlots[index].itemDetails != null)
        {
            if (inventorySlots[index].isSelected)
            {
                ClearAllHighlightItem();
                inventorySlots[index].itemSlotHighlight.color = Setting.itemSlotHighlightColor;
                inventorySlots[index].isSelected = true;
                selectedSlotIndex = index;
                Cursor.Instance.SetSelectedItem(inventorySlots[index].itemDetails);
                if (inventorySlots[index].itemDetails.canBeCarried)
                {
                    Player.Instance.ShowPlayerCarryItem(inventorySlots[index].itemDetails.itemType);
                }
                else
                {
                    Player.Instance.ClearAllCarryItem();
                }
            }
        }
    }

    //----------------------物品显示---------------------------
    private void ClearAllItem()
    {
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.itemImage.color = Setting.itemSlotNotHighlightColor;
            slot.itemNumText.text = "";
            slot.itemDetails = null;
            slot.itemNum = 0;
        }
    }
}
