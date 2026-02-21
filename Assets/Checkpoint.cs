using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform newRespawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        KillZone[] zones =
    FindObjectsOfType<KillZone>();
        foreach (KillZone zone in zones)
        {
            zone.respawnPoint = newRespawnPoint;
        }

        Debug.Log("Checkpoint Reached!");
    }
}