using UnityEngine;

public class GlobalRfs : MonoBehaviour
{
    public static GlobalRfs Instance{get; set;}
    public GameObject bulletImpactEffectPrefab;
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
