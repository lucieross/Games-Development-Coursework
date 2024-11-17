using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes
using System.Collections;

public class Portal : MonoBehaviour
{
    public string menuSceneName; 
    public GameObject PortalParticles;
    public Sprite alternateSprite;
    private SpriteRenderer spriteRenderer;

    public int nextsceneLoad;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        nextsceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    void Update()
    {
        int collectedCoins = CoinManager.GetCoinCount();
        if (CoinManager.GetCoinCount() == 5){
            spriteRenderer.sprite = alternateSprite;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (CoinManager.GetCoinCount() == 5) 
        {
            if (other.CompareTag("Player"))
            {
                Instantiate(PortalParticles, transform.position, Quaternion.identity); //particles
                AudioManager.instance.Play("LevelComplete");
                StartCoroutine(TeleportAfterDelay(2f)); // Start the coroutine with a 2 sec delay

            }

        }
    }

   private IEnumerator TeleportAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay); // Wait for the  delay

    if(SceneManager.GetActiveScene().buildIndex == 4) //equal to last level last level is 4
    {
        SceneManager.LoadScene(0);
    }
    else
    {
        SceneManager.LoadScene(nextsceneLoad);

        HeartManager.health = 3; //resetgs hearts each level
        CoinManager.coinsCollected = 0; // Resets coins each level
        ScoreScript.scoreValue = 0; //Reset Score each level
        
        // Updates PlayerPrefs only if the next scene is greater than the current saved level to unlock next level
        if(nextsceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextsceneLoad);
            PlayerPrefs.Save(); 
        }
    }
}


    void LoadMenu()
    {
        SceneManager.LoadScene(menuSceneName); // Loads scene
    }
}
