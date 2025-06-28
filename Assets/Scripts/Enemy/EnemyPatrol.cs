using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrolling")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy Type")]
    [SerializeField] private Transform enemy;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float idleDuration;
    private float idleTimer;
    private Vector3 initialScale;
    private bool goingLeft;

    [Header("Enemy's Animator")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        initialScale = enemy.localScale;
    }

    private void Update()
    {
        if(goingLeft)
        {
            if(enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
            {
                ChangeDirection();
            }
        }
        else
        {
            if (enemy && enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
            {
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    {
        animator.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if(idleTimer>idleDuration)
            goingLeft = !goingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        animator.SetBool("moving", true);

        enemy.localScale = new Vector3(Mathf.Abs(initialScale.x) * _direction, 
            initialScale.y, initialScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed, 
            enemy.position.y, enemy.position.z);
    }
    
    private void OnDisable()
    {
        if(animator)
            animator.SetBool("moving", false);
    }
}
