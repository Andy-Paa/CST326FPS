using UnityEngine;

public class SoundMng : MonoBehaviour
{
    public static SoundMng Instance{get; set;}

    public AudioSource vectorShooting;

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
