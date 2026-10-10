using UnityEngine;
using System.Collections.Generic;

public class CafeManager : MonoBehaviour
{
    // Initial Data Dictionary
    private Dictionary<string, int> initialIngredients =
        new Dictionary<string, int>()
        {
            {"Kopi Kapal Api", 10},
            {"Tea Box", 8},
            {"Sugar Sachet", 12},
            {"Milk", 4},
            {"Mineral Water", 14}
        };

    private Dictionary<string, int> playerInventory;

    [Header("Mug Component")]
    public GameObject mugPrefab;
    public Transform onTheTray;

    public FirstPersonController player;

    public bool playerInsideCafe = false;

    public bool thereIsMugOnTray = true;

    void Start()
    {
        playerInventory = new Dictionary<string, int>(initialIngredients);

        Debug.Log("Initial Data Dictionary Is Here :");

        foreach (KeyValuePair<string, int> item in playerInventory)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }

    public bool UseIngredient(string ingredientName, int amount)
    {
        if (!playerInventory.ContainsKey(ingredientName))
        {
            Debug.Log("Bahan : " + ingredientName + " Tidak Tersedia! Silahkan Untuk Membeli Bahan " + ingredientName + " di Gerai!");
            return false;
        }

        if (playerInventory[ingredientName] < amount)
        {
            Debug.Log("Bahan " + ingredientName + " Tidak Cukup!");
            return false;
        }

        playerInventory[ingredientName] -= amount;
        Debug.Log("Bahan " + ingredientName + " Tersisa : " + playerInventory[ingredientName]);

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("PLAYER MASUK CAFE");

        playerInsideCafe = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("=== PLAYER KELUAR CAFE ===");

        playerInsideCafe = false;
    }

    public void GenerateMug()
    {
        if (!playerInsideCafe)
        {
            Debug.Log("Player belum berada di dalam Cafe.");
            return;
        }

        Mug prevMug = player.GetComponentInChildren<Mug>(true);

        Debug.Log("Mug yang ditemukan: " + prevMug);

        if (prevMug != null)
        {
            Debug.Log("Player masih membawa Mug: " + prevMug.gameObject.name);
            return;
        }

        Debug.Log(">>> GENERATE MUG BARU <<<");

        GameObject newMug = Instantiate(
            mugPrefab,
            onTheTray.position,
            onTheTray.rotation
        );

        newMug.transform.SetParent(onTheTray, true);

        newMug.transform.localPosition = Vector3.zero;
        newMug.transform.localRotation = Quaternion.Euler(270f, 0f, 0f);

        thereIsMugOnTray = true;
    }

    public void PutBackMug()
    {
        Mug mugOnTray = onTheTray.GetComponentInChildren<Mug>(true);

        if (mugOnTray == null && !thereIsMugOnTray)
        {
            Debug.Log("Tidak ada mug di atas nampan");
            return;
        }

        mugOnTray.transform.SetParent(null);
        Destroy(mugOnTray.gameObject);
        Debug.Log("There Is Mug On Tray: " + thereIsMugOnTray);
        thereIsMugOnTray = false;
    }
}