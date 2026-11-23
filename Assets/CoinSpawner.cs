using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject altinPrefab;
    public float spawnSuresi = 3f; // Her 3 saniyede 1 altýn çýksýn
    public float arenaBuyuklugu = 15f; // Arenanýn geniþliðine göre ayarla

    void Start()
    {
        // Oyunu baþlat ve sürekli Spawnla
        InvokeRepeating("AltinUret", 2f, spawnSuresi);
    }

    void AltinUret()
    {
        // Rastgele X ve Z koordinatlarý belirle
        float rastgeleX = Random.Range(-arenaBuyuklugu, arenaBuyuklugu);
        float rastgeleZ = Random.Range(-arenaBuyuklugu, arenaBuyuklugu);

        // Y yüksekliði 1 olsun ki altýgenlerin hemen üstünde havada dursun
        Vector3 spawnNoktasi = new Vector3(rastgeleX, 2f, rastgeleZ);

        Instantiate(altinPrefab, spawnNoktasi, Quaternion.Euler(90, 0, 0)); // Parayý dik duracak þekilde yarat
    }
}