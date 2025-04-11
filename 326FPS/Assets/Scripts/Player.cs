using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;


public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;

    public TextMeshProUGUI hpText;
    public GameObject gameOverText;
    public bool isDead = false; 

    private void Start()
    {
        bloodyScreen.SetActive(false);
        hpText.text = "HP:" + HP.ToString();
    }
    
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            print("Player is dead!");
            PlayerDead();
            isDead = true; // Set the player as dead
            SoundMng.Instance.playerChannel.PlayOneShot(SoundMng.Instance.playerDeath);
        }
        else
        {
            print("Player took damage: " + damage + ", remaining HP: " + HP);
            hpText.text = "HP:" + HP.ToString();
            StartCoroutine(BloodyScreenEffect());
            SoundMng.Instance.playerChannel.PlayOneShot(SoundMng.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        SoundMng.Instance.playerChannel.clip = SoundMng.Instance.gameover;
        SoundMng.Instance.playerChannel.PlayDelayed(1f);
        GetComponent<MouseMovement>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        
        GetComponentInChildren<Animator>().enabled = true;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        hpText.gameObject.SetActive(false);

        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverText());
    }

    private IEnumerator ShowGameOverText()
    {
        yield return new WaitForSeconds(5f);
        gameOverText.SetActive(true);
    }

    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame.
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            if (!isDead){// Ignore if the player is dead
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
}
