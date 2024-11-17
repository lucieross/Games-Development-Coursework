using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuEvents : MonoBehaviour
{

    void start()
    {

    }

    public void LoadLevel(int index)
    {
        // Load the specified scene by index
        SceneManager.LoadScene(index);
    }

    void Start()
    {
      
    }

    private void ResetPlayerPrefs() //uses to reset player data
    {
        PlayerPrefs.DeleteAll(); // Deletes all PlayerPrefs data
        PlayerPrefs.Save(); 
        Debug.Log("PlayerPrefs have been reset.");
    }
}
