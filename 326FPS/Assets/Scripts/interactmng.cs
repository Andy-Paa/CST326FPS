using UnityEngine;

public class interactmng : MonoBehaviour
{
    public static interactmng Instance { get; set; }

    public Weapon hoveredWeapon = null;
    public ammoBox hoveredAmmoBox = null;
    public Throwable hoveredThrowable = null;

    public GameObject interactText;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<Weapon>() && hitObject.GetComponent<Weapon>().isEquipped == false)
            {
                hoveredWeapon = hitObject.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Weaponmng.Instance.PickUpWeapon(hitObject.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            if (hitObject.GetComponent<ammoBox>())
            {
                hoveredAmmoBox = hitObject.gameObject.GetComponent<ammoBox>();
                hoveredAmmoBox.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Weaponmng.Instance.PickUpAmmo(hoveredAmmoBox);
                    Destroy(hitObject.gameObject);
                }
            }
            else
            {
                if (hoveredAmmoBox)
                {
                    hoveredAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }

            if (hitObject.GetComponent<Throwable>())
            {
                hoveredThrowable = hitObject.gameObject.GetComponent<Throwable>();
                hoveredThrowable.GetComponent<Outline>().enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Weaponmng.Instance.PickUpThrowable(hoveredThrowable);
                }
            }
            else
            {
                if (hoveredThrowable)
                {
                    hoveredThrowable.GetComponent<Outline>().enabled = false;
                }
            }
        }
    }
}
