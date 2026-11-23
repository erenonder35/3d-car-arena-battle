using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaShrinker : MonoBehaviour
{
    [Header("Daralma Ayarlarý")]
    public float shrinkInterval = 10f;

    [Header("HAYAT KURTARAN AYAR (ÇOK ÖNEMLÝ)")]
    [Tooltip("Komþu sayma radarý. Senin ayarlarýna göre en kusursuz sayý 2.5'tir.")]
    public float radarRadius = 2.5f;

    [Header("Uyarý Sistemi")]
    public Color warningColor = Color.red;
    public float warningDuration = 1.5f;

    [Header("Oyuncu Takibi")]
    public Transform playerTransform;

    private List<GameObject> allHexes = new List<GameObject>();
    private bool isPlayerAlive = true;

    void Start()
    {
        StartCoroutine(StartShrinkingRoutine());
    }

    IEnumerator StartShrinkingRoutine()
    {
        yield return new WaitForSeconds(2f); // Haritanýn oluþmasýný bekle

        foreach (Transform child in transform)
        {
            allHexes.Add(child.gameObject);
        }

        while (allHexes.Count > 0 && isPlayerAlive)
        {
            yield return new WaitForSeconds(shrinkInterval);

            if (playerTransform != null && playerTransform.position.y < -5f)
            {
                isPlayerAlive = false;
                break;
            }

            if (isPlayerAlive)
            {
                ShrinkArena();
            }
        }
    }

    void ShrinkArena()
    {
        allHexes.RemoveAll(item => item == null);
        if (allHexes.Count == 0) return;

        List<GameObject> outermostHexes = new List<GameObject>();

        for (int i = allHexes.Count - 1; i >= 0; i--)
        {
            GameObject currentHex = allHexes[i];
            int neighborCount = 0;

            foreach (var otherHex in allHexes)
            {
                if (currentHex != otherHex)
                {
                    float dist = Vector3.Distance(currentHex.transform.position, otherHex.transform.position);

                    // ÝÞTE BÜYÜK SÝHÝR BURADA:
                    // Eðer mesafe 0.5'ten küçükse (bunlar üst üste binmiþ hatalý klonlardýr, SAYMA!)
                    // Eðer mesafe radarRadius (2.5) içindeyse GERÇEK KOMÞUDUR, say!
                    if (dist > 0.5f && dist <= radarRadius)
                    {
                        neighborCount++;
                    }
                }
            }

            // Hatalý klonlarý çöpe attýðýmýza göre, artýk gerçekten 6'dan az komþusu olan adam %100 KENARDADIR!
            if (neighborCount < 6)
            {
                outermostHexes.Add(currentHex);
                allHexes.RemoveAt(i);
            }
        }

        // Seçilen kurbanlarý aþaðý yolla
        foreach (var hex in outermostHexes)
        {
            StartCoroutine(WarnAndDropHex(hex));
        }
    }

    IEnumerator WarnAndDropHex(GameObject hex)
    {
        Renderer rnd = hex.GetComponentInChildren<Renderer>();
        if (rnd != null) rnd.material.color = warningColor;

        yield return new WaitForSeconds(warningDuration);

        if (hex != null)
        {
            // O GICIK KIRMIZI HATAYI ÇÖZEN KISIM
            MeshCollider mc = hex.GetComponent<MeshCollider>();
            if (mc != null) mc.convex = true;

            Rigidbody rb = hex.GetComponent<Rigidbody>();
            if (rb == null) rb = hex.AddComponent<Rigidbody>();

            rb.isKinematic = false;
            rb.useGravity = true;

            yield return new WaitForSeconds(3f);
            if (hex != null) Destroy(hex);
        }
    }
}