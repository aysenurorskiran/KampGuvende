using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage3Manager : MonoBehaviour
{
    [Header("Stage 3 UI")]
    public GameObject lookAroundPanel;
    public RectTransform stage3StartPanel;
    public GameObject stage3InteractionPanel;
    public GameObject phonePanel;
    public GameObject phonePanel2;
    public GameObject finalPanel;
    public GameObject finalKopukPanel;

    [Header("Telefon")]
    public PhonePanelManager phonePanelManager;

    [Header("Sonuc Panelleri")]
    public RectTransform correctPanel;
    public RectTransform badgePanel;

    [Header("Stage 1-2 UI Kapatilacaklar")]
    public GameObject mainTaskPanel;
    public GameObject counterPanel;
    public GameObject questionPanel;
    public GameObject rewardPanel;

    [Header("Butonlar")]
    public Button call112Button;
    public Button lookCloserButton;
    public Button handleMyselfButton;

    [Header("Konum Ayarlari")]
    public Vector2 topVisiblePos = new Vector2(0, 120);
    public Vector2 leftHiddenPos = new Vector2(-900, 220);
    public float slideDuration = 0.6f;

    private bool stage3Active = false;
    private bool playerNearSmoke = false;
    private bool phoneOpened = false;
    private bool wrongAnswerShowing = false;

    void Start()
    {
        CloseAllStage3Panels();

        if (stage3StartPanel != null)
            stage3StartPanel.gameObject.SetActive(false);

        if (correctPanel != null)
        {
            correctPanel.gameObject.SetActive(false);
            correctPanel.anchoredPosition = topVisiblePos;
        }

        if (badgePanel != null)
        {
            badgePanel.gameObject.SetActive(false);
            badgePanel.anchoredPosition = leftHiddenPos;
        }

        if (finalKopukPanel != null)
            finalKopukPanel.SetActive(false);

        if (call112Button != null)
            call112Button.onClick.AddListener(Open112PhoneTask);

        if (lookCloserButton != null)
            lookCloserButton.onClick.AddListener(WrongAnswerLookCloser);

        if (handleMyselfButton != null)
            handleMyselfButton.onClick.AddListener(WrongAnswerHandleMyself);
    }

    void Update()
    {
        if (stage3Active && playerNearSmoke && !phoneOpened && Input.GetKeyDown(KeyCode.E))
        {
            OpenPhonePanel();
        }
    }

    public void StartStage3()
    {
        StartCoroutine(Stage3StartRoutine());
    }

    IEnumerator Stage3StartRoutine()
    {
        if (mainTaskPanel != null) mainTaskPanel.SetActive(false);
        if (counterPanel != null) counterPanel.SetActive(false);
        if (questionPanel != null) questionPanel.SetActive(false);
        if (rewardPanel != null) rewardPanel.SetActive(false);

        if (lookAroundPanel != null)
            lookAroundPanel.SetActive(false);

        if (stage3StartPanel != null)
            stage3StartPanel.gameObject.SetActive(false);

        if (phonePanel != null)
            phonePanel.SetActive(false);

        if (phonePanel2 != null)
            phonePanel2.SetActive(false);

        if (correctPanel != null)
            correctPanel.gameObject.SetActive(false);

        if (badgePanel != null)
            badgePanel.gameObject.SetActive(false);

        if (finalKopukPanel != null)
            finalKopukPanel.SetActive(false);

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Etrafýna dikkatlice bakýp dumaný fark edebilir misin?"
            );
        }

        stage3Active = true;

        yield break;
    }

    public void PlayerEnteredSmokeArea()
    {
        if (!stage3Active || phoneOpened) return;

        playerNearSmoke = true;

        if (stage3InteractionPanel != null)
            stage3InteractionPanel.SetActive(true);
    }

    public void PlayerExitedSmokeArea()
    {
        playerNearSmoke = false;

        if (stage3InteractionPanel != null)
            stage3InteractionPanel.SetActive(false);
    }

    void OpenPhonePanel()
    {
        phoneOpened = true;

        if (stage3InteractionPanel != null)
            stage3InteractionPanel.SetActive(false);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClick();

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Ýþte burada!\nPeki böyle bir durumda en güvenli davranýþ hangisi olur?"
            );
        }

        if (phonePanel != null)
            phonePanel.SetActive(true);
    }

    public void Open112PhoneTask()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayClick();

        if (phonePanel != null)
            phonePanel.SetActive(false);

        if (stage3InteractionPanel != null)
            stage3InteractionPanel.SetActive(false);

        StartCoroutine(Open112PhoneRoutine());
    }

    IEnumerator Open112PhoneRoutine()
    {
        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Harika seçim!\nBöyle durumlarda yardým istemek çok önemlidir."
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "112 Acil Çaðrý Merkezi, acil ekiplerin yardým göndermesini saðlar."
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Þimdi telefonu kullanarak 112'yi tuþlayalým."
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (phonePanelManager != null)
            phonePanelManager.OpenPhone();
    }

    public void Wrong112Entered()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayFail();

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Tekrar deneyelim.\nAcil yardým için aramamýz gereken numara 112."
            );
        }
    }

    public void Correct112Entered()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySuccess();

        StartCoroutine(CorrectRoutine());
    }

    IEnumerator CorrectRoutine()
    {
        if (stage3InteractionPanel != null)
            stage3InteractionPanel.SetActive(false);

        if (phonePanel != null)
            phonePanel.SetActive(false);

        if (phonePanel2 != null)
            phonePanel2.SetActive(false);

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Aferin!\nDoðru numarayý tuþladýn."
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Duman veya yangýn gördüðümüzde güvenli bir yerden 112'yi aramak doðru davranýþtýr."
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Yardým isterken nerede olduðunu ve ne gördüðünü sakin bir þekilde anlatmayý da unutmamalýyýz."
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (correctPanel != null)
            correctPanel.gameObject.SetActive(true);

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (correctPanel != null)
            correctPanel.gameObject.SetActive(false);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySuccess();

        if (badgePanel != null)
        {
            TextMeshProUGUI badgeText = badgePanel.GetComponentInChildren<TextMeshProUGUI>();

            if (badgeText != null)
                badgeText.text = "Acil Yardýmcý Rozeti!";

            badgePanel.gameObject.SetActive(true);

            yield return StartCoroutine(
                SlidePanel(
                    badgePanel,
                    new Vector2(-900, 120),
                    new Vector2(0, 120)
                )
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (badgePanel != null)
            badgePanel.gameObject.SetActive(false);

        if (KopukGuide.Instance != null)
            KopukGuide.Instance.HideMessage();

        if (GameTimerManager.Instance != null)
            GameTimerManager.Instance.StopTimer();

        if (finalKopukPanel != null)
            finalKopukPanel.SetActive(true);
    }

    public void WrongAnswerLookCloser()
    {
        if (wrongAnswerShowing) return;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayFail();

        StartCoroutine(WrongRoutine(
            "Dumana yaklaþmak güvenli olmayabilir.\nÖnce kendimizi korumalýyýz."
        ));
    }

    public void WrongAnswerHandleMyself()
    {
        if (wrongAnswerShowing) return;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayFail();

        StartCoroutine(WrongRoutine(
            "Tek baþýna müdahale etmek tehlikeli olabilir.\nBöyle durumlarda yardým istemeliyiz."
        ));
    }

    IEnumerator WrongRoutine(string message)
    {
        wrongAnswerShowing = true;

        if (KopukGuide.Instance != null)
            KopukGuide.Instance.ShowMessage(message);

        yield return new WaitForSeconds(1.5f);

        wrongAnswerShowing = false;
    }

    IEnumerator SlidePanel(RectTransform panel, Vector2 startPos, Vector2 endPos)
    {
        float time = 0f;

        while (time < slideDuration)
        {
            time += Time.deltaTime;
            float t = time / slideDuration;
            t = Mathf.SmoothStep(0, 1, t);

            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        panel.anchoredPosition = endPos;
    }

    void CloseAllStage3Panels()
    {
        if (lookAroundPanel != null) lookAroundPanel.SetActive(false);
        if (stage3InteractionPanel != null) stage3InteractionPanel.SetActive(false);
        if (phonePanel != null) phonePanel.SetActive(false);
        if (phonePanel2 != null) phonePanel2.SetActive(false);
        if (finalPanel != null) finalPanel.SetActive(false);
        if (finalKopukPanel != null) finalKopukPanel.SetActive(false);
    }
}