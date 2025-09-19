using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderDetailsList", menuName = "ScriptableObjects/OrderDetailsList")]
public class SO_OrderDetails : ScriptableObject
{
    [SerializeField]
    public List<OrderDetails> orderDetailsList;

    public OrderDetails GetOrderDetails(int id)
    {
        return orderDetailsList.Find(x => x.orderId == id);
    }
}
