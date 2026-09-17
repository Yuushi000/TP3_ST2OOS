using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("Deplacement")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;

    [Header("Attaque")]
    public float attackInterval = 1.5f;

    [Header("References")]
    public Animator animator;
    public CharacterStats myStats;
    public Rigidbody rb;

    private CharacterStats playerStats;
    private Transform playerTransform;
    private bool playerDetected = false;
    private bool playerInAttackRange = false;
    private float attackTimer = 0f;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    public void OnPlayerEnterDetection(Collider player)
    {
        playerTransform = player.transform;
        playerStats = player.GetComponent<CharacterStats>();
        playerDetected = true;
    }

    public void OnPlayerExitDetection(Collider player)
    {
        playerDetected = false;
        playerTransform = null;
        playerStats = null;

        if (animator != null)
            animator.SetFloat("Speed", 0f);

        if (rb != null)
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
    }

    public void OnPlayerEnterAttackRange(Collider player)
    {
        playerInAttackRange = true;
        attackTimer = attackInterval;
    }

    public void OnPlayerExitAttackRange(Collider player)
    {
        playerInAttackRange = false;
    }

    void Update()
    {
        if (playerInAttackRange && playerStats != null)
        {
            // Arrete de bouger et attaque
            if (rb != null)
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

            if (animator != null)
                animator.SetFloat("Speed", 0f);

            FaceTarget();

            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                attackTimer = 0f;
                Attack();
            }
        }
        else if (playerDetected && playerTransform != null)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f;
        direction.Normalize();

        Vector3 targetVelocity = direction * moveSpeed;

        if (rb != null)
            rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        if (animator != null)
            animator.SetFloat("Speed", moveSpeed);

        FaceTarget();
    }

    void FaceTarget()
    {
        if (playerTransform == null) return;

        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        if (playerStats != null && myStats != null)
            playerStats.TakeDamage(myStats.attackPower);
    }
}