using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] projectiles;

    [Header("Collider")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer;

    [Header("Sound")]
    [SerializeField] private AudioClip projectileSound;

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

        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                animator.SetTrigger("rangeAttack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private void RangeAttack()
    {
        SoundManager.instance.PlaySoundEffect(projectileSound);
        cooldownTimer = 0;
        projectiles[FindProjectile()].transform.position = firepoint.position;
        projectiles[FindProjectile()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindProjectile()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (!projectiles[i].activeInHierarchy)
                return i;
        }
        return 0;
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
}
