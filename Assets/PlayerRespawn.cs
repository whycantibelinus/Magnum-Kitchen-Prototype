using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform currentRespawnPoint;

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void RespawnPlayer()
    {
        if (currentRespawnPoint == null)
        {
            Debug.LogWarning("No respawn point set!");
            return;
        }

        if (controller != null)
            controller.enabled = false;

        transform.position = currentRespawnPoint.position;

        if (controller != null)
            controller.enabled = true;
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        currentRespawnPoint = newPoint;
    }
}