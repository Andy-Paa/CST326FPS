using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{

    public int bulletDamage = 10;

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
        if (collision.gameObject.CompareTag("Zombie"))
        {
            print("Bullet hit: " + collision.gameObject.name);
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }

            CreateBloodEffect(collision);
            //CreateBulletImpactEffect(collision);

            Destroy(gameObject);
        }
    }

    void CreateBloodEffect(Collision hitedObject)
    {
        ContactPoint contact = hitedObject.contacts[0];
        GameObject blood = Instantiate(
            GlobalRfs.Instance.bloodEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );
        blood.transform.SetParent(hitedObject.transform);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        //blood.transform.rotation = rot;
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
