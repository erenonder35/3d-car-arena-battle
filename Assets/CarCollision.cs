using UnityEngine;

public class CarCollision : MonoBehaviour
{
    [Header("Çarpýþma Ayarlarý")]
    public float carpismaSiddeti = 15f; // Geri sekme gücü
    public float minHizSarti = 2f;      // Sadece belli bir hýzýn üstünde çarpýþýnca çalýþsýn (ufak dokunuþlarda uçmasýnlar)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // OnTriggerEnter deðil, gerçek fizik çarpýþmalarý için OnCollisionEnter kullanýyoruz
    void OnCollisionEnter(Collision collision)
    {
        // Çarptýðýmýz obje bir araba mý? (Player veya Enemy)
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            // Ýki arabanýn çarpýþma anýndaki baðýl hýzýný (þiddetini) ölçüyoruz
            float carpmaHizi = collision.relativeVelocity.magnitude;

            // Eðer arabalar sadece birbirine sürtünmüyorsa, gerçekten sert çarpýþtýlarsa:
            if (carpmaHizi > minHizSarti && rb != null)
            {
                // Çarpýþmanýn yönünü bul (Karþý arabadan benim arabama doðru bir vektör)
                Vector3 itmeYonu = transform.position - collision.gameObject.transform.position;
                itmeYonu.y = 0; // Havaya uçmasýnlar, yatayda itilsinler
                itmeYonu = itmeYonu.normalized;

                // Sürtünmeyi kýrmak için minik bir sýçrama efekti (opsiyonel)
                itmeYonu.y = 0.1f;

                // Gücü uygula! (Çarpma hýzý ne kadar yüksekse, sekme o kadar þiddetli olur)
                // ForceMode.Impulse, arabanýn aðýrlýðýný (Mass) hesaba katarak gerçekçi bir itme yapar.
                //rb.AddForce(itmeYonu * carpismaSiddeti * (carpmaHizi / 5f), ForceMode.Impulse);
                // Impulse yerine VelocityChange kullanarak arabanýn kilosunu (Mass) hiçe sayýyoruz!
                rb.AddForce(itmeYonu * carpismaSiddeti * (carpmaHizi / 5f), ForceMode.VelocityChange);
            }
        }
    }
}