using UnityEngine;
using TMPro;

public class Level2Instructions : MonoBehaviour
{
    public TextMeshProUGUI messageText; 
    public GameObject FallingPanel; 
    public float displayDuration = 10.0f; // Duration to show the text

    private bool messageShown = false; // ttracks if the message has been shown

    private void Start()
    {
        messageText.gameObject.SetActive(false); // Start with the text hidden
        FallingPanel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !messageShown) // Checks if players first time entering
        {
            ShowMessage("This is a night time level. It is HARDER. The mobs are stronger and take 2 hits to kill but some of them drop coins. Rememeber you need 5 to unlock the portal. Be aware and be careful you are on your own now, traveller. You got this!");
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
