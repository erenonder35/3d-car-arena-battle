using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [Header("UI (Arayüz) Ayarları")]
    public GameObject gameOverObjesi;
    public TextMeshProUGUI oyunSonuYazisi;

    [Header("Oyun Sonu Sesleri")] // 🎵 YENİ EKLENEN KISIM
    public AudioClip kazanmaSesi;
    public AudioClip kaybetmeSesi;

    private bool oyunBittiMi = false;

    void OnTriggerEnter(Collider other)
    {
        // Kurşun geçirmez kuralımız (Hata olmasın diye duruyor)
        if (other.transform.position.y > -2f)
        {
            return;
        }

        // 1. SENSEN (PLAYER)
        if (other.CompareTag("Player"))
        {
            // 🎵 KAYBEDİNCE SES ÇAL
            if (kaybetmeSesi != null)
            {
                AudioSource.PlayClipAtPoint(kaybetmeSesi, Camera.main.transform.position);
            }

            OyunuBitir("KAYBETTİN!\n<size=50>Yeniden başlamak için 'R'ye bas</size>", Color.red);
        }

        // 2. BOT İSE (ENEMY)
        else if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
            Invoke("KalanBotlariSay", 0.1f);
        }
    }

    void KalanBotlariSay()
    {
        GameObject[] kalanBotlar = GameObject.FindGameObjectsWithTag("Enemy");

        if (kalanBotlar.Length == 0)
        {
            // 🎵 KAZANINCA SES ÇAL
            if (kazanmaSesi != null)
            {
                AudioSource.PlayClipAtPoint(kazanmaSesi, Camera.main.transform.position);
            }

            OyunuBitir("KAZANDIN!\n<size=50>Yeniden başlamak için 'R'ye bas</size>", Color.green);
        }
    }

    void OyunuBitir(string mesaj, Color yaziRengi)
    {
        if (gameOverObjesi != null)
        {
            gameOverObjesi.SetActive(true);
            oyunSonuYazisi.text = mesaj;
            oyunSonuYazisi.color = yaziRengi;
        }

        oyunBittiMi = true; // Oyunun bittiğini sisteme bildir
        Time.timeScale = 0f; // Zamanı dondur
    }

    void Update()
    {
        // Sadece oyun bittiyse VE oyuncu 'R' tuşuna basarsa çalışır
        if (oyunBittiMi && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f; // Zamanı dondurulmuş halden çıkar
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Mevcut sahneyi baştan yükle
        }
    }
}