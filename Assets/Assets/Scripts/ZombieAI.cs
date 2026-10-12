using UnityEngine;

/// <summary>
/// Zombie AI: chases the player/agent using Rigidbody so walls block it correctly.
/// </summary>
public class ZombieAI : MonoBehaviour
{
    [Header("Chase Settings")]
    public float speed = 2f;
    public float wanderStrength = 0.3f;

    private Transform target;
    private Vector3 wanderOffset;
    private float wanderTimer;
    private Rigidbody rb;

    void Start()
    {
        // Get or add Rigidbody — required for wall collisions to work
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity  = true;
        rb.freezeRotation = true;

        FindTarget();
        RefreshWander();
    }

    void FindTarget()
    {
        var go = GameObject.FindWithTag("Agent");
        if (go == null) go = GameObject.FindWithTag("Player");
        if (go != null) target = go.transform;
    }

    void FixedUpdate()
    {
        if (target == null) { FindTarget(); return; }

        wanderTimer -= Time.fixedDeltaTime;
        if (wanderTimer <= 0f) RefreshWander();

        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 lateral = Vector3.Cross(dir, Vector3.up) * wanderOffset.x;
        Vector3 moveDir = (dir + lateral * wanderStrength).normalized;

        // Use Rigidbody velocity so physics walls block movement
        Vector3 vel = moveDir * speed;
        vel.y = rb.linearVelocity.y; // preserve gravity
        rb.linearVelocity = vel;

        // Face movement direction
        if (moveDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(moveDir),
                Time.fixedDeltaTime * 8f
            );
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Agent"))
        {
            var ph = other.GetComponent<PlayerHealth>();
            if (ph != null) ph.TakeDamage(1);

            var sa = other.GetComponent<SurvivorAgent>();
            if (sa != null)
            {
                sa.AddReward(-1f);
                sa.EndEpisode();
            }
        }
    }

    private void RefreshWander()
    {
        wanderOffset = new Vector3(Random.Range(-1f, 1f), 0f, 0f);
        wanderTimer  = Random.Range(1f, 2f);
    }
}