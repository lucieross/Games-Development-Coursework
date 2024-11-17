using UnityEngine;
using TMPro; 

public class LevelTimer : MonoBehaviour
{
    public float levelTime = 240f;
    private float timer;
    public TMP_Text timerText; 
    private PlayerManager playerManager; // references to PlayerManager

    void Start()
    {
        timer = levelTime;
        playerManager = FindObjectOfType<PlayerManager>(); 
    }

    void Update()
    {
        if (!PlayerManager.isGameOver) // Check if the game is not over
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                EndLevel();
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time Left: " + Mathf.Ceil(timer).ToString() + " seconds";
        }
    }

    void EndLevel()
    {
        PlayerManager.isGameOver = true; // Set game over state
        Debug.Log("Time's up! Game Over.");
        
    }
}
