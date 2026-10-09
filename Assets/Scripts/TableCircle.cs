using UnityEngine;

public class TableCircle : MonoBehaviour
{
    [Header("Table Properties")]
    public Transform mugNextPlacement;

    private bool playerInsideTable = false;

    [Header("Player Properties")]
    private FirstPersonController player;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Player masuk radius meja!");

        playerInsideTable = true;

        player = other.GetComponent<FirstPersonController>();

        if (player == null)
        {
            Debug.LogError("FirstPersonController tidak ditemukan pada Player!");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Player keluar radius meja!");

        playerInsideTable = false;
        player = null;
    }


    private void Update()
    {
        if (!playerInsideTable)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Mug mugOnTable =
                mugNextPlacement.GetComponentInChildren<Mug>(true);

            if (mugOnTable != null)
            {
                // Ada Mug di meja
                // Ambil kembali Mug
                PutBackMug();
            }
            else
            {
                // Tidak ada Mug di meja
                // Letakkan Mug dari Player
                TakeOffMug();
            }
        }
    }


    // =========================================================
    // PLAYER → TABLE
    // =========================================================

    private void TakeOffMug()
    {
        if (player == null)
        {
            Debug.LogError("Player belum ditemukan!");
            return;
        }

        Mug mug = player.GetComponentInChildren<Mug>(true);

        Debug.Log("Mug yang dibawa Player : " + mug);

        if (mug == null)
        {
            Debug.Log("Player tidak membawa Mug.");
            return;
        }

        Debug.Log("Mug diletakkan ke meja.");

        mug.transform.SetParent(mugNextPlacement);

        mug.transform.localPosition = Vector3.zero;

        mug.transform.localRotation =
            Quaternion.Euler(270f, 0f, 0f);

        mug.transform.localScale =
            new Vector3(2f, 2f, 2f);
    }


    // =========================================================
    // TABLE → PLAYER
    // =========================================================

    private void PutBackMug()
    {
        Mug mugOnTable =
            mugNextPlacement.GetComponentInChildren<Mug>(true);

        if (mugOnTable == null)
        {
            Debug.Log("Tidak ada Mug di meja.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player belum ditemukan!");
            return;
        }

        if (player.onTheTray == null)
        {
            Debug.LogError(
                "OnTheTray belum diisi pada FirstPersonController!"
            );

            return;
        }

        Debug.Log("Mengambil Mug dari meja.");

        mugOnTable.transform.SetParent(player.onTheTray);

        mugOnTable.transform.localPosition =
            Vector3.zero;

        mugOnTable.transform.localRotation =
            Quaternion.Euler(270f, 0f, 0f);

        mugOnTable.transform.localScale =
            new Vector3(4f, 4f, 4f);

        Debug.Log("Mug berhasil kembali ke tray Player.");
    }
}