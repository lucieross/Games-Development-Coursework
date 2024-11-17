using System;
using UnityEngine;

public class BlueEnemy : MonoBehaviour {
    private Rigidbody2D rg;
    public float speed = 10.0F;
    public float jumpspeed = 10.0F;
    private bool isGrounded;
    public Transform groundCheck; 
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer; 

    // Movement direction variables
    private float moveDirection = 1f; 
    public float changeDirectionTime = 5f; 
    private float nextDirectionChangeTime;

    // Jump cooldown variables
    private bool canJump = true; // tracks if enemy can jump
    public float jumpCooldown = 1f; 
    private float jumpCooldownTimer;

    // Player reset variables
    public GameObject player; 
    private Vector3 startingPosition;
    public GameObject enemyParticleSystem; //particles

    private int jumpCounter = 0; // Tracks how many times the player has jumped on the enemy

    // Launch back variables
    public float launchBackForce = 5.0f; 
    void Start() {
        rg = GetComponent<Rigidbody2D>();
        nextDirectionChangeTime = Time.time + changeDirectionTime;

        if (player != null) {
            startingPosition = player.transform.position;
        }
    }

    void Update() {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        rg.velocity = new Vector2(moveDirection * speed, rg.velocity.y);

        // Change direction after a set time
        if (Time.time >= nextDirectionChangeTime) {
            moveDirection *= -1; 
            nextDirectionChangeTime = Time.time + changeDirectionTime; 
        }

        // Make the enemy jump when grounded
        if (isGrounded && canJump) {
            rg.velocity = new Vector2(rg.velocity.x, jumpspeed);
            canJump = false; 
            jumpCooldownTimer = Time.time + jumpCooldown; 
        }

        // Reset canJump after a cooldown 
        if (Time.time >= jumpCooldownTimer) {
            canJump = true; 
        }
    }

    // Handles collision with the player
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Get the player's Rigidbody2D

            // Check if the collision is from above
            if (collision.contacts.Length > 0 && collision.contacts[0].point.y > transform.position.y) {
                jumpCounter++; 
                Debug.Log($"Jump Counter: {jumpCounter}");

                // Launch the player back 
                if (jumpCounter == 1 && playerRb != null) {
                    Debug.Log("First jump on enemy detected.");
                    AudioManager.instance.Play("SlimeHurt");
                    Vector2 launchDirection = new Vector2(-2f, 0.5f); 
                    playerRb.AddForce(launchDirection.normalized * launchBackForce, ForceMode2D.Impulse);
                }

                // Check if the player has jumped on the enemy twice
                if (jumpCounter >= 2) {
                    Vector2 launchDirection = new Vector2(-2f, 0.5f);
                    playerRb.AddForce(launchDirection.normalized * launchBackForce, ForceMode2D.Impulse);
                    Instantiate(enemyParticleSystem, transform.position, Quaternion.identity); // particles
                    AudioManager.instance.Play("EnemyPop");
                    Destroy(gameObject); // Destroy the enemy after two jumps
                }
            } else {
                // Reset player position
                collision.transform.position = startingPosition;
                HeartManager.health--;
                if (HeartManager.health <= 0) {
                    PlayerManager.isGameOver = true;
                    player.gameObject.SetActive(false);
                }

                // Reset all falling platforms
                ResetAllPlatforms();
            }
        }
    }

    private void ResetAllPlatforms() {
        FallingPlatform[] platforms = FindObjectsOfType<FallingPlatform>();
        foreach (FallingPlatform platform in platforms) {
            platform.ResetPlatform();
        }
    }
}
