using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OrderCarController : MonoBehaviour
{
    NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void MoveToDestination(Transform destination)
    {
        agent.destination = destination.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == Setting.playerTag)
        {
            OrderController.Instance.OpenOrCloseOrderPanel();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == Setting.playerTag)
        {
            OrderController.Instance.OpenOrCloseOrderPanel();
        }
    }
}
