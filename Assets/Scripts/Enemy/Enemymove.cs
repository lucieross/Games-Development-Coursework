using System;
using UnityEngine;

public class Enemymove : MonoBehaviour {
    private Rigidbody2D rg;
    public float speed = 10.0F;
    public float jumpspeed = 10.0F;
    private bool isGrounded;
    public Transform groundCheck; // reference for the ground check
    public float groundCheckRadius = 0.2f; // radius for the ground check
    public LayerMask groundLayer; 

    // Movement direction variables
    private float moveDirection = 1f; 
    public float changeDirectionTime = 5f; // Time before changing direction
    private float nextDirectionChangeTime;

    // Jump cooldown variables
    private bool canJump = true; // tracks if enemy can jump
    public float jumpCooldown = 1f; // Time to wait before jumping 
    private float jumpCooldownTimer;

    // Player reset variables
    public GameObject player; 
    private Vector3 startingPosition;

    public GameObject enemyParticleSystem;   // Reference to the particle system

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

        // Make the enemy jump when on the ground
        if (isGrounded && canJump) {
            rg.velocity = new Vector2(rg.velocity.x, jumpspeed);
            canJump = false; 
            jumpCooldownTimer = Time.time + jumpCooldown; 
        }

        if (Time.time >= jumpCooldownTimer) {
            canJump = true; // Re-enable jumping
        }
    }

    // Handles collisions with the player
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) { 
            // Check if the collision is from above
            if (collision.contacts[0].point.y > transform.position.y) {
                Instantiate(enemyParticleSystem, transform.position, Quaternion.identity); //particles
                AudioManager.instance.Play("EnemyPop"); //plays sound
                Destroy(gameObject); // Destroy the enemy
            } else {
                // Reset player position
                collision.transform.position = startingPosition;
                HeartManager.health--;
                if(HeartManager.health <= 0){
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

     
