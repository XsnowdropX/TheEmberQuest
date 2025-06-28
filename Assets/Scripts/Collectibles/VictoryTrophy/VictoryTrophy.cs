using UnityEngine;

public class VictoryTrophy : MonoBehaviour
{
    private UImanager uiManager;
    private void Awake()
    {
        uiManager = FindAnyObjectByType<UImanager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            uiManager.Victory();
        }
    }
}
