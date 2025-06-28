using UnityEngine;

public class Key : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!KeyManager.instance.HasKey())
            {
                KeyManager.instance.UpdateKeyStatus(true);

                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                gameObject.SetActive(false);
                Destroy(gameObject, 1f);
            }
        }
    }
}
