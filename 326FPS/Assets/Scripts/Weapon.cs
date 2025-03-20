using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;
    public bool isShooting, readyToShoot;
    public bool allowReset = true;
    public float timeBetweenShots = 0.1f;

    public int bulletsPerBurst = 5;
    public float timeBetweenBursts = 0.5f;
    public int currentBurst;

    public float spread = 0.1f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public enum ShootingMode { Auto, Burst, Single };
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        currentBurst = bulletsPerBurst;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentShootingMode == ShootingMode.Auto){
            isShooting = Input.GetKey(KeyCode.Mouse0);
        } else if(currentShootingMode == ShootingMode.Burst){
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        } else if(currentShootingMode == ShootingMode.Single){
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if(isShooting && readyToShoot){
            currentBurst = bulletsPerBurst;
            FireWeapon();
        }
    }
    

    private void FireWeapon(){
        readyToShoot = false;

        Vector3 shootingDirection = CalculateSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyBullet(bullet, bulletPrefabLifeTime));

        if(allowReset){
            Invoke("ResetShot", timeBetweenShots);
            allowReset = false;
        }

        if(currentShootingMode == ShootingMode.Burst && currentBurst > 1){
            currentBurst--;
            Invoke("FireWeapon", timeBetweenBursts);
        }
    }

    private void ResetShot(){
        allowReset = true;
        readyToShoot = true;
    }

    public Vector3 CalculateSpread(){
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit)){
            targetPoint = hit.point;
        } else {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        float xSpread = UnityEngine.Random.Range(-spread, spread);
        float ySpread = UnityEngine.Random.Range(-spread, spread);

        return direction + new Vector3(xSpread, ySpread, 0);
    }

    private IEnumerator DestroyBullet(GameObject bullet, float delay){
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
