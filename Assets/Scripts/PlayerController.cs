using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    private Coroutine powerupCountdownCoroutine;

    public float playerSpeed = 120f;

    [Header("Powerup Configuration")]
    public float jumpForce = 5f;
    public float smashCooldown = 10f;
    public float maximumSmashForce = 50f;
    public float smashRadius = 7.5f;
    private float upwardsModifier = 0.5f;
    private bool isOnGround = true;
    public bool isSmashingDown = false;
    private float nextSmashTime;
    public bool hasPowerup = false;
    public float powerupStrength = 45.0f;
    public GameObject powerupIndicator;

    [Header("Smash Cooldown UI")]
    public Slider smashCooldownSlider;

    [Header("Rocket Ability")]
    public GameObject rocketProjectile;
    public int fireCount = 3;
    public float fireDelay = 0.25f;

    void Awake()
    {
        controls = new InputSystem_Actions();
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smashCooldownSlider.minValue = 0f;
        smashCooldownSlider.maxValue = 1f;
        smashCooldownSlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        SmashAttack();
        UpdateSmashCooldownSlider();

        if (transform.position.y < -10)
        {
            Debug.Log("Game Over!");
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
    }

    void MovePlayer()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float forwardInput = moveInput.y; // W/S Arrow Keys
        playerRb.AddForce(focalPoint.transform.forward * playerSpeed * forwardInput * Time.deltaTime);
    }

    void SmashAttack()
    {
        bool isAttacking = controls.Player.Jump.WasPressedThisFrame();
        if (isAttacking && isOnGround && Time.time >= nextSmashTime)
        {
            isOnGround = false;
            nextSmashTime = Time.time + smashCooldown;
            StartCoroutine(PerformSmashAttack());
        }
    }

    private IEnumerator PerformSmashAttack()
    {
        playerRb.linearVelocity = Vector3.zero;
        playerRb.AddForce(focalPoint.transform.up * jumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.75f);

        isSmashingDown = true;
        playerRb.AddForce(-focalPoint.transform.up * jumpForce * 3, ForceMode.Impulse);
    }

    private void PerformSmashImpact()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, smashRadius);

        foreach (Collider hit in hits) {
            if (!hit.CompareTag("Enemy"))
            {
                continue;
            }

            Rigidbody enemyRb = hit.GetComponent<Rigidbody>();
            if (enemyRb != null)
            {
                enemyRb.AddExplosionForce(
                    maximumSmashForce,
                    transform.position,
                    smashRadius,
                    upwardsModifier,
                    ForceMode.Impulse
                );
            }
        }
    }


    void UpdateSmashCooldownSlider()
    {
        float remainingCooldown = Mathf.Max(0f, nextSmashTime - Time.time);
        smashCooldownSlider.value = 1f - remainingCooldown / smashCooldown;
    }

    private void OnTriggerEnter(Collider other)
    {
        PowerupPickup pickup = other.GetComponentInParent<PowerupPickup>();
        if (pickup == null)
        {
            return;
        }

        switch (pickup.powerupType)
        {
            case PowerupType.Knockback:
                ActivateKnockbackPowerup();
                break;

            case PowerupType.Rockets:
                StartCoroutine(FireRocketVolleys());
                break;
        }

        Destroy(pickup.gameObject);
    }

    private void ActivateKnockbackPowerup()
    {
        hasPowerup = true;
        powerupIndicator.SetActive(true);

        if (powerupCountdownCoroutine != null)
        {
            StopCoroutine(powerupCountdownCoroutine);
        }

        powerupCountdownCoroutine = StartCoroutine(PowerupCountdownRoutine());
    }

    private IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.SetActive(false);
        hasPowerup = false;
        powerupCountdownCoroutine = null;
    }

    private IEnumerator FireRocketVolleys()
    {
        for (int i = 0; i < fireCount; i++)
        {
            FireRockets();
            yield return new WaitForSeconds(fireDelay);
        }
    }

    private void FireRockets()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Vector3 fireDirection = (enemy.transform.position - transform.position).normalized;
            Quaternion fireRotation = Quaternion.FromToRotation(Vector3.up, fireDirection);
            GameObject rocket = Instantiate(rocketProjectile, transform.position, fireRotation);
            rocket.GetComponent<RocketProjectile>().Initialize(enemy.transform, RocketOwner.Player);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            Debug.Log("Collided with " + collision.gameObject.name
                + " with powerup set to " + hasPowerup);
            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;

            if (isSmashingDown)
            {
                isSmashingDown = false;
                PerformSmashImpact();
            }
        }
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
