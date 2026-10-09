using UnityEngine;

public class CafeManager : MonoBehaviour
{
    [Header("Mug Component")]
    public GameObject mugPrefab;
    public Transform onTheTray;

    public FirstPersonController player;

    public bool playerInsideCafe = false;

    public bool thereIsMugOnTray = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("=== PLAYER MASUK CAFE ===");

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