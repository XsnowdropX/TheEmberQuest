using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private float delayJumpTime;
    [SerializeField] public int JumpsNumber;

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;

    private float horizontalInput;
    private float wallJumpCooldown;
    private float delayJumpTimer;
    private int jumpCounter;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0)
            transform.localScale = Vector3.one;
        else if (horizontalInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        animator.SetBool("walk", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        if (wallJumpCooldown > 0.25f)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 1;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = 2.5f;
                if(isGrounded())
                {
                    delayJumpTimer = delayJumpTime;
                    jumpCounter = JumpsNumber - 1;
                }
                else
                    delayJumpTimer -= Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                Jump();
            else if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) && body.linearVelocity.y > 0)
                body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y / 2);
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (delayJumpTimer < 0 && !onWall() && jumpCounter <= 0) return;
        SoundManager.instance.PlaySoundEffect(jumpSound);
        animator.SetTrigger("jump");
        if (onWall())
        {
            if (horizontalInput == 0)
            {
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2.5f, 1);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
             
                body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 1.5f, 5);
            }
            
            wallJumpCooldown = 0;
            return;
        }
        else if(delayJumpTimer >0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            delayJumpTimer = 0;
            return;
        }
        else if(jumpCounter>0)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            jumpCounter--;
            return;
        }
    }

    private bool isGrounded()
    {
        LayerMask groundAndWalls = groundLayer | wallLayer;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundAndWalls);
        return raycastHit.collider != null && body.linearVelocity.y <= 0.1f;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center,
            boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public void BuyJump(int jumps)
    {
        JumpsNumber += jumps;
    }

    public bool canAttack()
    {
        //return horizontalInput == 0 && isGrounded() && !onWall();
        return horizontalInput == 0 && !onWall();
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float newSpeed)
    {
        this.speed = newSpeed;
    }
}
