using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoodInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI goodNameText;
    public TextMeshProUGUI goodPriceText;
    public TextMeshProUGUI goodDescText;
    public Image goodImage;
    void Start()
    {
        CleatGoodInfoPanel();
    }

    void OnEnable()
    {
        EventHolder.onGoodInfoPanelUpdateEvent += GoodInfoPanelUpdate;
        EventHolder.onClearGoodInfoPanelEvent += CleatGoodInfoPanel;
    }
    void OnDisable()
    {
        EventHolder.onGoodInfoPanelUpdateEvent += GoodInfoPanelUpdate;
        EventHolder.onClearGoodInfoPanelEvent -= CleatGoodInfoPanel;
    }

    private void GoodInfoPanelUpdate(ItemDetails itemDetails)
    {
        goodNameText.SetText(Setting.goodName + itemDetails.itemName);
        goodPriceText.SetText(Setting.sellPrice + itemDetails.itemPrice.ToString());
        goodDescText.SetText(Setting.goodDesc + itemDetails.itemDescription);
        goodImage.color = Setting.itemSlotColor;
        goodImage.transform.parent.gameObject.SetActive(true);
        goodImage.sprite = itemDetails.itemSprite;
    }
    private void CleatGoodInfoPanel()
    {
        goodNameText.SetText("");
        goodPriceText.SetText("");
        goodDescText.SetText("");
        goodImage.transform.parent.gameObject.SetActive(false);
    }
}
