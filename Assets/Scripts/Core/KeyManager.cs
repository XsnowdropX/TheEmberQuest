using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance;
    private bool hasKey = false;
    [SerializeField] GameObject keyIconUI;

    private void Awake()
{
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (keyIconUI != null)
        {
            keyIconUI.SetActive(false);
        }
    }
    public void UpdateKeyStatus(bool playerHasKey)
    {
        hasKey = playerHasKey;
        if (keyIconUI != null)
        {
            keyIconUI.SetActive(playerHasKey);
        }
    }
    public bool HasKey()
    {
        return hasKey;
    }
}
