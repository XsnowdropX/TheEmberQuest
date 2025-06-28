using System;
using System.Threading.Tasks;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    private float speed;
    private async Task OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speed = collision.GetComponent<PlayerMovement>().GetSpeed();
            gameObject.SetActive(false);
            speed += 5f;
            collision.GetComponent<PlayerMovement>().SetSpeed(speed);
            await Task.Delay(TimeSpan.FromSeconds(3));
            speed -= 5f;
            collision.GetComponent<PlayerMovement>().SetSpeed(speed);
            Destroy(gameObject);
        }
    }
}
