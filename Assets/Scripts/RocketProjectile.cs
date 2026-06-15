using System;
using UnityEngine;

public enum RocketOwner
{
    Player,
    Boss
}

public class RocketProjectile : MonoBehaviour
{
    private Rigidbody rocketRb;
    private Transform target;
    private RocketOwner owner;

    public float speed = 5.0f;
    public float turnSpeed = 180.0f;
    public float impactForce = 20.0f;

    void Awake()
    {
        rocketRb = GetComponent<Rigidbody>();
    }

    public void Initialize(Transform newTarget, RocketOwner newOwner)
    {
        target = newTarget;
        owner = newOwner;
    }

    void FixedUpdate()
    {
        if (target == null) // Destroys the projectile when enemy is destroyed
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - rocketRb.position).normalized;

        Quaternion rotation = Quaternion.RotateTowards(
            rocketRb.rotation,
            Quaternion.FromToRotation(Vector3.up, direction),
            turnSpeed * Time.deltaTime);

        rocketRb.MoveRotation(rotation);
        rocketRb.linearVelocity = rotation * Vector3.up * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (owner == RocketOwner.Player && other.CompareTag("Enemy"))
        {
            ApplyImpact(other);
        }
        else if (owner == RocketOwner.Boss && other.GetComponent<PlayerController>() != null)
        {
            ApplyImpact(other);
        }
    }

    private void ApplyImpact(Collider other)
    {
        Rigidbody targetRb = other.GetComponent<Rigidbody>();

        if (targetRb != null)
        {
            targetRb.AddForce(transform.up * impactForce, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }

}
