using System;
using UnityEngine;

public class RubbishTrash : MonoBehaviour
{
    private FirstPersonController player;

    public bool isInAreaRubbishBin = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        player = other.GetComponent<FirstPersonController>();
        player.currentRubbishTrash = this;
        isInAreaRubbishBin = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        player.currentRubbishTrash = null;
        player = null;
        isInAreaRubbishBin = false;
    }

    public void HandleWasteTrash()
    {
        NPCTrash currentTrash = player.GetComponentInChildren<NPCTrash>();

        if (!currentTrash)
        {
            return;
        }

        // currentTrash.transform.SetParent(null);
        Destroy(currentTrash.gameObject);

        Debug.Log("Complete to waste current trash!");
    }
}
