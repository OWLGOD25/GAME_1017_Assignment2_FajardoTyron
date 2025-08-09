using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public Text lastRunText;
    public Text bestTimeText;
    public Button mainMenuButton;

    private void Start()
    {
        float lastRun = PlayerPrefs.GetFloat("LastRunTime", 0f);
        float best = PlayerPrefs.GetFloat("BestTime", 0f);

        if (lastRunText) lastRunText.text = "Last Run: " + lastRun.ToString("F2") + "s";
        if (bestTimeText) bestTimeText.text = "Best Time: " + best.ToString("F2") + "s";

        if (mainMenuButton) mainMenuButton.onClick.AddListener(() => SceneManager.LoadScene("Title"));
    }
}
