using UnityEngine;

public class BombEffect : MonoBehaviour
{
    [Header("Fırlatma Ayarları")]
    public float yatayFirlatmaGucu = 40f;
    public float dikeySicrama = 0.3f;

    [Header("Ses Ayarları")]
    public AudioClip patlamaSesi;

    void OnTriggerEnter(Collider other)
    {
        // Çarpan kişi sen veya bot ise fiziksel patlama HERKES için çalışsın
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Fırlatma yönünü ve gücünü hesapla
                Vector3 firlatmaYonu = other.transform.position - transform.position;
                firlatmaYonu.y = 0;
                firlatmaYonu = firlatmaYonu.normalized;
                firlatmaYonu.y = dikeySicrama;

                // Arabayı fırlat!
                rb.AddForce(firlatmaYonu * yatayFirlatmaGucu, ForceMode.VelocityChange);
            }

            // 🎵 SES KONTROLÜ: Sadece SENSEN (Player) ses çalsın!
            if (other.CompareTag("Player") && patlamaSesi != null)
            {
                // Sesi bombanın patladığı noktada çal
                AudioSource.PlayClipAtPoint(patlamaSesi, transform.position);
            }

            // Bombayı sahneden sil (Bomba herkes için yok olur)
            Destroy(gameObject);
        }
    }
}