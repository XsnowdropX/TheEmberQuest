using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UImanager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindAnyObjectByType<UImanager>(); //or FindFirst..
    }

    public void Respawn()
    {
        if(currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        transform.position = currentCheckpoint.position;
        playerHealth.PlayerRespawn();
        //Camera.main.GetComponent<CameraController>()
        //    .MoveToNewRoom(currentCheckpoint.parent);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform;
            SoundManager.instance.PlaySoundEffect(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("activate");
        }
    }
}
