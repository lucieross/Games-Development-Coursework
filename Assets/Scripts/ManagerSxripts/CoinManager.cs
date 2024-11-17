using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static int coinsCollected = 0; // keep track of coins
    public int scoreValue = 1; // Amount of score
    private ScoreScript scoreScript;

    private void Start()
    {
        // reference to ScoreScript
        scoreScript = FindObjectOfType<ScoreScript>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player hit the coin
        {
            // Update the score
            AudioManager.instance.Play("CoinCollection");
            scoreScript.AddScore(scoreValue);
            coinsCollected++;

            Debug.Log("Coin collected! Current coins: " + coinsCollected);
            Destroy(gameObject); 
        }
    }

    public static int GetCoinCount()
    {
        return coinsCollected; 
    }
}
