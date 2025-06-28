using UnityEngine;

public class Lock : MonoBehaviour
{
    public AudioClip unlockSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (KeyManager.instance.HasKey())
            {
                KeyManager.instance.UpdateKeyStatus(false);
                if (unlockSound != null)
                {
                    AudioSource.PlayClipAtPoint(unlockSound, transform.position);
                }
                gameObject.SetActive(false);
                Destroy(gameObject, 1f);
            }
        }
    }
}
