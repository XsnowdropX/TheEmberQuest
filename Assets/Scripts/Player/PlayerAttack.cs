using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    [SerializeField] private AudioClip projectileSound;
    private Animator animator;
    private PlayerMovement playerMovement;
    private float cooldownTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        cooldownTimer = attackCooldown + 1;
    }

    private void Update()
    {
        if ((Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.KeypadEnter)) && 
            cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        SoundManager.instance.PlaySoundEffect(projectileSound);
        animator.SetTrigger("attack");
        cooldownTimer = 0;

        Boss boss = FindFirstObjectByType<Boss>();
        if (boss != null)
        {
            boss.TryToDodge();
        }

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<FireballProjectile>()
            .SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for(int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    public void UpgradeFireball(float speed, float damage)
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if(fireballs[i].GetComponent<FireballProjectile>().speed < 
                fireballs[i].GetComponent<FireballProjectile>().maxSpeed)
                fireballs[i].GetComponent<FireballProjectile>().speed += speed;
            if (fireballs[i].GetComponent<FireballProjectile>().damage < 
                fireballs[i].GetComponent<FireballProjectile>().maxDamage)
                fireballs[i].GetComponent<FireballProjectile>().damage += damage;
        }
    }
}
