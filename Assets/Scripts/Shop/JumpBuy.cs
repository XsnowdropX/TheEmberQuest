using UnityEngine;

public class JumpBuy : MonoBehaviour
{
    [SerializeField] private float upgradePrice;
    [SerializeField] private float upgradeReward;
    private bool playerInRange = false;
    private PlayerMovement playerMovement;
    private CoinManager playerCoins;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        playerInRange = true;
        playerCoins = collision.GetComponent<CoinManager>();
        playerMovement = collision.GetComponent<PlayerMovement>();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange = false;
        playerCoins = null;
        playerMovement = null;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerMovement != null && playerMovement.JumpsNumber <= 3 && 
                playerCoins.coinNumber >= upgradePrice)
            {
                playerCoins.coinNumber -= upgradePrice;
                playerMovement.BuyJump((int)upgradeReward);
            }
        }
    }
}
