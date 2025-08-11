using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }

    public SoundManager SoundManager { get; private set; }
    public UIManager UIManager { get; private set; }

    [Header("Pause Panel")]
    public GameObject pausePanel;

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

