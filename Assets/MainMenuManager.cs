using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Elemanları")]
    public CanvasGroup fadePanel;
    public CanvasGroup menuGroup;
    public Button oynaButton;
    public Button cikisButton;
    public Image carImage;

    [Header("Arayüz Panelleri")]
    public GameObject mainMenuPanel;
    public GameObject carSelectionPanel;
    public GameObject skySelectionPanel;

    [Header("Araba Galeri Sistemi")]
    public GameObject[] arabalar;
    public TextMeshProUGUI arabaAdiText;
    private int seciliArabaIndex = 0;

    [Header("YENİ: Ekonomi ve Dükkan Sistemi")]
    public int[] arabaFiyatlari;
    public GameObject kilitIkonu;
    public TextMeshProUGUI fiyatText;
    public Button satinAlButonu;
    public Button arabayiSecButonu;
    public TextMeshProUGUI toplamAltinText;

    [Header("Gökyüzü Galeri Sistemi")]
    [Tooltip("0:Gündüz, 1:Gece, 2:Kızıl, 3:Uzay sırasıyla koymayı unutma!")]
    public Material[] gokyuzleri;
    private int seciliGokyuzuIndex = 0;

    [Header("Gökyüzü Resimleri (Efekt İçin)")]
    [Tooltip("Hiyerarşideki o 4 fotoğrafı sırasıyla buraya at")]
    public Image[] gokyuzuResimleri;

    [Header("Animasyon Ayarları")]
    public float fadeSuresi = 1.5f;
    public float girisAnimSuresi = 1.2f;

    [Header("Sahne Ayarı")]
    public string oyunSahnesi = "Harun_Atolye";

    private bool oynaBasladi = false;

    void Start()
    {
        // TEST İÇİN: Başlangıç altını
        PlayerPrefs.SetInt("ToplamAltin", 1000);

        mainMenuPanel.SetActive(true);
        carSelectionPanel.SetActive(false);
        skySelectionPanel.SetActive(false);

        if (menuGroup != null) menuGroup.alpha = 0f;
        if (fadePanel != null) fadePanel.alpha = 1f;

        if (oynaButton != null) oynaButton.onClick.AddListener(OynaBaslat);
        if (cikisButton != null) cikisButton.onClick.AddListener(OyundanCik);

        StartCoroutine(MenuAc());

        for (int i = 0; i < arabalar.Length; i++)
        {
            if (arabalar[i] != null)
                arabalar[i].SetActive(i == 0);
        }
        seciliArabaIndex = 0;

        GuncelleArabaEkrani();
        GokyuzuSec(0);
    }

    public void IleriAraba()
    {
        if (arabalar.Length == 0) return;
        arabalar[seciliArabaIndex].SetActive(false);
        seciliArabaIndex = (seciliArabaIndex + 1) % arabalar.Length;
        arabalar[seciliArabaIndex].SetActive(true);
        GuncelleArabaEkrani();
    }

    public void GeriAraba()
    {
        if (arabalar.Length == 0) return;
        arabalar[seciliArabaIndex].SetActive(false);
        seciliArabaIndex--;
        if (seciliArabaIndex < 0) seciliArabaIndex = arabalar.Length - 1;
        arabalar[seciliArabaIndex].SetActive(true);
        GuncelleArabaEkrani();
    }

    private void GuncelleArabaEkrani()
    {
        if (arabalar.Length > 0 && arabaAdiText != null)
            arabaAdiText.text = arabalar[seciliArabaIndex].name;

        int mevcutAltin = PlayerPrefs.GetInt("ToplamAltin", 0);
        if (toplamAltinText != null) toplamAltinText.text = "KASA: " + mevcutAltin + " Altın";

        int arabaAcikMi = PlayerPrefs.GetInt("ArabaAcik_" + seciliArabaIndex, 0);
        if (seciliArabaIndex == 0) arabaAcikMi = 1;

        if (arabaAcikMi == 1)
        {
            if (kilitIkonu != null) kilitIkonu.SetActive(false);
            if (fiyatText != null) fiyatText.gameObject.SetActive(false);
            if (satinAlButonu != null) satinAlButonu.gameObject.SetActive(false);
            if (arabayiSecButonu != null) arabayiSecButonu.gameObject.SetActive(true);
        }
        else
        {
            if (kilitIkonu != null) kilitIkonu.SetActive(true);
            if (fiyatText != null) fiyatText.gameObject.SetActive(true);
            if (satinAlButonu != null) satinAlButonu.gameObject.SetActive(true);
            if (arabayiSecButonu != null) arabayiSecButonu.gameObject.SetActive(false);

            int fiyat = 0;
            if (arabaFiyatlari != null && seciliArabaIndex < arabaFiyatlari.Length)
                fiyat = arabaFiyatlari[seciliArabaIndex];

            if (fiyatText != null) fiyatText.text = fiyat + " Altın";

            if (satinAlButonu != null)
                satinAlButonu.interactable = (mevcutAltin >= fiyat);
        }
    }

    public void ArabaSatinAl()
    {
        int mevcutAltin = PlayerPrefs.GetInt("ToplamAltin", 0);
        int fiyat = (arabaFiyatlari != null && seciliArabaIndex < arabaFiyatlari.Length) ? arabaFiyatlari[seciliArabaIndex] : 0;

        if (mevcutAltin >= fiyat)
        {
            mevcutAltin -= fiyat;
            PlayerPrefs.SetInt("ToplamAltin", mevcutAltin);
            PlayerPrefs.SetInt("ArabaAcik_" + seciliArabaIndex, 1);
            PlayerPrefs.Save();
            GuncelleArabaEkrani();
        }
    }

    public void GokyuzuGorselGuncelle(int secilenNumara)
    {
        for (int i = 0; i < gokyuzuResimleri.Length; i++)
        {
            if (gokyuzuResimleri[i] != null)
            {
                if (i == secilenNumara)
                {
                    gokyuzuResimleri[i].transform.localScale = new Vector3(1.15f, 1.15f, 1f);
                    gokyuzuResimleri[i].color = Color.white;
                }
                else
                {
                    gokyuzuResimleri[i].transform.localScale = Vector3.one;
                    gokyuzuResimleri[i].color = new Color(0.3f, 0.3f, 0.3f, 1f);
                }
            }
        }
    }

    public void GokyuzuSec(int index)
    {
        if (gokyuzleri.Length > 0 && index < gokyuzleri.Length)
        {
            seciliGokyuzuIndex = index;
            RenderSettings.skybox = gokyuzleri[index];
            DynamicGI.UpdateEnvironment();
            GokyuzuGorselGuncelle(index);
        }
    }

    public void OynaBaslat()
    {
        if (oynaBasladi) return;
        oynaBasladi = true;
        StartCoroutine(OynaGecis());
    }

    IEnumerator OynaGecis()
    {
        if (oynaButton != null) oynaButton.interactable = false;
        yield return StartCoroutine(EkranSallama(0.15f, 8f));
        if (menuGroup != null) yield return StartCoroutine(FadeCanvasGroup(menuGroup, 1f, 0f, 0.3f));
        mainMenuPanel.SetActive(false);
        carSelectionPanel.SetActive(true);
    }

    // ARABAYI SEÇTİĞİMİZDE BURASI ÇALIŞIR
    public void ArabaSecildiGokyuzuneGec()
    {
        // SEÇİLEN ARABA INDEX'İ BURADA KASAYA KAYDEDİLİYOR
        PlayerPrefs.SetInt("SecilenAraba", seciliArabaIndex);

        carSelectionPanel.SetActive(false);
        skySelectionPanel.SetActive(true);
    }

    public void SavasaBasla()
    {
        PlayerPrefs.SetInt("SecilenGokyuzu", seciliGokyuzuIndex);
        PlayerPrefs.Save(); // Tüm verileri diske kaydet
        SceneManager.LoadScene(oyunSahnesi);
    }

    // --- YARDIMCI METODLAR ---
    IEnumerator MenuAc()
    {
        yield return new WaitForSeconds(0.3f);
        if (fadePanel != null) yield return StartCoroutine(FadeCanvasGroup(fadePanel, 1f, 0f, fadeSuresi));
        if (menuGroup != null) yield return StartCoroutine(FadeCanvasGroup(menuGroup, 0f, 1f, girisAnimSuresi));
        StartCoroutine(ArabaParilti());
        if (oynaButton != null) StartCoroutine(OynaButonNabiz());
    }

    public void OyundanCik() { Application.Quit(); }

    IEnumerator ArabaParilti()
    {
        while (true)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 0.4f;
                float parlaklik = Mathf.Lerp(0.85f, 1.05f, Mathf.PingPong(t * 2f, 1f));
                if (carImage != null) carImage.color = new Color(parlaklik, parlaklik, parlaklik, 1f);
                yield return null;
            }
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator OynaButonNabiz()
    {
        RectTransform rt = oynaButton.GetComponent<RectTransform>();
        Vector3 orijinalBoyut = rt.localScale;
        while (!oynaBasladi)
        {
            float t = Mathf.PingPong(Time.time * 1.2f, 1f);
            float boyut = Mathf.Lerp(1f, 1.06f, t);
            rt.localScale = orijinalBoyut * boyut;
            yield return null;
        }
        rt.localScale = orijinalBoyut;
    }

    IEnumerator EkranSallama(float sure, float siddet)
    {
        Vector3 orijinal = Camera.main.transform.localPosition;
        float gecenSure = 0f;
        while (gecenSure < sure)
        {
            gecenSure += Time.deltaTime;
            float x = Random.Range(-siddet, siddet) * 0.01f;
            float y = Random.Range(-siddet, siddet) * 0.01f;
            Camera.main.transform.localPosition = orijinal + new Vector3(x, y, 0f);
            yield return null;
        }
        Camera.main.transform.localPosition = orijinal;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float baslangic, float bitis, float sure)
    {
        float gecenSure = 0f;
        cg.alpha = baslangic;
        while (gecenSure < sure)
        {
            gecenSure += Time.deltaTime;
            cg.alpha = Mathf.Lerp(baslangic, bitis, gecenSure / sure);
            yield return null;
        }
        cg.alpha = bitis;
    }
}