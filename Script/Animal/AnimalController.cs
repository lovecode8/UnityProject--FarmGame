using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalController : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    private float timer;
    private float currentWaitTime;
    private AnimalState animalState;
    private AnimalState nextAnimalState;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool(Setting.animalWalk, true);
        agent.destination = GetRandomDestination();
        animalState = AnimalState.walk;
    }
    void Update()
    {
        if (animalState != nextAnimalState)
        {
            animalState = nextAnimalState;
        }

        AnimalControl();
    }
    private void AnimalControl()
    {
        switch (animalState)
        {
            case AnimalState.idle:
                AnimalOnIdle();
                break;
            case AnimalState.walk:
                AnimalOnWalk();
                break;
            default:
                break;
        }

    }
    private void AnimalOnIdle()
    {
        timer += Time.deltaTime;
        if (timer > currentWaitTime)
        {
            animator.SetBool(Setting.animalWalk, true);
            agent.destination = GetRandomDestination();
            nextAnimalState = AnimalState.walk;
            timer = 0;
        }
    }
    private void AnimalOnWalk()
    {
        timer += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, agent.destination);
        if (distance < Setting.animalChangeStateMinDistance || timer > currentWaitTime)
        {
            currentWaitTime = GetRandomTime();
            animator.SetBool(Setting.animalWalk, false);
            nextAnimalState = AnimalState.idle;
        }
    }
    private Vector3 GetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * Setting.animalwalkMaxDistance;
        return transform.position + randomDirection;
    }
    private float GetRandomTime()
    {
        return Random.Range(Setting.animalChangeStateMinTime, Setting.animalChangeStateMaxTime);
    }
}
