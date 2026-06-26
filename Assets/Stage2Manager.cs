using UnityEngine;
using TMPro;
using System.Collections;

public class Stage2Manager : MonoBehaviour
{
    public GameObject questionPanel;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI infoText;

    public TextMeshProUGUI counterText;
    public RectTransform counterPanel;

    public RectTransform stage2BadgePanel;
    public Stage3Manager stage3Manager;

    public int totalObjects = 5;

    private int foundCount = 0;
    private bool correctAnswer;
    private GameObject currentObject;
    private string currentObjectName;

    private bool stage2Started = false;
    private bool questionAnswered = false;
    private bool stage2Completed = false;

    private string[] motivationMessages =
    {
        "Harikasın! İlk riski fark ettin.",
        "Süper dedektif! Kamp alanı daha güvenli oluyor.",
        "Bravo! Dikkatli bakmaya devam et.",
        "Muhteşem! Son nesneye çok yaklaştın."
    };

    void Start()
    {
        totalObjects = 5;

        if (questionPanel != null) questionPanel.SetActive(false);
        if (infoText != null) infoText.text = "";
        if (counterPanel != null) counterPanel.gameObject.SetActive(false);
        if (stage2BadgePanel != null) stage2BadgePanel.gameObject.SetActive(false);

        UpdateCounter();
    }

    public bool IsStage2Active()
    {
        return stage2Started;
    }

    public void StartStage2()
    {
        totalObjects = 5;
        foundCount = 0;
        stage2Started = true;
        stage2Completed = false;
        questionAnswered = false;

        UpdateCounter();

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Görev 2’ye hazır mısın?\nEtraftaki nesneleri birlikte inceleyelim."
            );
        }

        if (counterPanel != null)
        {
            counterPanel.gameObject.SetActive(true);
            StartCoroutine(SlideCounterPanel());
        }
    }

    public void OpenQuestion(bool isRisky, string objectName, GameObject objectToHide)
    {
        if (!stage2Started || stage2Completed || questionAnswered)
            return;

        correctAnswer = isRisky;
        currentObject = objectToHide;
        currentObjectName = objectName;
        questionAnswered = false;

        if (questionPanel != null)
            questionPanel.SetActive(true);

        FindFirstObjectByType<PlayerMovement>().StopPlayer();

        if (questionText != null)
            questionText.text = objectName + "\n\nBu nesne doğada yangın riski oluşturabilir mi?";

        if (infoText != null)
            infoText.text = "";
    }

    public void AnswerYes()
    {
        CheckAnswer(true);
    }

    public void AnswerNo()
    {
        CheckAnswer(false);
    }

    void CheckAnswer(bool answer)
    {
        if (questionAnswered || stage2Completed)
            return;

        if (infoText != null)
            infoText.text = "";

        if (answer == correctAnswer)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySuccess();

            questionAnswered = true;
            foundCount++;
            UpdateCounter();

            if (KopukGuide.Instance != null)
            {
                KopukGuide.Instance.ShowMessage(
                    "Doğru seçim!\nBu nesne kamp alanında bırakılmamalı."
                );
            }

            if (currentObject != null)
                currentObject.SetActive(false);

            StartCoroutine(CloseQuestionAfterSpace());
        }
        else
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayFail();

            if (KopukGuide.Instance != null)
                KopukGuide.Instance.ShowMessage(GetWrongAnswerMessage(currentObjectName));
        }
    }

    string GetWrongAnswerMessage(string objectName)
    {
        if (string.IsNullOrEmpty(objectName))
            return "Tekrar düşün.\nBu nesneyi doğada bırakmak güvenli olmayabilir.";

        if (objectName.Contains("Cam") || objectName.Contains("Glass"))
            return "Cam parçası güneş ışığını büyüteç gibi toplayabilir.\nKuru otların tutuşmasına neden olabilir.";

        if (objectName.Contains("Kutu") || objectName.Contains("Soda") || objectName.Contains("Can"))
            return "Metal kutular güneşte çok ısınabilir.\nKuru otların arasında kalırsa yangın riskini artırabilir.";

        if (objectName.Contains("Kupa") || objectName.Contains("Mug"))
            return "Kırık kupa doğrudan yangın çıkarmaz.\nAma kamp alanını güvensiz yapar ve toplanması gerekir.";

        if (objectName.Contains("Elma") || objectName.Contains("Apple"))
            return "Elma çöpü doğrudan yangın riski oluşturmaz.\nDoğayı temiz tutmak için çöpe atmalıyız.";

        if (objectName.Contains("Kağıt") || objectName.Contains("Paper"))
            return "Kağıt çok kolay tutuşur.\nKamp ateşine yakın kalırsa küçük bir kıvılcımla yanabilir.";

        return "Tekrar düşün.\nBu nesne doğada bırakılırsa kamp alanını güvensiz hale getirebilir.";
    }

    void UpdateCounter()
    {
        if (counterText != null)
            counterText.text = "Bulunan: " + foundCount + "/" + totalObjects;
    }

    IEnumerator SlideCounterPanel()
    {
        Vector2 startPos = new Vector2(900, -85);
        Vector2 endPos = new Vector2(-220, -85);

        counterPanel.anchoredPosition = startPos;

        float duration = 0.8f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            t = 1f - Mathf.Pow(1f - t, 3f);

            counterPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        counterPanel.anchoredPosition = endPos;
    }

    IEnumerator CloseQuestionAfterSpace()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (questionPanel != null)
            questionPanel.SetActive(false);

        FindFirstObjectByType<PlayerMovement>().StartPlayer();

        if (foundCount >= totalObjects)
        {
            stage2Completed = true;
            StartCoroutine(ShowStage2Complete());
        }
        else
        {
            questionAnswered = false;

            int messageIndex = Mathf.Clamp(foundCount - 1, 0, motivationMessages.Length - 1);

            if (KopukGuide.Instance != null)
                KopukGuide.Instance.ShowMessage(motivationMessages[messageIndex]);
        }
    }

    IEnumerator ShowStage2Complete()
    {
        if (counterPanel != null)
            counterPanel.gameObject.SetActive(false);

        if (questionPanel != null)
            questionPanel.SetActive(false);

        if (KopukGuide.Instance != null)
            KopukGuide.Instance.ShowMessage("Harika! Kamp alanındaki riskleri başarıyla buldun.");

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySuccess();

        if (stage2BadgePanel != null)
        {
            TextMeshProUGUI badgeText = stage2BadgePanel.GetComponentInChildren<TextMeshProUGUI>();

            if (badgeText != null)
                badgeText.text = "Doğa Dedektifi Rozeti!";

            stage2BadgePanel.gameObject.SetActive(true);

            Vector2 startPos = new Vector2(900, stage2BadgePanel.anchoredPosition.y);
            Vector2 endPos = new Vector2(0, stage2BadgePanel.anchoredPosition.y);

            stage2BadgePanel.anchoredPosition = startPos;

            float duration = 0.8f;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = 1f - Mathf.Pow(1f - Mathf.Clamp01(time / duration), 3f);

                stage2BadgePanel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }

            stage2BadgePanel.anchoredPosition = endPos;
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (stage2BadgePanel != null)
            stage2BadgePanel.gameObject.SetActive(false);

        if (KopukGuide.Instance != null)
            KopukGuide.Instance.ShowMessage("Bir dakika...\nBurnuma farklı bir koku geliyor.");

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Space));
        yield return new WaitForSeconds(1f);

        if (stage3Manager != null)
        {
            stage3Manager.StartStage3();
        }
        else
        {
            Debug.LogWarning("Stage3Manager bağlı değil. Stage 3 başlatılamadı.");
        }
    }
}