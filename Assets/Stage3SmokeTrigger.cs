using UnityEngine;

public class Stage3SmokeTrigger : MonoBehaviour
{
    public Stage3Manager stage3Manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stage3Manager.PlayerEnteredSmokeArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            stage3Manager.PlayerExitedSmokeArea();
        }
    }
}