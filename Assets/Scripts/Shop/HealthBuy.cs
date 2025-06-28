using UnityEngine;

public class HealthBuy : MonoBehaviour
{
    [SerializeField] private float upgradePrice;
    [SerializeField] private float upgradeReward;
    private bool playerInRange = false;
    private Health playerHealth;
    private CoinManager playerCoins;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerInRange = true;
            playerHealth = collision.GetComponent<Health>();
            playerCoins = collision.GetComponent<CoinManager>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange = false;
        playerHealth = null;
        playerCoins = null;
    }

    private void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if(playerHealth != null && playerHealth.startingHealth<=10 && 
                playerCoins.coinNumber >=upgradePrice)
            {
                playerCoins.coinNumber -= upgradePrice;
                playerHealth.BuyHealth(upgradeReward);
            }
            else
            {
            }
        }
    }
}
