using UnityEngine;

public class ProjectileBuy : MonoBehaviour
{
    [SerializeField] private float upgradePrice;
    [SerializeField] private float upgradeRewardSpeed;
    [SerializeField] private float upgradeRewardDamage;
    private PlayerAttack playerAttack;
    private bool playerInRange = false;
    private CoinManager playerCoins;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        playerInRange = true;
        playerCoins = collision.GetComponent<CoinManager>();
        playerAttack = collision.GetComponent<PlayerAttack>();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange = false;
        playerCoins = null;
        playerAttack = null;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerAttack != null &&  
                playerCoins.coinNumber >= upgradePrice)
            {
                playerCoins.coinNumber -= upgradePrice;
                playerAttack.UpgradeFireball(upgradeRewardSpeed, upgradeRewardDamage);
            }
            else
            {
            }
        }
    }
}
