using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource sound;
    private AudioSource music;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        music = transform.GetChild(0).GetComponent<AudioSource>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
            Destroy(gameObject);

        changeMusicVolume(0);
        changeSoundVolume(0);
    }

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        sound.PlayOneShot(soundEffect);
    }

    public void changeSoundVolume(float volume)
    {
        float baseVolume = 0.6f;
        float currentVolume = PlayerPrefs.GetFloat("soundVolume", 1);
        currentVolume += volume;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        float finalVolume = currentVolume * baseVolume;
        sound.volume = finalVolume;
        PlayerPrefs.SetFloat("soundVolume", currentVolume);
    }
    
    public void changeMusicVolume(float volume)
    {
        float baseVolume = 0.2f;
        float currentVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        currentVolume += volume;

        if (currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;

        float finalVolume = currentVolume * baseVolume;
        music.volume = finalVolume;
        PlayerPrefs.SetFloat("musicVolume", currentVolume);
    }
}
