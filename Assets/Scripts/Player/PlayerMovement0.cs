using System;
using UnityEngine;

public class PlayerMovement0 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float delayJumpTime;
    private float delayJumpTimer;
    [SerializeField] public int JumpsNumber;
    private int jumpCounter;
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Sound")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body;
    private Animator animator;
    private bool grounded;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);

        animator.SetBool("walk", horizontalInput!=0);
        animator.SetBool("grounded", isGrounded());

        //old jump mechanic
        //if (wallJumpCooldown > 0.2f)
        //{

        //    body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

        //if (onWall() && !isGrounded())
        //{
        //    body.gravityScale = 0;
        //    body.linearVelocity = Vector2.zero;
        //}
        //else
        //    body.gravityScale = 2.5f;

        //    if (Input.GetKey(KeyCode.Space))
        //        Jump();
        //}
        //else
        //    wallJumpCooldown += Time.deltaTime;

        //new jump mechanic
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            Jump();

        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) && body.linearVelocity.y > 0)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y / 2);

        if(onWall())
        {
            body.gravityScale = 1;
            body.linearVelocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 2.5f;
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

            if (isGrounded())
            {
                delayJumpTimer = delayJumpTime;
                jumpCounter = JumpsNumber-1;
            }
            else
                delayJumpTimer -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (delayJumpTimer < 0 && !onWall() &&jumpCounter <=0) return;
        SoundManager.instance.PlaySoundEffect(jumpSound);
        animator.SetTrigger("jump");

        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            else
            {
                if(delayJumpTimer > 0)
                    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                else
                {
                    if(jumpCounter >0)
                    {
                        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }
            delayJumpTimer = 0;
        }

        //if (isGrounded())
        //{
        //    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
        //    //animator.SetTrigger("jump");
        //}
        //else if(onWall() && !isGrounded())
        //{
        //    if(horizontalInput == 0)
        //    {
        //        body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 10); //mess around
        //        transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        //    }
        //    else
        //        body.linearVelocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

        //    wallJumpCooldown = 0;
        //}
        //grounded = false;
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-MathF.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }

    //readded this method
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }

    public void BuyJump(int jumps)
    {
        JumpsNumber+=jumps;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
        return raycastHit.collider != null;
    }
    
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
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