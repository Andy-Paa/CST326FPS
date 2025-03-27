using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Weapon : MonoBehaviour
{
    public bool isEquipped;

    // public Camera playerCamera;
    public bool isShooting, readyToShoot;
    public bool allowReset = true;
    public float timeBetweenShots = 0.09f;

    public int bulletsPerBurst = 5;
    public float timeBetweenBursts = 0.02f;
    public int currentBurst;

    public float spread = 0.3f;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 100;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;

    internal Animator animator;

    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 weaponPosition;
    public Vector3 weaponRotation;
    public Vector3 weaponScale;

    public enum WeaponType { SIMG, VECTOR };

    public WeaponType weaponType;

    public enum ShootingMode { Auto, Burst, Single };
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        currentBurst = bulletsPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEquipped)
        {
            GetComponent<Outline>().enabled = false;
            if (bulletsLeft == 0 && isShooting){
            SoundMng.Instance.vectorEmpty.Play();
            }

            if(currentShootingMode == ShootingMode.Auto){
                isShooting = Input.GetKey(KeyCode.Mouse0);
            } else if(currentShootingMode == ShootingMode.Burst){
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            } else if(currentShootingMode == ShootingMode.Single){
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKeyDown(KeyCode.R) && isReloading==false && bulletsLeft < magazineSize){
                Reload();
            }

            if (readyToShoot && isShooting==false && bulletsLeft <= 0 && isReloading==false){
                Reload();
            }

            if(isShooting && readyToShoot && bulletsLeft > 0 && isReloading==false){
                currentBurst = bulletsPerBurst;
                FireWeapon();
            }

            // if (ammoMng.Instance.ammoDisplay != null){
            //     ammoMng.Instance.ammoDisplay.text = $"{bulletsLeft/bulletsPerBurst}/{magazineSize/bulletsPerBurst}";
            // }
        }
    }
    

    private void FireWeapon(){
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        // SoundMng.Instance.vectorShooting.Play();

        SoundMng.Instance.PlayShootingSound(weaponType);

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

    private void Reload(){
        // SoundMng.Instance.vectorReload.Play();

        SoundMng.Instance.PlayReloadSound(weaponType);

        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished(){
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot(){
        allowReset = true;
        readyToShoot = true;
    }

    public Vector3 CalculateSpread(){
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
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
