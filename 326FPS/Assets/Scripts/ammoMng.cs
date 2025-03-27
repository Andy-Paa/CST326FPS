using UnityEngine;
using TMPro;

public class ammoMng : MonoBehaviour
{
    public static ammoMng Instance{get; set;}

    public TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
