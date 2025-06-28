using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    private enum BossState { Patrol, Chase, Attack, Pause, Dodge, Hurt, Dead }
    private BossState currentState;
    [Header("Base")]
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [Header("Patrol")]
    [SerializeField] private Transform checkGround;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [Header("FOV")]
    [SerializeField] private Transform player;
    [SerializeField] private float fieldOfView;
    [Header("Attack")]
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackPointRadius;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackDuration;
    [SerializeField] private float pauseDuration;
    [SerializeField] private float hurtDuration;
    [Header("Dodge")]
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float dodgeDistance;
    [SerializeField] private float dodgeJumpForce;
    [Header("Victory")]
    [SerializeField] private GameObject victoryTrophy;
    private int currentStage = 1;
    private float dodgeChance = 0f;
    private float baseDamage;
    private float baseSpeed;
    private bool facingLeft = true;
    private Animator animator;
    private Health health;
    private Rigidbody2D rb;
    private void Awake(){
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();

        baseDamage = damage;
        baseSpeed = speed;

        if (victoryTrophy != null)
        {
            victoryTrophy.SetActive(false);
        }
    }
    private void OnEnable(){
        health.OnHealthChanged += HealthChanged;
    }
    private void OnDisable(){
        health.OnHealthChanged -= HealthChanged;
    }
    private void Start(){
        SwitchState(BossState.Patrol);
    }
    private void Update(){
        if (currentState == BossState.Dead) return;
        switch (currentState){
            case BossState.Patrol:
                PatrolState();
                CheckForPlayer();
                break;
            case BossState.Chase:
                ChaseState();
                break;
        }
    }
    private void SwitchState(BossState newState){
        if (currentState == newState) return;
        currentState = newState;
        StopAllCoroutines();
        switch (currentState){
            case BossState.Attack:
                StartCoroutine(AttackCoroutine());
                break;
            case BossState.Pause:
                StartCoroutine(PauseCoroutine());
                break;
            case BossState.Dodge:
                StartCoroutine(DodgeCoroutine());
                break;
            case BossState.Hurt:
                StartCoroutine(HurtCoroutine());
                break;
            case BossState.Dead:
                //handled in health
                break;
        }
    }
    private void PatrolState(){
        animator.SetBool("attacking", false);
        transform.Translate(Vector2.left * Time.deltaTime * speed);
        RaycastHit2D groundHit = Physics2D.Raycast(checkGround.position, 
            Vector2.down, rayDistance, groundLayer);
        RaycastHit2D wallHit = Physics2D.Raycast(checkGround.position, 
            Vector2.left, rayDistance, wallLayer);
        wallHit = Physics2D.Raycast(checkGround.position, Vector2.right, rayDistance, wallLayer);
        if (!groundHit || wallHit){
            TurnAround();
        }
    }
    private void ChaseState(){
        animator.SetBool("attacking", false);
        if (Vector2.Distance(transform.position, player.position) > fieldOfView){
            SwitchState(BossState.Patrol);
            return;
        }
        if (player.position.x > transform.position.x && facingLeft) TurnAround();
        else if (player.position.x < transform.position.x && !facingLeft) TurnAround();
        if (Vector2.Distance(transform.position, player.position) <= attackRange){
            SwitchState(BossState.Attack);
        }else{
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(player.position.x, transform.position.y),
                (speed + (Vector2.Distance(transform.position, player.position) / 2)) * Time.deltaTime);
        }
    }
    private IEnumerator AttackCoroutine(){
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("attacking", true);
        yield return new WaitForSeconds(attackDuration);
        animator.SetBool("attacking", false);
        SwitchState(BossState.Pause);
    }
    private IEnumerator PauseCoroutine(){
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(pauseDuration);
        SwitchState(BossState.Chase);
    }
    public void TryToDodge(){
        if ((currentState == BossState.Chase || currentState == BossState.Attack) && dodgeChance > 0){
            if (Random.value < dodgeChance){
                SwitchState(BossState.Dodge);
            }
        }
    }
    private IEnumerator DodgeCoroutine(){
        health.SetInvulnerableForDodge(true);
        Vector2 dodgeDirection = (transform.position - player.position).normalized;
        Vector2 startPos = transform.position;
        Vector2 targetPos = startPos + dodgeDirection * dodgeDistance;
        float timeToDodge = dodgeDistance / dodgeSpeed;
        float timePassed = 0;
        while (timePassed < timeToDodge){
            transform.position = Vector2.Lerp(startPos, targetPos, timePassed / timeToDodge);
            timePassed += Time.deltaTime;
            yield return null;
        }
        rb.AddForce(new Vector2(0f, dodgeJumpForce), ForceMode2D.Impulse);
        health.SetInvulnerableForDodge(false);
        SwitchState(BossState.Chase);
    }
    private void HealthChanged(float newHealth){
        if (newHealth <= 0){
            if (victoryTrophy != null){
                victoryTrophy.SetActive(true);
            }
            SwitchState(BossState.Dead);
            Destroy(gameObject, 3f);
            return;
        }
        if (currentState == BossState.Chase || currentState == BossState.Patrol || 
            currentState == BossState.Attack){
            SwitchState(BossState.Hurt);
        }
        float healthPercentage = newHealth / health.startingHealth;
        int previousStage = currentStage;
        if (healthPercentage <= 0.33f) currentStage = 3;
        else if (healthPercentage <= 0.66f) currentStage = 2;
        else currentStage = 1;
        if (currentStage > previousStage){
            if (currentStage == 2){
                dodgeChance = 0.33f;
                damage = baseDamage + 0.5f;
            }else if (currentStage == 3){
                dodgeChance = 0.66f;
                speed = baseSpeed * 2f;
                damage = damage * 2f;
            }
        }
    }
    private IEnumerator HurtCoroutine(){
        rb.linearVelocity = Vector2.zero;
        //animation handled in health
        yield return new WaitForSeconds(hurtDuration);
        SwitchState(BossState.Chase);
    }
    public void Attack(){
        Collider2D colliderInfo = Physics2D.OverlapCircle(attackPoint.position, attackPointRadius, playerLayer);
        if(colliderInfo)
            if (colliderInfo.transform.tag == "Player"){
                player.GetComponent<Health>().TakeDamage(damage);
            }
    }
    private void CheckForPlayer(){
        if (Vector2.Distance(transform.position, player.position) <= fieldOfView)
        {
            SwitchState(BossState.Chase);
        }
    }
    public void TurnAround(){
        if (facingLeft) transform.eulerAngles = new Vector3(0, 180, 0);
        else if (!facingLeft) transform.eulerAngles = new Vector3(0, 0, 0);
        facingLeft = !facingLeft;
    }
    private void OnDrawGizmosSelected(){
        if (!checkGround) return;
        else{
            Gizmos.color = Color.green;
            Gizmos.DrawRay(checkGround.position, Vector2.down * rayDistance);
            Gizmos.DrawRay(checkGround.position, Vector2.left * rayDistance);
            Gizmos.DrawRay(checkGround.position, Vector2.right * rayDistance);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fieldOfView);
        Gizmos.color = Color.orange;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.magenta;
        if (!attackPoint) return;
        else Gizmos.DrawWireSphere(attackPoint.position, attackPointRadius);
    }
}