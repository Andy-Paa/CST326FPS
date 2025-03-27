using UnityEngine;
using static Weapon;
using static ammoBox;
using static Weaponmng;
using System.Collections.Generic;

public class Weaponmng : MonoBehaviour
{
    public static Weaponmng Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject currentWeapon;

    [Header("Ammo")]
    public int lightAmmo = 0;
    public int energyAmmo = 0;

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

    internal void PickUpThrowable(GameObject pickedThrowable)
    {
        DropCurrentWeapon(pickedThrowable);
        pickedThrowable.transform.SetParent(currentWeapon.transform, false);
        pickedThrowable.transform.localPosition = new Vector3(0, 0, 0);
        pickedThrowable.transform.localRotation = Quaternion.Euler(0, 0, 0);
        pickedThrowable.transform.localScale = new Vector3(1, 1, 1);
        pickedThrowable.GetComponent<Throwable>().isEquipped = true;
    }
}
