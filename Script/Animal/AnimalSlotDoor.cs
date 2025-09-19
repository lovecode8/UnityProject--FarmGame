using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//负责处理玩家碰撞动物圈的们时的交互效果
public class AnimalSlotDoor : MonoBehaviour
{
    public int animalId; //养殖的动物id
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Setting.playerTag)
        {
            AnimalSlotManager.Instance.ShowAnimalPanel(animalId);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == Setting.playerTag)
        {
            AnimalSlotManager.Instance.ClearAnimalPanel();
        }
    }
}
