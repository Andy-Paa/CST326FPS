using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Weapon;
using System.Collections.Generic;

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
    public Sprite GreySlot;

    public GameObject crosshair;

    // 缓存字典
    private Dictionary<Weapon.WeaponType, Sprite> ammoSprites = new Dictionary<Weapon.WeaponType, Sprite>();
    private Dictionary<Weapon.WeaponType, Sprite> weaponSprites = new Dictionary<Weapon.WeaponType, Sprite>();

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

        // 预加载资源
        PreloadSprites();
    }

    private void PreloadSprites()
    {
        // 预加载 Ammo Sprites
        ammoSprites[Weapon.WeaponType.VECTOR] = Resources.Load<GameObject>("light_Ammo").GetComponent<SpriteRenderer>().sprite;
        ammoSprites[Weapon.WeaponType.SIMG] = Resources.Load<GameObject>("energy_Ammo").GetComponent<SpriteRenderer>().sprite;

        // 预加载 Weapon Sprites
        weaponSprites[Weapon.WeaponType.VECTOR] = Resources.Load<GameObject>("Vectoricn").GetComponent<SpriteRenderer>().sprite;
        weaponSprites[Weapon.WeaponType.SIMG] = Resources.Load<GameObject>("SIMGicn").GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        Weapon activeWeapon = Weaponmng.Instance.currentWeapon?.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = Weaponmng.Instance.weaponSlots.Find(w => w != Weaponmng.Instance.currentWeapon)?.GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{Weaponmng.Instance.CheckAmmoLeftFor(activeWeapon.weaponType)}";
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


        if (Weaponmng.Instance.LethalsCount <= 0)
        {
            lethalUI.sprite = GreySlot;
        }

        if (Weaponmng.Instance.TacticalCount <= 0)
        {
            tacticalUI.sprite = GreySlot;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponType model)
    {
        // 从缓存中获取 Sprite
        return ammoSprites.ContainsKey(model) ? ammoSprites[model] : null;
    }

    private Sprite GetWeaponSprite(Weapon.WeaponType model)
    {
        // 从缓存中获取 Sprite
        return weaponSprites.ContainsKey(model) ? weaponSprites[model] : null;
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

    internal void UpdateThrowables()
    {
        lethalAmountUI.text = $"{Weaponmng.Instance.LethalsCount}";
        tacticalAmountUI.text = $"{Weaponmng.Instance.TacticalCount}";

        switch (Weaponmng.Instance.currentLethal)
        {
            case Throwable.ThrowableType.GRENADE:

                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch (Weaponmng.Instance.currentTactical)
        {
            case Throwable.ThrowableType.SMOKE:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
