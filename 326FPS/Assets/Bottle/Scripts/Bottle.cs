using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] GameObject brokenBottlePrefab;
    
    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞物体是否在 "bullet" 层
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Explode();
        }
    }
    
    void Explode()
    {
        GameObject brokenBottle = Instantiate(brokenBottlePrefab, this.transform.position, Quaternion.identity);
        brokenBottle.GetComponent<BrokenBottle>().RandomVelocities();
        Destroy(gameObject);
    }
}
