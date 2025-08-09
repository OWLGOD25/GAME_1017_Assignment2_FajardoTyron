using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public static MainController Instance { get; private set; }

    public SoundManager SoundManager { get; private set; }
    public UIManager UIManager { get; private set; }

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

        SceneManager.LoadScene("Game");
    }

    // Called from Main Menu button in GameOver scene
    public void GoToMainMenu()
    {
        SoundManager?.PlayTitleMusic();
        SceneManager.LoadScene("Title");
    }

    public void Reset()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        // Save any necessary data here
        PlayerPrefs.Save();
        // then go back to title
        SceneManager.LoadScene("Title");
        
    }
}

