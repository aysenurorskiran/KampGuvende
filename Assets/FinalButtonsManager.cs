using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class FinalButtonsManager : MonoBehaviour
{
    public Button replayButton;
    public Button mainMenuButton;
    public Button finishButton;

    public PlayerNameManager playerNameManager;

    void Start()
    {
        if (replayButton != null)
            replayButton.onClick.AddListener(ReplayGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        if (finishButton != null)
            finishButton.onClick.AddListener(FinishGame);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void FinishGame()
    {
        string playerName = "Oyuncu";
        string timeText = "00:00";
        float timeValue = 0f;

        if (playerNameManager != null && !string.IsNullOrEmpty(playerNameManager.GetPlayerName()))
        {
            playerName = playerNameManager.GetPlayerName();
        }
        else
        {
            playerName = PlayerPrefs.GetString("PlayerName", "Oyuncu");
        }

        if (GameTimerManager.Instance != null)
        {
            GameTimerManager.Instance.StopTimer();
            timeText = GameTimerManager.Instance.GetFormattedTime();
            timeValue = GameTimerManager.Instance.GetTime();
        }

        string oldList = PlayerPrefs.GetString("ScoreList", "");
        string newRecord = playerName + "|" + timeText + "|" + timeValue;

        List<string> records = new List<string>();

        if (!string.IsNullOrEmpty(oldList))
        {
            records = oldList
                .Split('\n')
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();
        }

        records.Add(newRecord);

        records = records
            .OrderBy(x =>
            {
                string[] parts = x.Split('|');

                if (parts.Length >= 3 && float.TryParse(parts[2], out float t))
                    return t;

                return float.MaxValue;
            })
            .Take(5)
            .ToList();

        PlayerPrefs.SetString("ScoreList", string.Join("\n", records));
        PlayerPrefs.Save();

        SceneManager.LoadScene("EndScene");
    }
}