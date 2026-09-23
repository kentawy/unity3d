using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    public Transform player; 
    public Transform[] patrolPoints; 
    
    [Header("Налаштування AI")]
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float health = 100f; // Додаємо здоров'я для тригеру втечі
    public float idleWaitTime = 2f; // Час очікування на точці (стан Спокій)

    private NavMeshAgent agent;
    private int currentPatrolIndex;
    private float idleTimer;
    
    // Повний набір станів для Завдання 4
    private enum State { Idle, Patrol, Chase, Attack, Flee }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = State.Idle;
    }

    void Update()
    {
        // 1. Пріоритетна перевірка на втечу (якщо мало ХП)
        if (health < 30f)
        {
            currentState = State.Flee;
        }
        else
        {
            // 2. Стандартна логіка перемикання станів за відстанню
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                currentState = State.Attack;
            }
            else if (distanceToPlayer <= chaseRange)
            {
                currentState = State.Chase;
            }
            else if (currentState == State.Chase || currentState == State.Attack)
            {
                // Якщо гравець втік за межі chaseRange, ворог заспокоюється
                currentState = State.Idle;
            }
        }

        // Виконання дій залежно від поточного стану
        switch (currentState)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
            case State.Attack:
                AttackBehavior();
                break;
            case State.Flee:
                FleeBehavior();
                break;
        }
    }

    void IdleBehavior()
    {
        agent.isStopped = true;
        idleTimer += Time.deltaTime;

        // Стоїмо на місці, поки не спливе час, потім йдемо патрулювати
        if (idleTimer >= idleWaitTime)
        {
            idleTimer = 0f;
            GotoNextPatrolPoint();
            currentState = State.Patrol;
        }
    }

    void PatrolBehavior()
    {
        if (patrolPoints.Length == 0) return;

        // Якщо агент дійшов до точки — переходимо в режим спокою
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentState = State.Idle;
        }
    }

    void ChaseBehavior()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void AttackBehavior()
    {
        agent.isStopped = true;
        // Тут можна додати логіку нанесення шкоди гравцю
    }

    void FleeBehavior()
    {
        agent.isStopped = false;
        
        // Розраховуємо вектор напрямку ВІД гравця
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        
        // Задаємо точку для втечі на відстані 5 метрів від ворога
        Vector3 fleeTarget = transform.position + fleeDirection * 5f;
        agent.SetDestination(fleeTarget);
    }

    void GotoNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.isStopped = false;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }
}