using System.Collections;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [Header("Animasyon Ayarları")]
    public float dropDistance = 5f;
    public float animationDuration = 1f;
    public float waitTime = 4f;

    [HideInInspector] public int coordX;
    [HideInInspector] public int coordY;

    private Renderer hexRenderer;
    private Collider hexCollider;
    private bool isStepped = false;
    private Color originalColor;
    private Vector3 originalPosition;

    void Start()
    {
        hexRenderer = GetComponent<Renderer>();
        hexCollider = GetComponent<Collider>();
        originalColor = hexRenderer.material.color;
        originalPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // DÜZELTME: Artık isimlere değil, "Player" ve "Enemy" etiketlerine bakıyoruz!
        // Böylece botEnemy (1), (2), (99) hepsi çalışacak.
        if (!isStepped && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")))
        {
            isStepped = true;
            StartCoroutine(FallSequence());
        }
    }

    IEnumerator FallSequence()
    {
        hexRenderer.material.color = Color.yellow;
        yield return new WaitForSeconds(0.5f);

        hexRenderer.material.color = new Color(1f, 0.5f, 0f);
        yield return new WaitForSeconds(0.5f);

        hexRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);

        hexCollider.enabled = false;
        Vector3 targetDownPosition = originalPosition - new Vector3(0, dropDistance, 0);

        float elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, targetDownPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetDownPosition;

        yield return new WaitForSeconds(waitTime);

        hexRenderer.material.color = originalColor;

        elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            transform.position = Vector3.Lerp(targetDownPosition, originalPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;

        hexCollider.enabled = true;
        isStepped = false;
    }

    public void ShrinkAndFall()
    {
        StopAllCoroutines();
        StartCoroutine(PermanentFallSequence());
    }

    IEnumerator PermanentFallSequence()
    {
        hexRenderer.material.color = Color.red;

        // Üstündeki araç için 1 saniyelik kaçış süresi
        yield return new WaitForSeconds(1f);

        hexCollider.enabled = false;

        // ✅ startPos sabit tutuldu — titreme hatası giderildi
        Vector3 startPos = transform.position;
        Vector3 deepSpace = startPos - new Vector3(0, 20f, 0);

        float elapsedTime = 0;
        while (elapsedTime < animationDuration)
        {
            transform.position = Vector3.Lerp(startPos, deepSpace, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}