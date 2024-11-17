using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScript : MonoBehaviour
{
    public Button[] levelButtons; // Array of level buttons to manage if they are active

    void Start()
    {
        //PlayerPrefs.DeleteAll(); //for testing to reset progress 

        // Gets the highest level that the player has reached and unlocks any they have reached
        int levelAt = PlayerPrefs.GetInt("levelAt", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1; //level index = 1 cuase menu is 0

            // Enable or disables button based on the player progress
            levelButtons[i].interactable = (levelIndex <= levelAt);

            // Adds listener to load the level if it's unlocked
            if (levelButtons[i].interactable)
            {
                levelButtons[i].onClick.AddListener(() => LoadLevel(levelIndex));
            }
        }
    }
    void LoadLevel(int levelIndex)
    {
        // Load the level scene based on the level index
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + levelIndex);
    }
}
