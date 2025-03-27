using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Weapon;

public class HUDMng : MonoBehaviour
{    
    public static HUDMng Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

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

    private void Update()
    {
        Weapon activeWeapon = Weaponmng.Instance.currentWeapon?.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = Weaponmng.Instance.weaponSlots.Find(w => w != Weaponmng.Instance.currentWeapon)?.GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";
            Weapon.WeaponType model = activeWeapon.weaponType;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);
            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.weaponType);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }
    }
    private Sprite GetAmmoSprite(Weapon.WeaponType model)
    {
        switch (model)
        {
            case Weapon.WeaponType.VECTOR:
                return Instantiate(Resources.Load<GameObject>("light_Ammo")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponType.SIMG:
                return Instantiate(Resources.Load<GameObject>("energy_Ammo")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponType model)
    {
        switch (model)
        {
            case Weapon.WeaponType.VECTOR:
                return Instantiate(Resources.Load<GameObject>("Vectoricn")).GetComponent<SpriteRenderer>().sprite;

            case Weapon.WeaponType.SIMG:
                return Instantiate(Resources.Load<GameObject>("SIMGicn")).GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSprite()
    {
        foreach (GameObject weapon in Weaponmng.Instance.weaponSlots)
        {
            if (weapon != Weaponmng.Instance.currentWeapon)
            {
                return weapon;
            }
        }
        return null;
    }
}
