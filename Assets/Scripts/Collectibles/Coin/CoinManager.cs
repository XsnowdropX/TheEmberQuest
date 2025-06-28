using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public float coinNumber = 0;
    [SerializeField] private float coinValue;
    [SerializeField] private Text coinCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            coinNumber += coinValue;
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject, 1f);
        }
    }

    private void Update()
    {
        coinCount.text = coinNumber.ToString();
    }
}
