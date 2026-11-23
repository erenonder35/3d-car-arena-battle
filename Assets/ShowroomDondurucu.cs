using UnityEngine;

public class ShowroomDondurucu : MonoBehaviour
{
    [Header("Döndürme Ayarları")]
    public float klavyeHizi = 150f;
    public float fareHizi = 500f;

    void Update()
    {
        // 1. KLAVYE İLE DÖNDÜRME (A-D veya Sağ-Sol Ok Tuşları)
        // Input.GetAxis("Horizontal") sağa basınca 1, sola basınca -1 verir.
        float klavyeGirdisi = Input.GetAxis("Horizontal");
        if (klavyeGirdisi != 0)
        {
            // Vector3.up (Y ekseni) etrafında döndürüyoruz. Ters dönerse eksi (-) işaretini kaldırabilirsin.
            transform.Rotate(Vector3.up, -klavyeGirdisi * klavyeHizi * Time.deltaTime);
        }

        // 2. FARE SÜRÜKLEMESİ İLE DÖNDÜRME (Sol Tık Basılıyken)
        if (Input.GetMouseButton(0))
        {
            // Farenin X eksenindeki (sağ-sol) hareket hızını al
            float fareX = Input.GetAxis("Mouse X");

            // Y ekseni etrafında farenin hızına göre döndür
            transform.Rotate(Vector3.up, -fareX * fareHizi * Time.deltaTime);
        }
    }
}