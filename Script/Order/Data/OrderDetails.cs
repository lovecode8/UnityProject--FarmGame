using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//订单类-- 记录订单信息
[Serializable]
public class OrderDetails
{
    public int orderId; //订单id
    public int orderAppearDay; //订单在第几天出现
    public List<int> orderItemIdList; //订单所需物品id
    public List<int> orderItemNumList; //对应物品数量
    public int orderDays; //订单限时
    public int orderRewardCount; //订单奖励数
}
