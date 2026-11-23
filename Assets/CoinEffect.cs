using UnityEngine;

public class CoinEffect : MonoBehaviour
{
    public float donmeHizi = 150f;

    [Header("Ses Ayarları")]
    public AudioClip toplamaSesi;

    void Update()
    {
        // Altını fırıl fırıl döndür
        transform.Rotate(0, donmeHizi * Time.deltaTime, 0);

        // Aşağı düşerse sil
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Çarpan obje bizim araba (Player) veya bot (Enemy) ise
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // SADECE çarpan kişi "Player" (sen) ise ses çal!
            if (other.CompareTag("Player") && toplamaSesi != null)
            {
                AudioSource.PlayClipAtPoint(toplamaSesi, transform.position);
            }

            // Ama kim çarparsa çarpsın altını sahneden yok et
            Destroy(gameObject);
        }
    }
}