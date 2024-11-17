using System;
using UnityEngine;

public class BlueCoinEnemy : MonoBehaviour {
    private Rigidbody2D rg;
    public float speed = 10.0F;
    public float jumpspeed = 10.0F;
    private bool isGrounded;
    public Transform groundCheck; //reference point for ground check
    public float groundCheckRadius = 0.2f; // radius for ground check
    public LayerMask groundLayer; // Layer for ground objects

    // Movement direction 
    private float moveDirection = 1f;
    public float changeDirectionTime = 5f; // Time before changing direction
    private float nextDirectionChangeTime;

    // Jump cooldown 
    private bool canJump = true; // tracks if the enemy can jump
    public float jumpCooldown = 1f; // time to wait before jumping again
    private float jumpCooldownTimer;

    // player reset variables
    public GameObject player; 
    private Vector3 startingPosition;

    public GameObject enemyParticleSystem; //emey particle system

    // Counter to count amount of hits to emeny
    private int jumpCounter = 0; // Tracks times the player has jumped on the enemy
    public float launchBackForce = 5.0f; // Force applied to the player when hit

    public GameObject coinPrefab; // reference to the coin prefab

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
            moveDirection *= -1; // Reverses direction
            nextDirectionChangeTime = Time.time + changeDirectionTime; // Resets the timer
        }

        // Make the enemy jump as soon as it hits the ground
        if (isGrounded && canJump) {
            rg.velocity = new Vector2(rg.velocity.x, jumpspeed);
            canJump = false; // Disable enemy jumping
            jumpCooldownTimer = Time.time + jumpCooldown; 
        }

        if (Time.time >= jumpCooldownTimer) {
            canJump = true; // Re-enable jumping
        }
    }

    // Handles collision with the player
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Get the player's Rigidbody2D

            // Check if the collision is from above
            if (collision.contacts[0].point.y > transform.position.y) {
                jumpCounter++; //  jump counter + 1

                // Launch the player back to the left on the first hit
                if (jumpCounter == 1 && playerRb != null) {
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
                    DropCoin(); // Call the method to drop a coin
                    Destroy(gameObject); // Destroy the enemy after two jumps
                }
            } else {
                // Reset player position to spawn
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

    private void DropCoin() { //method to drop a coin when dies
        if (coinPrefab != null) {
            Instantiate(coinPrefab, transform.position, Quaternion.identity); // Drop the coin at the enemy's position
        }
    }

    private void ResetAllPlatforms() { //resets all the platforms if the player dies so they can still complete the level
        FallingPlatform[] platforms = FindObjectsOfType<FallingPlatform>();
        foreach (FallingPlatform platform in platforms) {
            platform.ResetPlatform();
        }
    }
}
