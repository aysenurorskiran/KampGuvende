using UnityEngine;
using TMPro;

public class GameTimerManager : MonoBehaviour
{
    public static GameTimerManager Instance;

    public TextMeshProUGUI timerText;

    private float activeTime = 0f;
    private bool timerRunning = false;
    private bool timerFinished = false;


    public float GetTime()
    {
        return activeTime;
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateTimerText();
    }

    void Update()
    {
        if (timerRunning && !timerFinished)
        {
            activeTime += Time.deltaTime;
            UpdateTimerText();
        }
    }

    public void StartTimer()
    {
        if (timerFinished) return;
        timerRunning = true;
    }

    public void PauseTimer()
    {
        if (timerFinished) return;
        timerRunning = false;
    }

    public void StopTimer()
    {
        timerRunning = false;
        timerFinished = true;
        UpdateTimerText();
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(activeTime / 60f);
        int seconds = Mathf.FloorToInt(activeTime % 60f);

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = GetFormattedTime();
    }
}