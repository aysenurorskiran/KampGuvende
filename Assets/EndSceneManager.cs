using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EndSceneManager : MonoBehaviour
{
    public TextMeshProUGUI bestPlayerText;
    public TextMeshProUGUI resultListText;

    public TextMeshProUGUI siraText;
    public TextMeshProUGUI kahramanText;
    public TextMeshProUGUI sureText;

    public Button mainMenuButton;

    private Vector3 bestTextStartScale;

    void Start()
    {
        if (bestPlayerText != null)
            bestTextStartScale = bestPlayerText.transform.localScale;

        ShowResults();

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    void Update()
    {
        if (bestPlayerText != null && bestPlayerText.gameObject.activeSelf)
        {
            float scale = 1f + Mathf.Sin(Time.time * 3f) * 0.08f;
            bestPlayerText.transform.localScale = bestTextStartScale * scale;
        }
    }

    void ShowResults()
    {
        string scoreList = PlayerPrefs.GetString("ScoreList", "");

        if (bestPlayerText != null)
        {
            bestPlayerText.alignment = TextAlignmentOptions.Center;
            bestPlayerText.color = new Color32(92, 54, 28, 255);
            bestPlayerText.richText = false;
        }

        SetupTableText(siraText);
        SetupTableText(kahramanText);
        SetupTableText(sureText);

        if (resultListText != null)
            resultListText.gameObject.SetActive(false);

        if (string.IsNullOrEmpty(scoreList))
        {
            if (bestPlayerText != null)
                bestPlayerText.text = "HENÜZ KAYIT YOK";

            if (siraText != null) siraText.text = "";
            if (kahramanText != null) kahramanText.text = "Oyunu bitiren kahramanlar burada listelenecek.";
            if (sureText != null) sureText.text = "";

            return;
        }

        string[] records = scoreList.Split('\n');
        List<ScoreRecord> scoreRecords = new List<ScoreRecord>();

        foreach (string record in records)
        {
            string[] parts = record.Split('|');

            if (parts.Length >= 3)
            {
                string playerName = parts[0];
                string timeText = parts[1];

                float timeValue = 99999f;
                float.TryParse(parts[2], out timeValue);

                scoreRecords.Add(new ScoreRecord(playerName, timeText, timeValue));
            }
        }

        scoreRecords = scoreRecords
            .OrderBy(x => x.timeValue)
            .Take(5)
            .ToList();

        if (scoreRecords.Count == 0)
        {
            if (bestPlayerText != null)
                bestPlayerText.text = "HENÜZ KAYIT YOK";

            if (siraText != null) siraText.text = "";
            if (kahramanText != null) kahramanText.text = "Oyunu bitiren kahramanlar burada listelenecek.";
            if (sureText != null) sureText.text = "";

            return;
        }

        ScoreRecord best = scoreRecords[0];

        if (bestPlayerText != null)
        {
            bestPlayerText.text =
                "EN HIZLI OYUNCU\n" +
                best.playerName + "\n" +
                best.timeText;
        }

        StringBuilder siraBuilder = new StringBuilder();
        StringBuilder kahramanBuilder = new StringBuilder();
        StringBuilder sureBuilder = new StringBuilder();

        siraBuilder.AppendLine("SIRA");
        kahramanBuilder.AppendLine("KAHRAMAN");
        sureBuilder.AppendLine("SÜRE");

        for (int i = 0; i < scoreRecords.Count; i++)
        {
            siraBuilder.AppendLine((i + 1).ToString());
            kahramanBuilder.AppendLine(scoreRecords[i].playerName);
            sureBuilder.AppendLine(scoreRecords[i].timeText);
        }

        if (siraText != null)
            siraText.text = siraBuilder.ToString();

        if (kahramanText != null)
            kahramanText.text = kahramanBuilder.ToString();

        if (sureText != null)
            sureText.text = sureBuilder.ToString();
    }

    void SetupTableText(TextMeshProUGUI text)
    {
        if (text == null)
            return;

        text.alignment = TextAlignmentOptions.Center;
        text.color = new Color32(58, 36, 20, 255);
        text.richText = false;
        text.enableWordWrapping = false;
        text.fontSize = 21;
    }

    public void GoToMainMenu()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClick();

        SceneManager.LoadScene("MainMenu");
    }

    class ScoreRecord
    {
        public string playerName;
        public string timeText;
        public float timeValue;

        public ScoreRecord(string playerName, string timeText, float timeValue)
        {
            this.playerName = playerName;
            this.timeText = timeText;
            this.timeValue = timeValue;
        }
    }
}