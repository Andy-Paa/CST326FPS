using UnityEngine;
using static Weapon;

public class SoundMng : MonoBehaviour
{
    public static SoundMng Instance { get; set; }

    public AudioSource ShootingChannel;

    public AudioSource vectorReload;

    public AudioSource SIMGReload;
    public AudioSource vectorEmpty;

    public AudioClip SIMGShootingClip;
    public AudioClip vectorShootingClip;

    public AudioSource throwablesChannel;
    public AudioClip grenadeSound;
    public AudioClip smokeSound;


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

    public void PlayShootingSound(WeaponType Weapon) // Fully qualify the enum
    {
        switch (Weapon)
        {
            case WeaponType.SIMG:
                ShootingChannel.PlayOneShot(SIMGShootingClip);
                break;
            case WeaponType.VECTOR:
                ShootingChannel.PlayOneShot(vectorShootingClip);
                break;
        }
    }

    public void PlayReloadSound(WeaponType Weapon) // Fully qualify the enum
    {
        switch (Weapon)
        {
            case WeaponType.SIMG:
                SIMGReload.Play();
                break;
            case WeaponType.VECTOR:
                vectorReload.Play();
                break;
        }
    }

    public void PlayEmptySound(WeaponType Weapon) // Fully qualify the enum
    {
        switch (Weapon)
        {
            case WeaponType.SIMG:
                SIMGReload.Play();
                break;
            case WeaponType.VECTOR:
                vectorReload.Play();
                break;
        }
    }
}
