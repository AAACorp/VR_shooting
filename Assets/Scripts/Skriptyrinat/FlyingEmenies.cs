using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEmenies : MonoBehaviour
{
    public int attackRange, sightRange;
    public float attackSpeed;
    public int damage, health;

    public NavMeshAgent agent;
    private Animator animator;
    private GameObject player;
    public GameObject projectile;
    public Transform ShootPlace;

    public float walkPointRange;
    bool walkPointSet;
    public Vector3 walkPoint;

    public LayerMask whatIsAir, whatIsPlayer, whatIsWall;

    private bool alreadyAttacked;
    public bool playerInSightRange, playerInAttackRange;

    private void Update()
    {
        if (!playerInSightRange && !playerInAttackRange ) Patroling();
        if (playerInSightRange && !playerInAttackRange ) ChaseToPlayer();
        if (playerInSightRange && playerInAttackRange) Attack();
    }

    void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distantToWalkPoint = transform.position - walkPoint;

        if (distantToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, whatIsAir))
        {
            if (!Physics.CheckSphere(walkPoint, 0.1f, whatIsWall))
            {
                walkPointSet = true;
            }

        }
    }

    void TakeDamage(int value)
    {
        health -= value;
        if(health<=0)
        {
            Destroy(gameObject);
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void Attack()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        agent.SetDestination(transform.position);
        transform.LookAt(player.transform.position);

        if (Physics.Raycast(transform.position, fwd, out hit))
        {
            if (hit.transform.tag == "Wall")
            {
                playerInAttackRange = false;
                ChaseToPlayer();
            }

            else
            {
                if (!alreadyAttacked)
                {
                    Rigidbody rb = Instantiate(projectile, ShootPlace.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward * 40f, ForceMode.Impulse);
                    alreadyAttacked = true;
                    animator.SetTrigger("Shoot");
                    Invoke(nameof(ResetAttack), attackSpeed);
                }
            }
        }
    }

    void ChaseToPlayer()
    {
        agent.SetDestination(player.transform.position);
    }
}
