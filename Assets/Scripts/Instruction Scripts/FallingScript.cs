using UnityEngine;
using TMPro;

public class FallingScript : MonoBehaviour
{
    public TextMeshProUGUI messageText; 
    public GameObject FallingPanel; 
    public float displayDuration = 3f; // Duration to show the text

    private void Start()
    {
        messageText.gameObject.SetActive(false); // Starts with text hidden
        FallingPanel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            ShowMessage("This next part can be difficult. These platforms drop if you you are on them too long watch out!");
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
