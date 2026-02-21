using UnityEngine;

public class Crumb : MonoBehaviour
{
    public GameObject roombaToActivate; // Leave empty for normal crumbs

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Activate roomba if assigned
        if (roombaToActivate != null)
        {
            roombaToActivate.SetActive(true);
        }

        Destroy(gameObject);
        Debug.Log("Crumb Collected!");
    }
}