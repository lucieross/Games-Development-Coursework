using UnityEngine;
using TMPro;

public class Level4Instructions : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public GameObject FallingPanel; 
    public float displayDuration = 10.0f; // show text for this much time 

    private bool messageShown = false; 

    private void Start()
    {
        messageText.gameObject.SetActive(false); // Starts with the text hidden
        FallingPanel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !messageShown) 
        {
            ShowMessage("The enemies are harder now, they have trained and now follow you. Make sure you hide until you are ready to fight! They also get alot faster after the first hit :) GOOD LUCK");
            messageShown = true; 
        }
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        FallingPanel.gameObject.SetActive(true);
        Invoke("HideMessage", displayDuration); // Hide the message after a delay
    }

    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
        FallingPanel.gameObject.SetActive(false);
    }
}
