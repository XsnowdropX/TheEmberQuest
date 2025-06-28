using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private AudioClip shootSound;

    private float cooldownTimer;
    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
            Attack();
    }
    private void Attack()
    {
        cooldownTimer = 0;
        if (shootSound)
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        //SoundManager.instance.PlaySoundEffect(shootSound);
        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindArrow()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

}
