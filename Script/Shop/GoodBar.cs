using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GoodBar : MonoBehaviour
{
    public SO_ItemDetails so_ItemDetails;
    public TextMeshProUGUI coinCountText;
    public GameObject storePanel;
    public Button buyItemButton;
    public Button closeShopButton;
    private bool isStoreOpen;
    private List<GoodSlot> goodSlotsList;

    private ItemDetails selectedItemDetails;
    private int currentCoinCount;

    void Start()
    {
        buyItemButton.onClick.AddListener(BuyItem);
        closeShopButton.onClick.AddListener(OpenOrCloseStorePanel);
        InitShop();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenOrCloseStorePanel();
        }
    }


    private void InitShop()
    {
        goodSlotsList = new List<GoodSlot>();
        goodSlotsList = GetComponentsInChildren<GoodSlot>().ToList();

        for (int i = 0; i < so_ItemDetails.itemDetailsList.Count; i++)
        {
            goodSlotsList[i].GoodSlotInit(so_ItemDetails.itemDetailsList[i]);
        }
    }

    public void coinCountTextUpdate()
    {
        currentCoinCount = InventoryManager.Instance.coinCount;
        coinCountText.SetText(currentCoinCount.ToString());
    }
    private void BuyItem()
    {
        if (selectedItemDetails == null)
        {
            return;
        }

        int itemPrice = selectedItemDetails.itemPrice;
        if (currentCoinCount < itemPrice)
        {
            EventHolder.CallonShowAdmireEvent("金币不足，无法购买！");
            return;
        }
        currentCoinCount -= itemPrice;
        coinCountText.SetText(currentCoinCount.ToString());
        AudioManager.Instance.PlaySound("buyItem");
        InventoryManager.Instance.AddItem(selectedItemDetails.itemId);
    }
    public void ClearAllSelectedGoods()
    {
        foreach (GoodSlot slot in goodSlotsList)
        {
            slot.goodSlotHightlight.color = Setting.itemSlotNotHighlightColor;
            slot.isSelected = false;
        }

        selectedItemDetails = null;
        buyItemButton.SetActive(false);
        EventHolder.CallonClearGoodInfoPanelEvent();
    }
    public void SetSelectedGood()
    {
        foreach (GoodSlot slot in goodSlotsList)
        {
            if (slot.isSelected)
            {
                slot.goodSlotHightlight.color = Setting.itemSlotHighlightColor;
                selectedItemDetails = slot.goodItemdetails;
                EventHolder.CallonGoodInfoPanelUpdateEvent(selectedItemDetails);
            }
        }
        buyItemButton.SetActive(true);
    }

    private void OpenOrCloseStorePanel()
    {
        RectTransform rectTransform = storePanel.GetComponent<RectTransform>();
        if (!isStoreOpen)
        {
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x +
            Setting.storePanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);
            coinCountTextUpdate();
            isStoreOpen = true;
        }
        else
        {
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x -
            Setting.storePanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);
            InventoryManager.Instance.coinCount = currentCoinCount;
            EventHolder.CallonCoinCountUpdateEvent(currentCoinCount);
            isStoreOpen = false;
        }
    }
}
