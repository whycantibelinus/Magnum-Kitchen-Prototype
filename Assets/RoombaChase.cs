using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class RoombaNavWanderChase : MonoBehaviour
{
    public Transform target; // assign player or leave empty to auto-find by tag "Player"

    [Header("Wander")]
    public float wanderRadius = 6f;
    public float wanderTimer = 3.5f;
    public float wanderSpeed = 2.5f;

    [Header("Chase")]
    public float chaseSpeed = 4.0f;
    public float viewDistance = 10f;
    public float loseSightDelay = 1.25f;

    [Header("LOS")]
    public LayerMask obstructionMask; // set to Everything except Player (and Floor is fine)
    public Vector3 eyeOffset = new Vector3(0f, 0.15f, 0f);
    public float blindHeight = 0.35f;  // if player is above this, Roomba "can't see" (rideable)

    private NavMeshAgent agent;
    private float timer;
    private float lastSeenTime = -999f;
    private Vector3 wanderDestination;

    private enum State { Wander, Chase }
    private State state = State.Wander;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }

        timer = wanderTimer;
        agent.speed = wanderSpeed;
        wanderDestination = transform.position;

        // Roomba-ish handling
        agent.acceleration = 12f;
        agent.angularSpeed = 180f;
        agent.stoppingDistance = 0.1f;
    }

    void Update()
    {
        bool canSee = CanSeeTarget();

        if (canSee)
        {
            lastSeenTime = Time.time;
            state = State.Chase;
        }
        else if (Time.time - lastSeenTime > loseSightDelay)
        {
            state = State.Wander;
        }

        if (state == State.Chase && target != null)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.speed = wanderSpeed;
            timer -= Time.deltaTime;

            if (!agent.hasPath || agent.remainingDistance < 0.25f || timer <= 0f)
            {
                wanderDestination = RandomNavPoint(transform.position, wanderRadius);
                agent.SetDestination(wanderDestination);
                timer = wanderTimer;
            }
        }
    }

    bool CanSeeTarget()
    {
        if (target == null) return false;

        // Rideable = blind
        if (target.position.y > transform.position.y + blindHeight)
            return false;

        Vector3 eyePos = transform.position + eyeOffset;
        Vector3 toTarget = (target.position + Vector3.up * 0.3f) - eyePos;

        float dist = toTarget.magnitude;
        if (dist > viewDistance) return false;

        if (Physics.Raycast(eyePos, toTarget.normalized, out RaycastHit hit, dist, obstructionMask, QueryTriggerInteraction.Ignore))
        {
            return hit.transform == target || hit.transform.IsChildOf(target);
        }

        return true;
    }

    Vector3 RandomNavPoint(Vector3 origin, float radius)
    {
        Vector3 rand = Random.insideUnitSphere * radius + origin;

        if (NavMesh.SamplePosition(rand, out NavMeshHit hit, radius, NavMesh.AllAreas))
            return hit.position;

        return origin;
    }
}