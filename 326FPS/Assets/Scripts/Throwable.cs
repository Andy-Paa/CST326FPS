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
        NONE,
        SMOKE,
        GRENADE
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
            case ThrowableType.GRENADE:
                GrenadeEffect();
                break;
            case ThrowableType.SMOKE:
                SmokeEffect();
                break;
        }
    }

    private void SmokeEffect()
    {
        GameObject smokeEffect = GlobalRfs.Instance.smokeEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        SoundMng.Instance.throwablesChannel.PlayOneShot(SoundMng.Instance.smokeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearbyObject in colliders)
        {
            // Example: Apply a debuff or obscure vision
            // VisionObscurer vision = nearbyObject.GetComponent<VisionObscurer>();
            // if (vision != null)
            // {
            //     vision.ObscureVision(5f); // Obscure vision for 5 seconds
            // }
        }
    }

    private void GrenadeEffect()
    {
        GameObject explosionEffect = GlobalRfs.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        SoundMng.Instance.throwablesChannel.PlayOneShot(SoundMng.Instance.grenadeSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            if (nearbyObject.gameObject.GetComponent<Enemy>())
            {
                nearbyObject.gameObject.GetComponent<Enemy>().TakeDamage(100);
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