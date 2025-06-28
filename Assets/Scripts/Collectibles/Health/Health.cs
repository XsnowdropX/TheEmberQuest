using System;
using System.Collections;
//using Unity.Mathematics;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] public float startingHealth;
    public float currentHealth { get; private set; }
    private Animator animator;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int noOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Sound")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip dieSound;

    public Action<float> OnHealthChanged;
    private bool isDodgeInvulnerable = false; //for boss

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float _damage)
    {
        if (isDodgeInvulnerable) return;
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if(currentHealth > 0)
        {
            animator.SetTrigger("hurt");
            if(hurtSound)
                SoundManager.instance.PlaySoundEffect(hurtSound);
            StartCoroutine(Invulnerability());
        }
        else
        {
            if(!dead)
            {
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                    if(component.gameObject.tag != "Player")
                        Destroy(component.gameObject, 1f);
                }

                animator.SetTrigger("die");

                dead = true;
                if(dieSound)
                    SoundManager.instance.PlaySoundEffect(dieSound);
                
            }
        }
        OnHealthChanged?.Invoke(currentHealth);
    }

    public void Heal(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void BuyHealth(float amount)
    {
        startingHealth += amount;
        currentHealth = startingHealth;
    }

    private IEnumerator Invulnerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(8,9,true);
        for (int i = 0; i < noOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (noOfFlashes*2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (noOfFlashes*2));
        }
        Physics2D.IgnoreLayerCollision(8,9,false);
        invulnerable = false;
    }

    public void PlayerRespawn()
    {
        dead = false;

        Heal(startingHealth);
        animator.SetBool("grounded", true);
        animator.ResetTrigger("die");
        animator.Play("Idle");
        StartCoroutine(Invulnerability());

        foreach (Behaviour component in components)
        component.enabled = true;
    }
    public void SetInvulnerableForDodge(bool status)
    {
        isDodgeInvulnerable = status;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
