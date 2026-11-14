using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }

    [Header("States")]
    [SerializeField] private AIState currentState = AIState.Patrol;

    [Header("Detect Settings")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private float attackRadius = 2f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 2f;
    private int currentPatrolIndex;
    private bool isWaitingAtPoint;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1.5f;
    private bool isAttacking = false;

    private NavMeshAgent agent;
    private Transform playerTransform;

    private bool isStunned;

    private HealthSystem healthSystemPlayer;
    private Attack attack;

    //Color 
    private Renderer enemyRenderer;
    private Color originalColor;
      
   
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attack = GetComponent<Attack>();
        enemyRenderer = GetComponent<Renderer>();
    }
    void Start()
    {
        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.material.color;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            healthSystemPlayer = playerObj.GetComponent<HealthSystem>();
        }
        else
        {
            Debug.LogError(" No  hay GameObjects con  tag Player");
        }

        if (patrolPoints.Length > 0)
        {
            GoToNextPatrolPoint();
        }
    }

    void Update()
    {
        if (isStunned || playerTransform == null || healthSystemPlayer != null && healthSystemPlayer.IsDead)
        {
            agent.isStopped = true;
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case AIState.Patrol:
                UpdatePatrolState(distanceToPlayer);
                break;
            case AIState.Chase:
                UpdateChaseState(distanceToPlayer);
                break;
            case AIState.Attack:
                UpdateAttackState(distanceToPlayer);
                break;
        }
    }

    private void UpdatePatrolState(float distanceToPlayer)
    {
        agent.isStopped = false;

        if (distanceToPlayer <= detectionRadius)
        {
            currentState = AIState.Chase;
            return;
        }
        if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)
        {
            if (!isWaitingAtPoint)
            {
                StartCoroutine(WaitAndMoveToNextPoint());
            }
        }
    }

    private void UpdateChaseState(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(playerTransform.position);

        if (distanceToPlayer <= attackRadius)
        {
            currentState = AIState.Attack;
            return;
        }

        if (distanceToPlayer > detectionRadius * 1.2f)
        {
            currentState = AIState.Patrol;
            GoToNextPatrolPoint();
        }
               
    }

    private void UpdateAttackState(float distanceToPlayer)
    {

        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        
        // Transicion de regreso a Persecucion si se aleja

        if (distanceToPlayer > attackRadius && !isAttacking)
        {
            currentState = AIState.Chase;
            isAttacking = false;
            if (enemyRenderer != null)
            {
                enemyRenderer.material.color = originalColor;
            }
            return;
        }
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(AttackCooldown());
        }

    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        agent.isStopped = true;
        ExecuteAttack();
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        agent.isStopped = false;


    }

    private void ExecuteAttack()
    {
        
        if (healthSystemPlayer != null)
        {
            Debug.Log("Ataque  contra el jugador");
            if (enemyRenderer != null)
            {
                enemyRenderer.material.color = Color.black;
            }
            attack.Melee();
        }


    }
   
    private IEnumerator WaitAndMoveToNextPoint()
    {
        isWaitingAtPoint = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(waitTimeAtPoint);

        if (patrolPoints.Length > 0)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            GoToNextPatrolPoint();
        }

        isWaitingAtPoint = false;
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
    private void OnDrawGizmosSelected()
    {
        // Radio de deteccion (Amarillo)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Radio de ataque (Rojo)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        agent.isStopped = true;

        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = Color.white;
        }
        Debug.Log($"Aturdido por {duration} segundos");

        yield return new WaitForSeconds(duration);
        if (enemyRenderer != null)
        {
            enemyRenderer.material.color = originalColor;
        }
        isStunned = false;
        agent.isStopped = false;
        Debug.Log("Recuperado del aturdimiento");
    }
}
