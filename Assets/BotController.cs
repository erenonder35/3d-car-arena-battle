using UnityEngine;
using System.Collections.Generic;

public class BotController : MonoBehaviour
{
    [Header("Bot Ayarları")]
    public float botSpeed = 50f;
    public float jumpForce = 15f;
    public float maxSpeed = 14f;

    [Header("Davranış Ayarları")]
    public float retreatDistance = 2f;
    public float retreatForce = 60f;
    public float orbitStrength = 0.3f;

    [Header("Hedef Seçim Ayarları")]
    public int maxBotsTargetingPlayer = 1;
    public float retargetInterval = 4f;

    [Header("Hedef (Oyuncu)")]
    public Transform player;

    [Header("Yapay Zeka (Çukur Algılama)")]
    public float lookAheadDistance = 2.5f;
    public float groundCheckDistance = 2f;

    [Header("Tekerlek Ayarları")]
    public Transform[] wheels;
    public float wheelSpinMultiplier = 150f;

    private enum BotRole { Aggressor, Brawler }

    private static HashSet<BotController> botsTargetingPlayer = new HashSet<BotController>();
    private static BotController currentAggressor = null;

    private Rigidbody rb;
    private bool isGrounded;
    private Transform currentTarget;
    private bool isTargetingPlayer = false;
    private BotRole myRole = BotRole.Brawler;
    private float retargetTimer = 0f;

    private float aggressorSpeed;
    private float aggressorMaxSpeed;
    private float aggressorRetreatDistance;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        retargetTimer = Random.Range(0f, retargetInterval);
        TryBecomeAggressor();
        PickNewTarget();
    }

    void OnDestroy()
    {
        botsTargetingPlayer.Remove(this);
        if (currentAggressor == this)
        {
            currentAggressor = null;
            AssignNewAggressorFromAll();
        }
    }

    void TryBecomeAggressor()
    {
        if (currentAggressor == null)
        {
            currentAggressor = this;
            myRole = BotRole.Aggressor;
            aggressorSpeed = botSpeed * 1.4f;
            aggressorMaxSpeed = maxSpeed * 1.3f;
            aggressorRetreatDistance = retreatDistance * 0.5f;
        }
        else
        {
            myRole = BotRole.Brawler;
        }
    }

    static void AssignNewAggressorFromAll()
    {
        BotController[] allBots = Object.FindObjectsByType<BotController>(FindObjectsSortMode.None);
        List<BotController> candidates = new List<BotController>();

        foreach (BotController bot in allBots)
        {
            if (bot != null && bot.gameObject.activeInHierarchy)
                candidates.Add(bot);
        }

        if (candidates.Count == 0) return;

        BotController newAggressor = candidates[Random.Range(0, candidates.Count)];
        newAggressor.myRole = BotRole.Aggressor;
        newAggressor.aggressorSpeed = newAggressor.botSpeed * 1.4f;
        newAggressor.aggressorMaxSpeed = newAggressor.maxSpeed * 1.3f;
        newAggressor.aggressorRetreatDistance = newAggressor.retreatDistance * 0.5f;
        currentAggressor = newAggressor;

        botsTargetingPlayer.Add(newAggressor);
        newAggressor.isTargetingPlayer = true;
        newAggressor.currentTarget = newAggressor.player;
    }

    void PickNewTarget()
    {
        if (myRole == BotRole.Aggressor)
        {
            botsTargetingPlayer.Add(this);
            isTargetingPlayer = true;
            currentTarget = player;
            return;
        }

        BotController[] allBots = Object.FindObjectsByType<BotController>(FindObjectsSortMode.None);
        List<Transform> otherBots = new List<Transform>();

        foreach (BotController bot in allBots)
        {
            if (bot != this && bot != null && bot.gameObject.activeInHierarchy)
                otherBots.Add(bot.transform);
        }

        botsTargetingPlayer.Remove(this);
        isTargetingPlayer = false;

        if (otherBots.Count > 0)
            currentTarget = otherBots[Random.Range(0, otherBots.Count)];
        else
        {
            botsTargetingPlayer.Add(this);
            isTargetingPlayer = true;
            currentTarget = player;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        retargetTimer += Time.fixedDeltaTime;
        if (retargetTimer >= retargetInterval)
        {
            retargetTimer = 0f;
            PickNewTarget();
        }

        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
            PickNewTarget();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.2f);

        Vector3 sensorPosition = transform.position + (transform.forward * lookAheadDistance);
        bool isGapAhead = !Physics.Raycast(sensorPosition, Vector3.down, groundCheckDistance);
        if (isGapAhead && isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Vector3 toTarget = currentTarget.position - transform.position;
        toTarget.y = 0;
        float distanceToTarget = toTarget.magnitude;
        Vector3 directionToTarget = toTarget.normalized;

        float activeSpeed = myRole == BotRole.Aggressor ? aggressorSpeed : botSpeed;
        float activeMaxSpeed = myRole == BotRole.Aggressor ? aggressorMaxSpeed : maxSpeed;
        float activeRetreat = myRole == BotRole.Aggressor ? aggressorRetreatDistance : retreatDistance;
        float activeOrbit = myRole == BotRole.Aggressor ? orbitStrength * 0.3f : orbitStrength;

        if (distanceToTarget < activeRetreat)
        {
            rb.AddForce(-directionToTarget * retreatForce, ForceMode.Acceleration);
        }
        else
        {
            Vector3 perpendicular = Vector3.Cross(directionToTarget, Vector3.up).normalized;
            float sideSign = Mathf.Sin(Time.time * 0.5f + GetInstanceID()) > 0 ? 1f : -1f;
            Vector3 approachDir = (directionToTarget + perpendicular * activeOrbit * sideSign).normalized;

            if (rb.linearVelocity.magnitude < activeMaxSpeed)
                rb.AddForce(approachDir * activeSpeed, ForceMode.Acceleration);
        }

        transform.LookAt(new Vector3(currentTarget.position.x, transform.position.y, currentTarget.position.z));
    }

    void Update()
    {
        float currentSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        float rotationAmount = currentSpeed * wheelSpinMultiplier * Time.deltaTime;

        foreach (Transform wheel in wheels)
        {
            if (wheel != null)
                wheel.Rotate(rotationAmount, 0, 0, Space.Self);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (currentTarget != null)
        {
            if (myRole == BotRole.Aggressor)
                Gizmos.color = Color.red;
            else if (isTargetingPlayer)
                Gizmos.color = Color.yellow;
            else
                Gizmos.color = Color.cyan;

            Gizmos.DrawLine(transform.position, currentTarget.position);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, retreatDistance);
    }
}