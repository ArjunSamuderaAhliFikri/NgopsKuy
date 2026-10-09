using UnityEngine;

public class NPCTrash : MonoBehaviour
{
    bool withInRadiusCup = false;

    private FirstPersonController player;

    private Renderer targetRenderer;
    private Material targetMaterial;
    private Color originalEmissionColor;

    [Header("Material Settings")]
    public float speedBlink = 5.0f;
    public float maximumEmission = 10.0f;
    public float emissionIntensity = 20.0f;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        targetMaterial = targetRenderer.material;

        targetMaterial.EnableKeyword("_EMISSION");
        originalEmissionColor = targetMaterial.GetColor("_EmissionColor");

        SetEmission();
    }

    void Update()
    {
        HandleBlinkingEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        withInRadiusCup = true;
        player = other.GetComponent<FirstPersonController>();

        Debug.Log("Trigger Entered From NPCTrash : " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        withInRadiusCup = false;
        player = null;

        Debug.Log("Trigger Exited From NPCTrash : " + other.gameObject.name);
    }

    private void SetEmission()
    {
        Color newColor = originalEmissionColor * emissionIntensity;

        targetMaterial.SetColor("_EmissionColor", newColor);
    }

    public void HandleBlinkingEffect()
    {
        float countSynus = (Mathf.Sin(Time.time * speedBlink) + 1.0f) / 2.0f;

        float currentEmission = countSynus * maximumEmission;

        Color newColor = originalEmissionColor * currentEmission;

        targetMaterial.SetColor("_EmissionColor", newColor);
    }

    public void HandlePlayerPickingUpCup()
    {
        if (withInRadiusCup && player)
        {
            transform.SetParent(player.onTheTray);

            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(12f, 12f, 12f);

            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            Debug.Log("Player successfully picked up the cup from NPCTrash !");
        }
    }

}
