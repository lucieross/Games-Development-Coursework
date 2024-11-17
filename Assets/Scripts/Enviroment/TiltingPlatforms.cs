using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TiltingPlatform : MonoBehaviour
{
    public float tiltAngle = 15f; // Maximum tilt of platform angle
    public float tiltSpeed = 2f;  // Speed of tilting platform
    public float pushForce = 2f;  // push player
    private bool isPlayerOnPlatform = false; 

    private Quaternion initialRotation; // Original postion of platform
    private Quaternion targetRotation;  
    private Rigidbody2D rb;

    void Start()
    {
        // Stores initial rotation
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (isPlayerOnPlatform)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) 
            {
                Vector3 playerPosition = player.transform.position;
                Vector3 platformPosition = transform.position;

                float direction = playerPosition.x - platformPosition.x;

                // Set target rotation based on the player's position relative to the platform
                if (direction > 0)
                {
                    targetRotation = Quaternion.Euler(0, 0, -tiltAngle); // Tilt to the right
                    ApplyPushForce(player, Vector2.right);
                }
                else 
                {
                    targetRotation = Quaternion.Euler(0, 0, tiltAngle); // Tilt to the left
                    ApplyPushForce(player, Vector2.left);
                }
            }
        }
        else
        {
            // if player is not on the platform, reset it
            targetRotation = initialRotation;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, tiltSpeed * Time.deltaTime);
    }

    private void ApplyPushForce(GameObject player, Vector2 direction)
    {

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.AddForce(direction * pushForce, ForceMode2D.Impulse);
        }
    }

    // detects when the player enters the platform using the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
        }
    }

    // detects when the player leaves
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }
}
