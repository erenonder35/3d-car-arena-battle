using UnityEngine;

public class SavasGokyuzuMudur : MonoBehaviour
{
    [Tooltip("0:Gündüz, 1:Uzay, 2:Kızıl, 3:Gece (Menüdeki sırayla BİREBİR AYNI olmalı)")]
    public Material[] oyunGokyuzleri;

    void Start()
    {
        // Menüde kaydedilen gökyüzü numarasını oku (Eğer hata olursa varsayılan olarak 0'ı yani Gündüz'ü aç)
        int secilenNumara = PlayerPrefs.GetInt("SecilenGokyuzu", 0);

        // Sahnenin gökyüzünü bu numaraya göre ayarla
        if (oyunGokyuzleri.Length > 0 && secilenNumara < oyunGokyuzleri.Length)
        {
            RenderSettings.skybox = oyunGokyuzleri[secilenNumara];
            DynamicGI.UpdateEnvironment(); // Işıkları tazele
        }
    }
}