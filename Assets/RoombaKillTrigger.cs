using UnityEngine;

public class RoombaKillTrigger : MonoBehaviour
{
    private PlayerRespawn respawn;

    void Start()
    {
        respawn = Object.FindFirstObjectByType<PlayerRespawn>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (respawn != null)
            respawn.RespawnPlayer();
    }
}