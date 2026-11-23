using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Ayarları")]
    public Transform target; // Kameranın takip edeceği obje (Araban)

    [Tooltip("Kameranın arabanın neresinde duracağı (X: Sağ/Sol, Y: Yukarı, Z: Geri)")]
    public Vector3 offset = new Vector3(0f, 5f, -10f);

    [Tooltip("Kameranın yumuşaklık hissi (Ne kadar düşükse o kadar esnek takip eder)")]
    public float smoothSpeed = 10f;

    // Kameranın titrememesi için Update yerine LateUpdate kullanıyoruz (Fizikten hemen sonra çalışır)
    void LateUpdate()
    {
        // Eğer sahnede takip edilecek araba yoksa hata verme, bekle
        if (target == null) return;

        // Arabanın döndüğü yöne göre kameranın durması gereken HEDEF noktayı hesapla
        Vector3 desiredPosition = target.TransformPoint(offset);

        // Kamerayı şu anki yerinden hedef noktaya doğru pürüzsüzce (Lerp ile) kaydır
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Kamera her zaman arabanın tam ortasına (hedefe) baksın
        transform.LookAt(target);
    }
}