using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;


public class UIManager : MonoBehaviour
{
    [Header("UI Button")]
    public Button startButton; // persistent button to start game
    public Button resetButton; // persistent button to reset game
    public Button quitButton; // persistent button to quit game

    [Header("Pause / Options")]
    public GameObject pausePanel; // translucent panel (child of canvas)
    public Button pauseToggleButton; // persistent button
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("HUD")]
    public Text timerText;
    public Text bestTimeText;
    public Text livesText;

    private bool isPaused = false;

    private void Start()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (pauseToggleButton) pauseToggleButton.onClick.AddListener(TogglePause);

        // initialize sliders from SoundManager
        if (MainController.Instance && MainController.Instance.SoundManager)
        {
            var sm = MainController.Instance.SoundManager;
            if (musicSlider) musicSlider.value = PlayerPrefs.HasKey("MusicVol") ? PlayerPrefs.GetFloat("MusicVol") : 1f;
            if (sfxSlider) sfxSlider.value = PlayerPrefs.HasKey("SfxVol") ? PlayerPrefs.GetFloat("SfxVol") : 1f;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel) pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel) pausePanel.SetActive(false);
        }
    }

    // Hook these to sliders OnValueChanged
    public void SetMusicSlider(float v)
    {
        MainController.Instance.SoundManager.SetMusicVolume(v);
        PlayerPrefs.SetFloat("MusicVol", v);
    }

    public void SetSfxSlider(float v)
    {
        MainController.Instance.SoundManager.SetSfxVolume(v);
        PlayerPrefs.SetFloat("SfxVol", v);
    }

    // HUD updates
    public void UpdateTimer(float seconds)
    {
        if (timerText) timerText.text = "Time: " + seconds.ToString("F2");
    }

    public void UpdateBestTime(float seconds)
    {
        if (bestTimeText) bestTimeText.text = "Best: " + seconds.ToString("F2");
    }

    public void UpdateLives(int lives)
    {
        if (livesText) livesText.text = "Lives: " + lives;
    }

    public void ShowGameOver(float bestTime)
    {
        // go to Game Over scene after a short delay (pause handled by GameManager)
        SceneManager.LoadScene("GameOver");
    }
}
