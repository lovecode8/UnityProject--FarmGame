using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanel : MonoBehaviour
{
    public List<Image> itemImageList;
    public List<TextMeshProUGUI> itemNameTextList;
    public List<TextMeshProUGUI> itemNumTextList;
    public TextMeshProUGUI restTimeText;
    public TextMeshProUGUI rewardText;
    public Button finishButton;
    public Button closeButton;

    void Start()
    {
        finishButton.onClick.AddListener(TryFinishOrder);
        closeButton.onClick.AddListener(ClosePanel);
    }

    public void UpdateOrderPanel(OrderDetails order, int daysSinceArrived)
    {
        for (int i = 0; i < Setting.orderItemCount; i++)
        {
            ItemDetails item = InventoryManager.Instance.GetItemDetails(order.orderItemIdList[i]);
            itemImageList[i].sprite = item.itemSprite;
            itemNameTextList[i].SetText(item.itemName);
            itemNumTextList[i].SetText("X" + order.orderItemNumList[i].ToString());
        }

        int restTime = order.orderDays - daysSinceArrived;
        restTimeText.SetText(Setting.restTime + restTime + "天");
        rewardText.SetText(Setting.orderReward + order.orderRewardCount);
    }

    private void TryFinishOrder()
    {
        OrderController.Instance.TryFinishCurrentOrder();
    }
    private void ClosePanel()
    {
        OrderController.Instance.OpenOrCloseOrderPanel();
    }
}
