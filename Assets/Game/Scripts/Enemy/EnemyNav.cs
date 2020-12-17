using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints;
    [SerializeField] Transform camOrientation;

    private NavMeshAgent agent;
    private int curWaypointIndex = 0;

    private float minDistAway = 40f; // Distance that enemy can see players
    private float distToShoot = 15f; // Starts shooting if this far away from target
    private GameObject curTarget;    // Player target we are engaging in

    // Keps track of enemy state
    public enum State
    {
        Patrolling,
        ChaseTarget,
    }
    private State state;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // If there are no waypoints, set destination to current position
        if (waypoints.Count == 0)
            waypoints.Add(this.transform);
        agent.SetDestination(waypoints[0].position);

        state = State.Patrolling; // Enemy always starts as patrolling
        StartCoroutine(DoPlayerProximityCheck());
    }

    public State getState() { return state; }

    void Update()
    {
        switch (state)
        {
            default:
            case State.Patrolling:
                Patrol();
                Debug.Log("Patrolling");
                break;
            case State.ChaseTarget:
                ChaseTarget();
                Debug.Log("Chasing");
                break;
        }
    }

    // Checks player proximity every 0.1 seconds
    IEnumerator DoPlayerProximityCheck()
    {
        for(;;)
        {
            CheckPlayerProximity();
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Checks if there are any viewable or nearby players.
    // If so, switches to chasing target state.
    // If not, switches to patrolling state.
    void CheckPlayerProximity()
    {
        List<Transform> playerTransforms = LevelManager.Instance.getPlayerTransforms();
        Transform closestPlayer = null;
        float closestDist = Mathf.Infinity;

        // Loop through all players and find closest and checks if in view
        for (int i = 0; i < playerTransforms.Count; i++)
        {
            Vector3 playerPos = playerTransforms[i].GetComponentInChildren<Rigidbody>().transform.position; // super hacky
            Vector3 vecToPlayer = (playerPos - transform.position);
            bool inView = canViewPlayer(vecToPlayer, playerTransforms[i].gameObject);
            float distToPlayer = vecToPlayer.magnitude;

            if (inView && distToPlayer < minDistAway && distToPlayer < closestDist)
            {
                // Update closest player
                closestPlayer = playerTransforms[i];
                closestDist = distToPlayer;
            }
        }

        // No viewable/close players
        if (closestPlayer == null) {
            state = State.Patrolling;
            agent.SetDestination(waypoints[curWaypointIndex].position);
            return;
        }

        // Viewable player, switch to chase state
        curTarget = closestPlayer.GetComponentInChildren<Rigidbody>().gameObject; // super hacky
        state = State.ChaseTarget;
    }

    bool canViewPlayer(Vector3 vecToPlayer, GameObject player)
    {
        // Checks if player is within ~70 degree view of player
        bool inForwardView = Vector3.Dot(vecToPlayer.normalized, camOrientation.forward) > 0.2;
        Debug.DrawLine(camOrientation.position, camOrientation.position + vecToPlayer.normalized * 1000f, Color.white, 0.02f);
        
        RaycastHit hit;

        // Check if anything blocks ray to player
        if (inForwardView && Physics.Raycast(camOrientation.position, vecToPlayer.normalized, out hit, 1000f))
        {
            if (hit.transform.CompareTag("Player") || hit.transform.CompareTag("Clone"))
                return true;
        }
        return false;
    }


    // Sets destination to be chasing the target
    void ChaseTarget()
    {
        // Check if greater than shooting distance away, then go towards target
        if ((transform.position - curTarget.transform.position).magnitude > distToShoot) {
            agent.SetDestination(curTarget.transform.position);
        }
        // If close enough to target, face towards target instead
        else
        {
            Vector3 dir = curTarget.transform.position - transform.position;
            dir.y = 0; //This allows the object to only rotate on its y axis
            Quaternion rot = Quaternion.LookRotation(dir);
            float rotationSpeed = 6.0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
            // Set destination to itself so it doesn't move
            agent.SetDestination(transform.position);
        }
    }

    // If patrolling, checks if agent has reached it's destination.
    void Patrol()
    {
        if (agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {
            StartCoroutine(UpdateToNextWaypoint(2)); // Wait 2 seconds at destination
        }
    }


    IEnumerator UpdateToNextWaypoint(int secondsIdle)
    {
        curWaypointIndex++;
        curWaypointIndex = curWaypointIndex % waypoints.Count;

        // Enemy Idle when they reached a waypoint
        yield return new WaitForSeconds(secondsIdle);
        agent.SetDestination(waypoints[curWaypointIndex].position);
    }

}
