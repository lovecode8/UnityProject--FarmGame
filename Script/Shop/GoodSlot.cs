using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoodSlot : MonoBehaviour, IPointerClickHandler
{
    public GoodBar goodBar;
    public Image goodImage;
    public TextMeshProUGUI goodPriceText;
    public Image goodSlotHightlight;
    public ItemDetails goodItemdetails;
    public bool isSelected;

    public void GoodSlotInit(ItemDetails itemDetails)
    {
        goodItemdetails = itemDetails;
        goodImage.sprite = goodItemdetails.itemSprite;
        goodPriceText.SetText(goodItemdetails.itemPrice.ToString());
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (isSelected)
        {
            ClearAllSelectedGoods();
        }
        else
        {
            if (goodItemdetails != null)
            {
                SetSelectedGood();
            }
        }
    }

    private void ClearAllSelectedGoods()
    {
        isSelected = false;
        goodBar.ClearAllSelectedGoods();
    }
    private void SetSelectedGood()
    {
        goodBar.ClearAllSelectedGoods();
        isSelected = true;
        goodBar.SetSelectedGood();
    }
}
