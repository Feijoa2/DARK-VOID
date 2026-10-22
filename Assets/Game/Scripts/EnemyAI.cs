using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Движение")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float stopDistance = 0.4f;

    [Header("Реакция на свет")]
    [SerializeField] private LightDetector lightDetector;
    [SerializeField] private float lightExposureToDie = 3f;
    [SerializeField] private float fleeDistance = 5f;
    [SerializeField] private float fleeSpeed = 3f;

    [Header("Стан после света")]
    [SerializeField] private float stunDuration = 1f;

    [Header("Смерть игрока")]
    [SerializeField] private string playerTag = "Player";

    [Header("Респавн")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float respawnDelay = 10f;

    private NavMeshAgent agent;
    private Transform player;
    private float lightTimer = 0f;
    private float stunTimer = 0f;
    private bool isDying = false;
    private bool isStunned = false;
    private bool wasInLightLastFrame = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
        agent.stoppingDistance = stopDistance;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (isDying || player == null) return;

        bool isInLight = lightDetector != null && lightDetector.IsInLight();

        // === Свет на руке ===
        if (isInLight)
        {
            // Вызываем событие ТОЛЬКО при первом попадании света
            if (!wasInLightLastFrame)
            {
                GameEvents.EnemyRepelled();
            }

            lightTimer += Time.deltaTime;
            wasInLightLastFrame = true;

            FleeFromPlayer();

            if (lightTimer >= lightExposureToDie)
                Die();

            return;
        }

        // === Свет ушёл: старт стана ===
        if (wasInLightLastFrame)
        {
            isStunned = true;
            stunTimer = stunDuration;
            wasInLightLastFrame = false;
        }

        lightTimer = 0f;

        // === Стан ===
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;

            if (stunTimer <= 0)
            {
                isStunned = false;
                agent.isStopped = false;
            }
            return;
        }

        // === Преследование ===
        agent.isStopped = false;
        agent.speed = moveSpeed;
        agent.SetDestination(player.position);
    }

    private void FleeFromPlayer()
    {
        if (player == null) return;

        Vector2 fleeDir = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + (Vector3)fleeDir * fleeDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.isStopped = false;
            agent.speed = fleeSpeed;
            agent.SetDestination(hit.position);
        }
    }

    private void Die()
    {
        if (isDying) return;
        isDying = true;

        GameEvents.EnemyKilled(); 

        agent.isStopped = true;
        gameObject.SetActive(false);
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        if (spawnPoint != null)
            transform.position = spawnPoint.position;

        gameObject.SetActive(true);
        isDying = false;
        isStunned = false;
        lightTimer = 0f;
        wasInLightLastFrame = false;
        agent.isStopped = false;
        agent.speed = moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (isDying || isStunned) return;
        if (other.CompareTag(playerTag))
            GameManager.Instance.GameOver();
    }
}

