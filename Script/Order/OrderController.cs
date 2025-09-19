using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//订单系统控制器
public class OrderController : SingletonMonoBehavior<OrderController>
{
    public GameObject orderCarPrefab; //订单车预制体
    public Transform newOrderCarInstantiateTransform;
    public GameObject orderPanel; //订单显示面板引用
    public SO_OrderDetails so_OrderDetails;
    public Transform carDestination;
    public Transform carEndPosition;

    private GameObject currentOrderCar; //现在正在等待的订单车
    private GameObject newOrderCar; //新的订单车
    private OrderDetails currentOrderDetails; //现在的订单信息
    private int daysSinceArrived = -1; //订单车等待的天数
    private bool isOrderPanelOpen; //订单界面是否打开
    void Start()
    {
        // CreateNewOrder(so_OrderDetails.GetOrderDetails(5001));
    }
    void OnEnable()
    {
        EventHolder.onGameDayEvent += OrderDayEvent;
    }
    void OnDisable()
    {
        EventHolder.onGameDayEvent -= OrderDayEvent;
    }
    private void OrderDayEvent(int year, Season season, int day, int hour, int mintue)
    {
        daysSinceArrived++;
        if(currentOrderDetails!=null)
        Debug.Log(daysSinceArrived + "  " + currentOrderDetails.orderDays);
        if (currentOrderDetails != null && daysSinceArrived == currentOrderDetails.orderDays)
        {
            //订单自动结束
            StartCoroutine(CurrentOrderEnd());
        }
        OrderDetails newOrder = FindNewOrder(day);
        if (newOrder != null) //生成新订单
        {
            daysSinceArrived = 0;
            CreateNewOrder(newOrder);
        }
    }

    private OrderDetails FindNewOrder(int gameDay)
    {
        foreach (OrderDetails order in so_OrderDetails.orderDetailsList)
        {
            if (gameDay == order.orderAppearDay)
            {
                return order;
            }
        }
        return null;
    }
    private void CreateNewOrder(OrderDetails order)
    {
        newOrderCar = GameObject.Instantiate
        (orderCarPrefab, newOrderCarInstantiateTransform.position, Quaternion.identity);

        OrderCarController orderCar = newOrderCar.GetComponent<OrderCarController>();
        orderCar.MoveToDestination(carDestination);

        currentOrderDetails = order;
    }

    public void OpenOrCloseOrderPanel()
    {
        RectTransform rectTransform = orderPanel.GetComponent<RectTransform>();
        if (!isOrderPanelOpen)
        {
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x -
            Setting.orderPanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);

            orderPanel.GetComponent<OrderPanel>().UpdateOrderPanel(currentOrderDetails, daysSinceArrived);
            isOrderPanelOpen = true;
        }
        else
        {
            rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x +
            Setting.orderPanelMoveDistance, Setting.panelMoveDuration).SetEase(Ease.OutBack);

            isOrderPanelOpen = false;
        }
    }

    public void TryFinishCurrentOrder()
    {
        List<int> orderItemIdList = currentOrderDetails.orderItemIdList;
        List<int> orderItemCountList = currentOrderDetails.orderItemNumList;

        //检查数量
        for (int i = 0; i < Setting.orderItemCount; i++)
        {
            int orderItemId = orderItemIdList[i];
            int orderItemCount = orderItemCountList[i];
            if (!InventoryManager.Instance.FindItemInInventoryWithCount(orderItemId, orderItemCount))
            {
                EventHolder.CallonShowAdmireEvent("你还没有完成订单！");
                return;
            }
        }

        //真正交付
        for (int i = 0; i < Setting.orderItemCount; i++)
        {
            InventoryManager.Instance.RemoveItem(orderItemIdList[i], orderItemCountList[i]);
        }
        InventoryManager.Instance.AddCoin(currentOrderDetails.orderRewardCount);
        AudioManager.Instance.PlaySound("buyItem");
        EventHolder.CallonShowAdmireEvent("订单交付成功！");
        StartCoroutine(CurrentOrderEnd());
    }
    IEnumerator CurrentOrderEnd()
    {
        currentOrderCar = newOrderCar;
        OrderCarController car = currentOrderCar.GetComponent<OrderCarController>();
        car.MoveToDestination(carEndPosition);
        yield return Setting.carDisappearTime;
        Destroy(currentOrderCar.gameObject);
    }
}
