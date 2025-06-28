using System.Collections;
//using Unity.VisualScripting;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private float damage;
    [Header("Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    [Header("Sound")]
    [SerializeField] private AudioClip triggerSound;
    [SerializeField] private AudioClip fireSound;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Health playerHealth;
    private bool triggered;
    private bool active;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(playerHealth != null && active)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();
            if (!triggered)
                StartCoroutine(ActivateFiretrap());
            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

        private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            playerHealth = null;
    }

    private IEnumerator ActivateFiretrap()
    {
        triggered = true;
        SoundManager.instance.PlaySoundEffect(triggerSound);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(activationDelay);

        spriteRenderer.color = Color.white;
        SoundManager.instance.PlaySoundEffect(fireSound);
        active = true;
        animator.SetBool("activated", true);
        yield return new WaitForSeconds(activeTime);

        active = false;
        triggered = false;
        animator.SetBool("activated", false);
    }
}
