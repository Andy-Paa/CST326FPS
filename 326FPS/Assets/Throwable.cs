using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;
    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        Grenade
    }

    public ThrowableType throwableType;

    void Start()
    {
        countdown = delay;
    }

    void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    public void Throw(Vector3 force)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(force, ForceMode.Impulse);
            hasBeenThrown = true;
        }
    }

    void Explode()
    {
        GetThrowableEffect();

        hasExploded = true;

        // Show explosion effect (placeholder)
        Debug.Log("Boom!");

        // Detect objects in the damage radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // Apply explosion force
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            // Damage logic (placeholder)
            // Example: Health health = nearbyObject.GetComponent<Health>();
            // if (health != null) health.TakeDamage(damageAmount);
        }

        // Destroy the throwable object
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;
        }
    }

    private void GrenadeEffect()
    {
        GameObject explosionEffect = GlobalRfs.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
            // Health health = nearbyObject.GetComponent<Health>();
            // if (health != null)
            // {
            //     health.TakeDamage(50);
            // }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the damage radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}