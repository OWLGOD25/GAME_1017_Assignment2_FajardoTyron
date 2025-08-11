using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }

    public SoundManager SoundManager { get; private set; }
    public UIManager UIManager { get; private set; }

    private GameObject pausePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Find managers as children
        SoundManager = GetComponentInChildren<SoundManager>();
        UIManager = GetComponentInChildren<UIManager>();
        
        

        if (SoundManager == null) Debug.LogWarning("SoundManager not found under MainController.");
        if (UIManager == null) Debug.LogWarning("UIManager not found under MainController.");
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find the pause panel in the new scene
        pausePanel = GameObject.FindWithTag("PausePanel");

        if (pausePanel != null)
        {
            if (scene.name == "Game")
            {
                pausePanel.SetActive(false); // start hidden
            }
            else
            {
                pausePanel.SetActive(false); // keep it off in Title/GameOver
            }
        }
    }
    public void StartGame()
    {
        // Optional: stop title music
        SoundManager?.PlayGameMusic();

        SceneManager.LoadScene("GameScene");
    }

    // Called from Main Menu button in GameOver scene
    public void GoToMainMenu()
    {
        SoundManager?.PlayTitleMusic();
        SceneManager.LoadScene("TitleScene");
    }

    public void Reset()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        // Save any necessary data here
        PlayerPrefs.Save();
        // then go back to title
        SceneManager.LoadScene("TitleScene");
        
    }
}

