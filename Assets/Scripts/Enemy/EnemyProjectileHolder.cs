using UnityEngine;

public class EnemyProjectileHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()
    {
        if(enemy != null)
            transform.localScale = enemy.localScale;
    }
}
