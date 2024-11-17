using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static Boolean isGameOver;
    public GameObject GameOverScreen;

    
    public void Awake()
    {
        isGameOver = false;
    }
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            GameOverScreen.SetActive(true);
        }
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reloads current screen
        HeartManager.health = 3; 
        CoinManager.coinsCollected = 0; // Reset coins
        ScoreScript.scoreValue = 0; //Reset Score
        isGameOver = false; // Reset game over state
        GameOverScreen.SetActive(false); // Hide the Game Over screen
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll(); // Deletes all PlayerPrefs data
        PlayerPrefs.Save(); 
        Debug.Log("PlayerPrefs reset.");
    }

}
