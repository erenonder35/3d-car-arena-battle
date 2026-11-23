using UnityEngine;
using TMPro; // Yazý sistemi için

public class Cuzdan : MonoBehaviour
{
    public int toplamAltin = 0;
    public TextMeshProUGUI altinYazisi;

    void OnTriggerEnter(Collider other)
    {
        // Eðer çarptýðýmýz þeyin etiketi Coin ise
        if (other.CompareTag("Coin"))
        {
            toplamAltin += 10; // Her altýn 10 deðerinde olsun
            altinYazisi.text = "Altýn: " + toplamAltin;

            // Topladýðýmýz altýný sahneden sil
            Destroy(other.gameObject);
        }
    }
}