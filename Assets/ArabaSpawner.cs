using UnityEngine;

public class ArabaSpawner : MonoBehaviour
{
    [Header("Oyundaki Arabalar")]
    [Tooltip("DİKKAT: Buradaki sıralama, ana menüdeki sıralamayla BİREBİR AYNI olmalıdır!")]
    public GameObject[] arenadakiArabalar;

    void Start()
    {
        // 1. Ana menüde hafızaya kaydettiğimiz araba numarasını çekiyoruz
        // (Eğer hafızada bir şey bulamazsa, varsayılan olarak 0. arabayı yani Cayman'ı seçer)
        int secilenArabaIndex = PlayerPrefs.GetInt("SecilenAraba", 0);

        // 2. Arenadaki tüm arabaları gizle
        for (int i = 0; i < arenadakiArabalar.Length; i++)
        {
            if (arenadakiArabalar[i] != null)
                arenadakiArabalar[i].SetActive(false);
        }

        // 3. Sadece hafızadan gelen numaradaki arabayı aktif et!
        if (arenadakiArabalar.Length > 0 && secilenArabaIndex < arenadakiArabalar.Length)
        {
            arenadakiArabalar[secilenArabaIndex].SetActive(true);
        }
    }
}