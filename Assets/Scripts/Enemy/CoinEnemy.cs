using System;
using UnityEngine;

public class CoinEnemy : MonoBehaviour {
    private Rigidbody2D rg;
    public float speed = 10.0F;
    public float jumpspeed = 10.0F;
    private bool isGrounded;
    public Transform groundCheck; // reference point for ground check
    public float groundCheckRadius = 0.2f; // Radius for ground check
    public LayerMask groundLayer; 

    // Movement direction variables
    private float moveDirection = 1f; 
    public float changeDirectionTime = 5f;
    private float nextDirectionChangeTime;

    // Jump cooldown variables
    private bool canJump = true; 
    public float jumpCooldown = 1f; 
    private float jumpCooldownTimer;

    // Player reset variables
    public GameObject player; 
    private Vector3 startingPosition;
    public GameObject enemyParticleSystem; //particles
    public GameObject coinPrefab; // Reference to the coin 

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

        // Change direction after a set time 5 seoncds 
        if (Time.time >= nextDirectionChangeTime) {
            moveDirection *= -1; 
            nextDirectionChangeTime = Time.time + changeDirectionTime; 
        }

        // Make the enemy jump when thet are grounded
        if (isGrounded && canJump) {
            rg.velocity = new Vector2(rg.velocity.x, jumpspeed);
            canJump = false; 
            jumpCooldownTimer = Time.time + jumpCooldown; // Set cooldown timer
        }

        // Reset canJump after cooldown time
        if (Time.time >= jumpCooldownTimer) {
            canJump = true; 
        }
    }

    // Handles collision with the player
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) { 
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>(); 

            // Check if the collision is from above
            if (collision.contacts.Length > 0 && collision.contacts[0].point.y > transform.position.y) {
                Instantiate(enemyParticleSystem, transform.position, Quaternion.identity); // Particles
                AudioManager.instance.Play("EnemyPop");
                
                // throw the player back
                if (playerRb != null) {
                    Vector2 launchDirection = new Vector2(-5f, 7f);
                    playerRb.AddForce(launchDirection, ForceMode2D.Impulse); // Applys force
                }

                DropCoin(); // Drop the coin
                Destroy(gameObject); // Destroy the enemy
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

    private void DropCoin() {
        if (coinPrefab != null) {
            Instantiate(coinPrefab, transform.position, Quaternion.identity); // Drop the coin at the enemy's position
        }
    }

    private void ResetAllPlatforms() {
        FallingPlatform[] platforms = FindObjectsOfType<FallingPlatform>();
        foreach (FallingPlatform platform in platforms) {
            platform.ResetPlatform();
        }
    }
}
