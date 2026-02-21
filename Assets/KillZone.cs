using UnityEngine;

public class KillZone : MonoBehaviour
{
    public Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var controller =
    other.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = false;

        other.transform.position =
    respawnPoint.position;

        if (controller != null) controller.enabled = true;
    }
}