using UnityEngine;
using TMPro;

public class PhonePanelManager : MonoBehaviour
{
    public GameObject phonePanel2;
    public TextMeshProUGUI numberText;
    public Stage3Manager stage3Manager;

    private string enteredNumber = "";

    void Start()
    {
        if (phonePanel2 != null)
            phonePanel2.SetActive(false);

        UpdateNumberText();
    }

    public void OpenPhone()
    {
        enteredNumber = "";
        UpdateNumberText();

        if (phonePanel2 != null)
            phonePanel2.SetActive(true);
    }

    public void PressNumber(string number)
    {
        if (enteredNumber.Length >= 3)
            return;

        enteredNumber += number;
        UpdateNumberText();
    }

    public void ClearNumber()
    {
        enteredNumber = "";
        UpdateNumberText();
    }

    public void CallNumber()
    {
        if (enteredNumber == "112")
        {
            if (phonePanel2 != null)
                phonePanel2.SetActive(false);

            stage3Manager.Correct112Entered();
        }
        else
        {
            stage3Manager.Wrong112Entered();
            ClearNumber();
        }
    }

    private void UpdateNumberText()
    {
        if (numberText == null)
            return;

        if (enteredNumber.Length == 0)
            numberText.text = "_ _ _";
        else if (enteredNumber.Length == 1)
            numberText.text = enteredNumber + " _ _";
        else if (enteredNumber.Length == 2)
            numberText.text = enteredNumber[0] + " " + enteredNumber[1] + " _";
        else
            numberText.text = enteredNumber[0] + " " + enteredNumber[1] + " " + enteredNumber[2];
    }
}