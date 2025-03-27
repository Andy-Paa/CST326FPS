using UnityEngine;

public class interactmng : MonoBehaviour
{
    public static interactmng Instance { get; set; }

    public Weapon hoveredWeapon = null;

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
        }
    }
}
