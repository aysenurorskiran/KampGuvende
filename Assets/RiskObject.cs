using UnityEngine;
using TMPro;

public class RiskObject : MonoBehaviour
{
    public bool isRisky;
    public string objectName;
    public Stage2Manager stage2Manager;
    public TextMeshProUGUI taskText;

    private bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (stage2Manager != null && stage2Manager.IsStage2Active())
            {
                GameObject objectToHide = transform.parent != null ? transform.parent.gameObject : gameObject;

                stage2Manager.OpenQuestion(isRisky, objectName, objectToHide);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            playerNear = true;

            if (stage2Manager != null && stage2Manager.IsStage2Active())
            {
                if (taskText != null)
                    taskText.text = "";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            playerNear = false;
        }
    }
}