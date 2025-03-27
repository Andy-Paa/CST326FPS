using UnityEngine;

public class ammoBox : MonoBehaviour
{
    public int ammoAmount = 300;
    public AmmoType ammoType;
    public enum AmmoType
    {
        light_Ammo,
        energy_Ammo
    }
}
