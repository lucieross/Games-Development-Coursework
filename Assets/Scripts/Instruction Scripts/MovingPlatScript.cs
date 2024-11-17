using UnityEngine;
using TMPro;

public class MovingPlatScript : MonoBehaviour
{
    public TextMeshProUGUI messageText; 
    public GameObject MovingPanel; 
    public float displayDuration = 3f; // elnght to display the text

    private void Start()
    {
        messageText.gameObject.SetActive(false); // Start with the text hidden
        MovingPanel.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            ShowMessage("Wait dont jump off the edge. The platform is coming");
        }
    }

    private void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        MovingPanel.gameObject.SetActive(true);
        Invoke("HideMessage", displayDuration); // Hide the message after a delay
    }

    private void HideMessage()
    {
        messageText.gameObject.SetActive(false);
        MovingPanel.gameObject.SetActive(false);
    }
}
