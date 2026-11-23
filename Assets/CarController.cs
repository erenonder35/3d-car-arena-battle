using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Araba Ayarlar�")]
    public float moveSpeed = 150f;
    public float turnSpeed = 150f;

    [Header("Z�plama Ayarlar�")]
    public float jumpForce = 10f; // Z�plama g�c� (Yetmezse art�r)
    public float groundCheckDistance = 1.2f; // Yere de�di�imizi anlayan lazerin uzunlu�u

    // --- YEN� EKLENEN KISIM BA�LANGICI ---
    [Header("Tekerlek Ayarlar�")]
    public Transform[] wheels; // 0, 1, 2, 3 isimli tekerlekleri buraya atayaca��z
    public float wheelSpinMultiplier = 150f; // D�n�� h�z� �arpan�
    // --- YEN� EKLENEN KISIM B�T��� ---

    private Rigidbody rb;
    private bool jumpRequested = false;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. ZEM�N KONTROL�
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        // 2. ZIPLAMA EMR�
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }

        // --- YEN� EKLENEN KISIM BA�LANGICI (Tekerlek D�nd�rme) ---
        // Araban�n o anki ileri do�ru h�z�n� hesapla
        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);

        // H�za g�re d�n�� miktar�n� belirle
        float rotationAmount = currentSpeed * wheelSpinMultiplier * Time.deltaTime;

        // Atad���m�z t�m tekerlekleri X ekseninde d�nd�r
        foreach (Transform wheel in wheels)
        {
            if (wheel != null)
            {
                wheel.Rotate(rotationAmount, 0, 0, Space.Self);
            }
        }
        // --- YEN� EKLENEN KISIM B�T��� ---
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // �leri - Geri
        Vector3 moveForce = transform.forward * moveInput * moveSpeed;
        rb.AddForce(moveForce, ForceMode.Acceleration);

        // D�n��
        rb.angularVelocity = Vector3.zero;
        transform.Rotate(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);

        // Z�plama Fizi�ini Uygula
        if (jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }
}