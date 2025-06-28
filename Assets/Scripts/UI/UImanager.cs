using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private AudioClip pauseSound;
    [SerializeField] private AudioClip unpauseSound;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private AudioClip victorySound;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private void Awake(){
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        victoryScreen.SetActive(false);
        Time.timeScale = 1;
        playerMovement = FindAnyObjectByType<PlayerMovement>().GetComponent<PlayerMovement>();
        playerAttack = FindAnyObjectByType<PlayerAttack>().GetComponent<PlayerAttack>();
    }
    public void GameOver(){
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySoundEffect(gameOverSound);
    }
    public void Victory(){
        Time.timeScale = 0;
        victoryScreen.SetActive(true);
        SoundManager.instance.PlaySoundEffect(victorySound);
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void GoToMainMenu(){
        SceneManager.LoadScene(0);
    }
    
    public void Quit(){
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeInHierarchy)
            {
                SoundManager.instance.PlaySoundEffect(unpauseSound);
                PauseGame(false);
            }
            else
            {
                SoundManager.instance.PlaySoundEffect(pauseSound);
                PauseGame(true);
            }
        }
    }
    public void PauseGame(bool status){
        pauseScreen.SetActive(status);
        if (status){
            playerMovement.enabled = false;
            playerAttack.enabled = false;
            Time.timeScale = 0.25f;
        }
        else{   
            playerMovement.enabled = true;
            playerAttack.enabled = true;
            Time.timeScale = 1;
        }
    }
    public void changeSoundVolume(){
        SoundManager.instance.changeSoundVolume(0.2f);
    }
    public void changeMusicVolume(){
        SoundManager.instance.changeMusicVolume(0.2f);
    }
}
