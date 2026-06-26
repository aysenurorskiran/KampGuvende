using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public Button howToPlayButton;
    public Button backButton;

    void Start()
    {
       

        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);

        if (howToPlayButton != null)
            howToPlayButton.onClick.AddListener(OpenHowToPlay);

        if (backButton != null)
            backButton.onClick.AddListener(CloseHowToPlay);
    }

    public void StartGame()
    {
        SoundManager.Instance.PlayClick();
        SceneManager.LoadScene("MainScene");
    }

    public void ExitGame()
    {
        SoundManager.Instance.PlayClick();
        Application.Quit();
    }

    public void OpenHowToPlay()
    {
        SoundManager.Instance.PlayClick();
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        SoundManager.Instance.PlayClick();
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);
    }
}