using UnityEngine;
using static Weapon;
using static ammoBox;
using static Weaponmng;
using static HUDMng;
using static Throwable;
using System.Collections.Generic;

public class Weaponmng : MonoBehaviour
{
    public static Weaponmng Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject currentWeapon;

    [Header("Ammo")]
    public int lightAmmo = 0;
    public int energyAmmo = 0;

    [Header("Throwable")]
    public float throwforce = 40f;
    public float forceMultiplier = 0f;
    public float forceMultiplierMax = 10f;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;

    public int LethalsCount = 0;
    public Throwable.ThrowableType currentLethal;

    public int TacticalCount = 0;
    public Throwable.ThrowableType currentTactical;
    public GameObject smokePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentWeapon = weaponSlots[0];

        currentLethal = Throwable.ThrowableType.NONE;
        currentTactical = Throwable.ThrowableType.NONE;
    }

    private void Update()
    {
        foreach (GameObject weapon in weaponSlots)
        {
            if (weapon == currentWeapon)
            {
                weapon.SetActive(true);
            }
            else
            {
                weapon.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = weaponSlots[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = weaponSlots[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (GameObject weapon in weaponSlots)
            {
            weapon.SetActive(false);
            }
            currentWeapon = null;
        }

        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.Q))
        {
            forceMultiplier += Time.deltaTime;

            if (forceMultiplier > forceMultiplierMax)
            {
                forceMultiplier = forceMultiplierMax;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (LethalsCount > 0)
            {
                ThrowLethal();
                HUDMng.Instance.UpdateThrowables();
            }
            forceMultiplier = 0f;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (TacticalCount > 0)
            {
                ThrowTactical();
                HUDMng.Instance.UpdateThrowables();
            }
            forceMultiplier = 0f;
        }
    }

    public void PickUpWeapon(GameObject pickedweapon)
    {
        AddWeaponIntoActiveSlot(pickedweapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedweapon)
    {
        DropCurrentWeapon(pickedweapon);

        pickedweapon.transform.SetParent(currentWeapon.transform, false);
        Weapon weapon = pickedweapon.GetComponent<Weapon>();
        pickedweapon.transform.localPosition = new Vector3(weapon.weaponPosition.x, weapon.weaponPosition.y, weapon.weaponPosition.z);
        pickedweapon.transform.localRotation = Quaternion.Euler(weapon.weaponRotation.x, weapon.weaponRotation.y, weapon.weaponRotation.z);
        pickedweapon.transform.localScale = new Vector3(weapon.weaponScale.x, weapon.weaponScale.y, weapon.weaponScale.z);
        weapon.isEquipped = true;
        weapon.animator.enabled = true;
    }

    internal void PickUpAmmo(ammoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoType.light_Ammo:
                lightAmmo += ammo.ammoAmount;
                break;
            case AmmoType.energy_Ammo:
                energyAmmo += ammo.ammoAmount;
                break;
        }
    }

    private void DropCurrentWeapon(GameObject pickedweapon)
    {
        if (currentWeapon.transform.childCount > 0)
        {
            var weaponToDrop = currentWeapon.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isEquipped = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedweapon.transform.parent);
            weaponToDrop.transform.position = pickedweapon.transform.localPosition;
            weaponToDrop.transform.rotation = pickedweapon.transform.localRotation;
            weaponToDrop.transform.localScale = pickedweapon.transform.localScale;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponType weaponType){
        switch(weaponType){
            case Weapon.WeaponType.SIMG:
                return energyAmmo;
            case Weapon.WeaponType.VECTOR:
                return lightAmmo;
            default:
                return 0;
        }
    }

    internal void DecreaseAmmo(int decreasenumber, Weapon.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case Weapon.WeaponType.VECTOR:
                lightAmmo -= decreasenumber;
                break;
            case Weapon.WeaponType.SIMG:
                energyAmmo -= decreasenumber;
                break;
        }
    }

    public void PickUpThrowable(Throwable pickedThrowable)
    {
        switch (pickedThrowable.throwableType)
        {
            case Throwable.ThrowableType.GRENADE:
                PickUpThrowableAsLethal(Throwable.ThrowableType.GRENADE);
                break;
            case Throwable.ThrowableType.SMOKE:
                PickUpThrowableAsTactical(Throwable.ThrowableType.SMOKE);
                break;
        }
    }

    private void PickUpThrowableAsTactical(Throwable.ThrowableType tactical)
    {
        if (currentTactical == tactical || currentTactical == Throwable.ThrowableType.NONE)
        {
            currentTactical = tactical;

            if (TacticalCount < 2)
            {
                TacticalCount += 1;
                Destroy(interactmng.Instance.hoveredThrowable.gameObject);
                HUDMng.Instance.UpdateThrowables();
            }
            else
            {
                print("You already have max tacticals");
            }
        }
        else
        {
            print("You already have a type of tactical");
        }
    }

    private void PickUpThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if (currentLethal == lethal || currentLethal == Throwable.ThrowableType.NONE)
        {
            currentLethal = lethal;

            if (LethalsCount <2){
                LethalsCount +=1;
                Destroy(interactmng.Instance.hoveredThrowable.gameObject);
                HUDMng.Instance.UpdateThrowables();
            }
            else
            {
                print("You already have max lethals");
            }
        }
        else
        {
            print("You already have a type of lethal");
        }
    }

    private void ThrowTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(currentTactical);
        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwforce + forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        TacticalCount -= 1;

        if (TacticalCount <= 0)
        {
            currentTactical = Throwable.ThrowableType.NONE;
        }

        HUDMng.Instance.UpdateThrowables();
    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(currentLethal);
        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwforce + forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        LethalsCount -= 1;

        if (LethalsCount <= 0)
        {
            currentLethal = Throwable.ThrowableType.NONE;
        }

        HUDMng.Instance.UpdateThrowables();
    }

    private GameObject GetThrowablePrefab(Throwable.ThrowableType currentLethal)
    {
        switch (currentLethal)
        {
            case Throwable.ThrowableType.GRENADE:
                return grenadePrefab;
            case Throwable.ThrowableType.SMOKE:
                return smokePrefab;
        }

        return null;
    }
}
