using System.Collections;
using System.ComponentModel;
using NUnit.Framework;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions controls;
    private Rigidbody playerRb;
    private GameObject focalPoint;

    public float playerSpeed = 120f;
    public bool hasPowerup = false;
    public float powerupStrength = 45.0f;
    public GameObject powerupIndicator;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    private IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.SetActive(false);
        hasPowerup = false;
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
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
