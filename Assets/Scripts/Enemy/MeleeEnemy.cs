using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;

    [Header("Collider")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer;

    [Header("Sound")]
    [SerializeField] private AudioClip attackSound;

    private Animator animator;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        cooldownTimer = attackCooldown + 1;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(PlayerInSight())
        {
            if(cooldownTimer>=attackCooldown && playerHealth.currentHealth>0)
            {
                cooldownTimer = 0;
                animator.SetTrigger("meleeAttack");
                SoundManager.instance.PlaySoundEffect(attackSound);
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * 
            transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, 
            boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * 
            transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, 
            boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
