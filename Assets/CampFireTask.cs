using UnityEngine;
using TMPro;
using System.Collections;

public class CampFireTask : MonoBehaviour
{
    public GameObject fireObjects;
    public GameObject emberSmoke;

    public GameObject taskPanel;
    public TextMeshProUGUI taskText;
    public GameObject rewardPanel;
    public Stage2Manager stage2Manager;

    private bool playerInside = false;
    private bool completed = false;
    private bool taskStarted = true;

    void Start()
    {
        if (taskPanel != null)
            taskPanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(false);
    }

    void Update()
    {
        if (taskStarted && playerInside && !completed && Input.GetKeyDown(KeyCode.E))
        {
            completed = true;

            if (fireObjects != null)
                fireObjects.SetActive(false);

            if (emberSmoke != null)
                emberSmoke.SetActive(false);

            SoundManager.Instance.PlayWater();
            SoundManager.Instance.PlaySuccess();

            if (KopukGuide.Instance != null)
            {
                KopukGuide.Instance.ShowMessage(
                    "Aferin! Közleri söndürmek yangýn riskini azaltýr."
                );
            }

            StartCoroutine(ShowStage1Reward());
        }
    }

    IEnumerator ShowStage1Reward()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (taskPanel != null)
            taskPanel.SetActive(false);

        if (rewardPanel != null)
            rewardPanel.SetActive(true);

        RectTransform rewardRect = rewardPanel.GetComponent<RectTransform>();

        Vector2 startPos = new Vector2(-900, rewardRect.anchoredPosition.y);
        Vector2 endPos = new Vector2(0, rewardRect.anchoredPosition.y);

        rewardRect.anchoredPosition = startPos;

        float duration = 0.8f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);
            t = 1f - Mathf.Pow(1f - t, 3f);

            rewardRect.anchoredPosition =
                Vector2.Lerp(startPos, endPos, t);

            yield return null;
        }

        rewardRect.anchoredPosition = endPos;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (rewardPanel != null)
            rewardPanel.SetActive(false);

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Görev 2’ye hazýr mýsýn?\nEtraftaki çöpleri bulmamýza yardým eder misin?"
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (KopukGuide.Instance != null)
        {
            KopukGuide.Instance.ShowMessage(
                "Bazý nesneler doðada býrakýldýðýnda yangýn riski oluþturabilir.\nHadi kamp alanýný birlikte güvenli hale getirelim!"
            );
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        if (stage2Manager != null)
            stage2Manager.StartStage2();

        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null && !completed)
        {
            playerInside = true;

            if (KopukGuide.Instance != null)
            {
                KopukGuide.Instance.ShowMessage(
                    "Harika! Közleri güvenli hale getirebilir misin?"
                );
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null && !completed)
        {
            playerInside = false;

            if (KopukGuide.Instance != null)
            {
                KopukGuide.Instance.ShowMessage(
                    "Kamp ateþine yaklaþ ve közleri güvenli hale getir."
                );
            }
        }
    }
}