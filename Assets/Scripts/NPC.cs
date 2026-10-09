using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [Header("NPC Settings")]
    NavMeshAgent agent;
    Animator animator;

    [Header("Target Settings")]
    public Transform target_npc;
    public float walkRadius = 10.0f;

    public float idleTime = 2f;
    float idleTimer;

    [Header("State NPC ( Idle / Moving )")]
    string state = "Idle";

    [Header("Chair Target")]
    Chair targetChair;

    private float dropTimer = 10f;
    int randomInt = 0;
    [SerializeField] private GameObject cup;

    public bool isDroppedCupAlreadyHandled = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Debug.Log("NPC on NavMesh: " + agent.isOnNavMesh);

        if (agent.isOnNavMesh)
        {
            MoveToRandomPosition();
            // ("NPC Move to Random position in start!");
        }
    }

    void Update()
    {

        if (!isDroppedCupAlreadyHandled)
        {
            dropTimer -= Time.deltaTime;

            if (dropTimer <= 0f)
            {
                HandlingDroppedCup();
            }
        }

        // Debug.Log("State : " + state);
        if (state == "Moving")
        {
            animator.SetBool("IsMoving", true);

            if (HasArrived())
            {
                state = "Idle";
                idleTimer = idleTime;

                // Debug.Log("NPC arrived. Now Idle.");
            }
        }
        else if (state == "Idle")
        {
            idleTimer -= Time.deltaTime;

            if (idleTimer <= 0)
            {
                FindChair();
            }
        }
        else if (state == "GoingToChair")
        {
            animator.SetBool("IsMoving", true);
            if (HasArrived())
            {
                SitDown();
            }
        }
    }

    public bool HasArrived()
    {
        if (agent.pathPending) return false;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

    public void MoveToRandomPosition()
    {
        Vector3 nextRandomDirection = Random.insideUnitSphere * walkRadius;

        nextRandomDirection += transform.position;

        NavMeshHit hit;
        animator.SetBool("IsMoving", true);

        if (NavMesh.SamplePosition(nextRandomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            state = "Moving";
            animator.SetBool("IsMoving", true);
        }

    }

    public void FindChair()
    {
        Chair[] chairs = FindObjectsByType<Chair>(FindObjectsSortMode.None);

        foreach (Chair chair in chairs)
        {
            if (!chair.isOccupied)
            {
                targetChair = chair;

                chair.isOccupied = true;

                agent.SetDestination(chair.sitPoint.position);

                state = "GoingToChair";

                return;
            }
        }
        MoveToRandomPosition();
    }

    public void SitDown()
    {
        agent.isStopped = true;

        transform.position = targetChair.sitPoint.position;
        transform.rotation = targetChair.sitPoint.rotation;

        state = "Sitting";

        animator.SetBool("IsMoving", false);
        animator.SetBool("IsSitting", true);
    }

    private void HandlingDroppedCup()
    {
        if (isDroppedCupAlreadyHandled)
            return;

        if (cup == null)
        {
            Debug.LogWarning("Cup belum diassign ke NPC.");
            return;
        }

        Rigidbody cupRigidbody = cup.GetComponent<Rigidbody>();

        if (cupRigidbody != null)
        {
            cup.transform.SetParent(null);

            cupRigidbody.isKinematic = false;
            cupRigidbody.useGravity = true;
        }

        isDroppedCupAlreadyHandled = true;

        Debug.Log("NPC menjatuhkan Cup!");
    }
}
