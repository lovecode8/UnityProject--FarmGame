using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

//管理单个物品槽
public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //UI
    public Image itemImage;
    public Image itemSlotHighlight;
    public TextMeshProUGUI itemNumText;

    public GameObject itemInfoPanelPrefab;
    public bool isSelected;

    //数据
    public ItemDetails itemDetails;
    public int itemNum;

    private InventoryInfoPanel inventoryInfoPanel;


    //------------------------鼠标事件--------------------------
    public void OnBeginDrag(PointerEventData data)
    {

    }
    public void OnDrag(PointerEventData data)
    {

    }
    public void OnEndDrag(PointerEventData data)
    {

    }
    public void OnPointerEnter(PointerEventData data)
    {
        if (itemDetails != null)
        {
            GameObject itemInfoPanel = Instantiate(itemInfoPanelPrefab, transform);
            inventoryInfoPanel = itemInfoPanel.GetComponent<InventoryInfoPanel>();
            inventoryInfoPanel.Init(itemDetails.itemName, itemDetails.itemDescription);
        }
    }

    public void OnPointerExit(PointerEventData data)
    {
        DestoryinventoryInfoPanel();
    }

    public void OnPointerClick(PointerEventData data)
    {
        Debug.Log("Click");
        if (data.button == PointerEventData.InputButton.Left)
        {
            if (isSelected)
            {
                ClearSelectedItem();
            }
            else
            {
                if (itemDetails != null)
                {
                    SetSelectedItem();
                }
            }
        }
    }

    private void SetSelectedItem()
    {
        InventoryBar.Instance.ClearAllHighlightItem();
        isSelected = true;
        InventoryBar.Instance.SetHighlightItem();
    }

    private void ClearSelectedItem()
    {
        InventoryBar.Instance.ClearAllHighlightItem();
        isSelected = false;
    }
    private void DestoryinventoryInfoPanel()
    {
        if (inventoryInfoPanel != null)
        {
            Destroy(inventoryInfoPanel.gameObject);
        }
    }
}
