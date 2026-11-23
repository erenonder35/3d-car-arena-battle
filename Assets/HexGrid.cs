using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [Header("Arena Ayarları")]
    public GameObject hexPrefab;

    [Tooltip("Merkezden dışa doğru kaç halka altıgen olacak?")]
    public int arenaYaricapi = 5;

    [Header("Grid Ayarları")]
    public float xOffset = 1.732f;
    public float zOffset = 1.5f;

    [Header("Daralma Ayarları")]
    public float shrinkInterval = 3f; // Her 3 saniyede bir dıştan içe daralır

    // Tüm hex'leri axial koordinatıyla tutan sözlük
    private Dictionary<Vector2Int, HexTile> hexMap = new Dictionary<Vector2Int, HexTile>();
    private int currentRing; // Şu an hangi halkayı düşüreceğiz

    void Start()
    {
        currentRing = arenaYaricapi; // En dıştan başla
        ArenayiOlustur();
        StartCoroutine(ShrinkLoop());
    }

    void ArenayiOlustur()
    {
        for (int q = -arenaYaricapi; q <= arenaYaricapi; q++)
        {
            int r1 = Mathf.Max(-arenaYaricapi, -q - arenaYaricapi);
            int r2 = Mathf.Min(arenaYaricapi, -q + arenaYaricapi);

            for (int r = r1; r <= r2; r++)
            {
                float xPos = (q + r / 2f) * xOffset;
                float zPos = r * zOffset;

                Vector3 spawnPos = new Vector3(xPos, 0, zPos);
                GameObject yeniHex = Instantiate(hexPrefab, spawnPos, Quaternion.Euler(0, 30, 0));

                yeniHex.name = $"Hex_{q}_{r}";
                yeniHex.transform.SetParent(this.transform);

                // ✅ Koordinatları HexTile'a ata
                HexTile tile = yeniHex.GetComponent<HexTile>();
                tile.coordX = q;
                tile.coordY = r;

                // ✅ Sözlüğe kaydet
                hexMap[new Vector2Int(q, r)] = tile;
            }
        }
    }

    // ✅ Her 3 saniyede bir dıştan içe bir halka düşür
    IEnumerator ShrinkLoop()
    {
        while (currentRing > 0)
        {
            yield return new WaitForSeconds(shrinkInterval);
            HalkayiDusur(currentRing);
            currentRing--;
        }
    }

    void HalkayiDusur(int ring)
    {
        // Axial koordinatta bir halkadaki hex'lerin |q|, |r|, |q+r| değerlerinden
        // en az biri "ring"e eşit olur — bu klasik hexagonal ring tespit formülü
        foreach (var kvp in hexMap)
        {
            int q = kvp.Key.x;
            int r = kvp.Key.y;

            bool kenarMi = Mathf.Max(Mathf.Abs(q), Mathf.Abs(r), Mathf.Abs(q + r)) == ring;

            if (kenarMi && kvp.Value != null)
            {
                kvp.Value.ShrinkAndFall();
            }
        }
    }
}