using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void changeSoundVolume()
    {
        SoundManager.instance.changeSoundVolume(0.2f);
    }

    public void changeMusicVolume()
    {
        SoundManager.instance.changeMusicVolume(0.2f);
    }

    public void Quit()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
