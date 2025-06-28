using System;
using System.Threading.Tasks;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask PlayerLayer;

    private float checkTimer;
    private bool attacking;
    private Vector3 destination;
    private Vector3 direction = new Vector3();
    private FallingRock script;
    private BoxCollider2D boxColl;
    private Rigidbody2D body;

    private void Awake()
    {
        script = GetComponent<FallingRock>();
        boxColl = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();

        script.enabled = true;
    }

    private void Update()
    {
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckPlayer();
        }
    }

    private void CheckPlayer()
    {
        direction = -transform.up * range;

        Debug.DrawRay(transform.position, direction, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range, PlayerLayer);

        if (hit.collider != null && !attacking)
        {
            attacking = true;
            destination = direction;
            checkTimer = 0;
        }
    }

    private async void Stop(int seconds)
    {
        attacking = false;
        destination = transform.position;
        gameObject.layer = 7;
        body.gravityScale = 10;
        boxColl.isTrigger = false;
        script.enabled = false;
        await Task.Delay(TimeSpan.FromSeconds(seconds));
        body.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && attacking)
        {
            collision.GetComponent<Health>().TakeDamage(collision.GetComponent<Health>().startingHealth);
            Stop(2);
        }
        if(collision.tag == "Ground")
        {
            Stop(0);
        }

    }
}