using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("Bullet hit: " + collision.gameObject.name);

            CreateBulletImpactEffect(collision);

            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("Bullet hit: " + collision.gameObject.name);

            CreateBulletImpactEffect(collision);

            Destroy(gameObject);
        }
    }

    void CreateBulletImpactEffect(Collision hitedObject)
    {
        ContactPoint contact = hitedObject.contacts[0];
        GameObject hole = Instantiate(
            GlobalRfs.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        hole.transform.SetParent(hitedObject.transform);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        
    }
}
