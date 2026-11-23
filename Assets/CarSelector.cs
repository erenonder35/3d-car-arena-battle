using UnityEngine;

public class CarSelector : MonoBehaviour
{
    [Header("Araba Modelleri")]
    [Tooltip("Menüdeki araba sýrasýyla BÝREBÝR AYNI sýrayla ekle!")]
    public GameObject[] arabalar;

    void Start()
    {
        // Arkadaþýnýn menüde kaydettiði sayýyý hafýzadan al (Yoksa 0. arabayý alýr)
        int secilenIndex = PlayerPrefs.GetInt("SecilenAraba", 0);

        // Döngü ile tüm arabalarý kontrol et
        for (int i = 0; i < arabalar.Length; i++)
        {
            // Eðer sýradaki araba, seçtiðimiz arabaysa onu AÇ
            if (i == secilenIndex)
            {
                arabalar[i].SetActive(true);
            }
            // Deðilse onu KAPAT (Görünmez yap)
            else
            {
                arabalar[i].SetActive(false);
            }
        }
    }
}