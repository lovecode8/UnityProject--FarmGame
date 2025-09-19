using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//物品信息类
[System.Serializable]
public class ItemDetails
{

    public int itemId;
    public string itemName;
    public ItemType itemType;
    public int itemPrice;
    public string itemDescription;

    public Sprite itemSprite;

    public bool canBePickedUp;
    public bool canBeEaten;
    public bool canBeDraped;
    public bool canBeCarried;
}
