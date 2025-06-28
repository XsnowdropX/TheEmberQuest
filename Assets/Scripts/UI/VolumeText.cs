using UnityEngine;
using UnityEngine.UI;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string sourceName;
    [SerializeField] private string displaySourceName;
    private Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        text.text = displaySourceName + (PlayerPrefs.GetFloat(sourceName)*100).ToString() + "%";
    }
}
