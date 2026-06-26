using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerNameManager : MonoBehaviour
{
    public GameObject namePanel;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI kopukText;
    public MonoBehaviour playerMovement;

    private string playerName;

    void Start()
    {
        namePanel.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (nameInputField != null)
            nameInputField.ActivateInputField();
    }

    void Update()
    {
        if (namePanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmName();
        }
    }

    public void ConfirmName()
    {
        playerName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(playerName))
            return;

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        StartCoroutine(StartGameFlow());
    }

    IEnumerator StartGameFlow()
    {
        namePanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        kopukText.text =
            "Merhaba " + playerName +
            "!\nKamp güvenliðini birlikte öðreneceðiz.";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        kopukText.text =
            "Hazýrsan ilk görevimize baþlayalým!";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        kopukText.text =
            "Þu kamp ateþinin yanýnda hâlâ sýcak közler var.";

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        kopukText.text =
            "Hadi ateþe yaklaþ ve güvenli olup olmadýðýný kontrol et.";

        if (GameTimerManager.Instance != null)
            GameTimerManager.Instance.StartTimer();
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}