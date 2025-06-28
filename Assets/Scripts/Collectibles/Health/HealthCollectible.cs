using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip collectingSound;
    [SerializeField] private bool max;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            SoundManager.instance.PlaySoundEffect(collectingSound);
            if (max)
                collision.GetComponent<Health>().Heal(collision.GetComponent<Health>().startingHealth);
            else
                collision.GetComponent<Health>().Heal(healthValue);
            gameObject.SetActive(false);
        }
    }
}
