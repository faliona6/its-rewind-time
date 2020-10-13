using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;

    // So far very barebones and bad, but will add more stuff later

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // For setting position, we'll eventually need to get a list of all 
        // the clones, and (maybe) pick whichever one's closest/in line of sight.
        agent.SetDestination(target.position);

        // also need to do shooting if they're a certain distance away
    }
}
