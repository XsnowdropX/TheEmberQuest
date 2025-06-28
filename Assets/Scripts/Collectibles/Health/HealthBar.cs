using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    void Start()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    void Update()
    {
        totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
