using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public sealed class EnemyPatrolChase : MonoBehaviour
{
    private enum State { Patrol, Chase, Search } // Додано стан Search

    [SerializeField] private Transform target;
    [SerializeField] private Transform[] waypoints;
    [SerializeField, Min(0.1f)] private float detectRange = 6f;
    [SerializeField, Min(0.2f)] private float loseRange = 9f;
    [SerializeField, Min(0.05f)] private float decisionInterval = 0.25f;
    [SerializeField, Min(0.1f)] private float sampleRadius = 1f;

    [Header("Line of Sight (Extra Task)")]
    [SerializeField] private LayerMask obstacleMask; // Маска для стін/перешкод
    [SerializeField, Min(0.1f)] private float eyeHeight = 1.5f; // Висота для променя, щоб не бити в землю

    private NavMeshAgent agent;
    private State state;
    private int waypointIndex;
    private float nextDecision;
    private bool requestedPath;
    private bool warnedMissingMesh;

    private Vector3 lastKnownPosition; // Пам'ять про останню видиму позицію

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        loseRange = Mathf.Max(loseRange, detectRange + 0.1f);
    }

    private void Update()
    {
        if (!agent.isActiveAndEnabled || !agent.isOnNavMesh)
        {
            if (!warnedMissingMesh)
            {
                Debug.LogWarning("Enemy needs an active NavMesh.", this);
                warnedMissingMesh = true;
            }
            requestedPath = false;
            return;
        }
        warnedMissingMesh = false;

        if (Time.time < nextDecision) return;
        nextDecision = Time.time + decisionInterval;

        float distance = target == null ? float.PositiveInfinity
            : Vector3.Distance(transform.position, target.position);

        // Перевірка видимості (Linecast)
        bool isVisible = false;
        if (target != null && distance <= loseRange)
        {
            Vector3 eyePos = transform.position + Vector3.up * eyeHeight;
            Vector3 targetEyePos = target.position + Vector3.up * eyeHeight;
            // Промінь перевіряє ТІЛЬКИ перешкоди, ігноруючи самого себе та землю
            isVisible = !Physics.Linecast(eyePos, targetEyePos, obstacleMask);
        }

        State wanted = state;

        // Логіка переходів між станами
        if (state == State.Patrol)
        {
            if (distance <= detectRange && isVisible)
                wanted = State.Chase;
        }
        else if (state == State.Chase)
        {
            if (distance >= loseRange)
                wanted = State.Patrol;
            else if (!isVisible)
                wanted = State.Search; // Гравець сховався за стіну
        }
        else if (state == State.Search)
        {
            if (isVisible)
                wanted = State.Chase; // Гравець знову з'явився
        }

        if (wanted != state)
        {
            // Якщо втратили ціль з поля зору, запам'ятовуємо координату
            if (state == State.Chase && wanted == State.Search && target != null)
            {
                lastKnownPosition = target.position;
            }

            state = wanted;
            agent.ResetPath();
            requestedPath = false;
            Debug.Log("AI state: " + state, this);
        }

        if (agent.pathPending) return;

        // Рух: Переслідування
        if (state == State.Chase)
        {
            if (target != null) RequestDestination(target.position);
            return;
        }

        // Рух: Пошук останнього відомого місця
        if (state == State.Search)
        {
            RequestDestination(lastKnownPosition);

            if (requestedPath)
            {
                bool invalid = !agent.hasPath || agent.pathStatus != NavMeshPathStatus.PathComplete;
                bool arrived = !invalid && agent.remainingDistance <= agent.stoppingDistance + 0.1f;

                // Якщо дійшли до місця (або воно недоступне), а гравця не видно - повертаємось на патруль
                if (invalid || arrived)
                {
                    state = State.Patrol;
                    agent.ResetPath();
                    requestedPath = false;
                    Debug.Log("AI state: " + state, this);
                }
            }
            return;
        }

        // Рух: Патрулювання
        if (waypoints == null || waypoints.Length == 0)
        {
            if (agent.hasPath) agent.ResetPath();
            return;
        }

        if (requestedPath)
        {
            bool invalid = !agent.hasPath ||
                agent.pathStatus != NavMeshPathStatus.PathComplete;
            bool arrived = !invalid &&
                agent.remainingDistance <= agent.stoppingDistance + 0.1f;
            
            if (!invalid && !arrived) return;
            
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            requestedPath = false;
        }

        Transform point = waypoints[waypointIndex];
        if (point == null || !RequestDestination(point.position))
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
    }

    private bool RequestDestination(Vector3 position)
    {
        var filter = new NavMeshQueryFilter
        {
            agentTypeID = agent.agentTypeID,
            areaMask = agent.areaMask
        };

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit,
            sampleRadius, filter))
        {
            agent.ResetPath();
            requestedPath = false;
            return false;
        }

        requestedPath = agent.SetDestination(hit.position);
        return requestedPath;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, loseRange);

        // Відображення червоної точки, куди йде ворог під час стану Search
        if (Application.isPlaying && state == State.Search)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastKnownPosition, 0.5f);
        }
    }
}