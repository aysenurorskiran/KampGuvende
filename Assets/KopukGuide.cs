using UnityEngine;
using TMPro;

public class KopukGuide : MonoBehaviour
{
    public static KopukGuide Instance;

    public GameObject speechPanel;
    public TextMeshProUGUI speechText;

    void Awake()
    {
        Instance = this;
    }

    public void ShowMessage(string message)
    {
        if (speechPanel != null)
            speechPanel.SetActive(true);

        if (speechText != null)
            speechText.text = message;
    }

    public void HideMessage()
    {
        if (speechPanel != null)
            speechPanel.SetActive(false);
    }
}