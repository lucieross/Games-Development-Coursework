using UnityEngine;

public class CombinedEnemy : MonoBehaviour
{
    public GameObject player;
    public float speed = 10.0f;
    public float jumpspeed = 10.0f;
    public float distanceBetween = 5.0f; // Distance until enemy start chasing
    public GameObject enemyParticleSystem; // particle system

    public BoxCollider2D boundaryCollider; // Reference to the boundary

    private Rigidbody2D rg;
    private int jumpCounter = 0; // Track how many times the player has jumped on enemy
    private CharacterController2D playerController; 

    // Movement direction variables
    private float moveDirection = 1f; 
    public float changeDirectionTime = 5f; 
    private float nextDirectionChangeTime;

    // Jumping variables
    private bool canJump = true; 
    public float jumpCooldown = 1f; 
    private float jumpCooldownTimer;

    // New variables for harder enemy after hit 1
    private bool isharder = false; 
    public float harderSpeedMultiplier = 1.5f; 
    public float harderJumpMultiplier = 1.5f;


    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        playerController = player.GetComponent<CharacterController2D>(); 
        nextDirectionChangeTime = Time.time + changeDirectionTime;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < distanceBetween && !playerController.IsHidden())
        {
            // Moves towards the player
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rg.velocity = new Vector2(direction.x * speed * (isharder ? harderSpeedMultiplier : 1), rg.velocity.y);

            // Jumping logic while chasing
            HandleJumping();
        }
        else
        {
            // Random movement logic
            rg.velocity = new Vector2(moveDirection * speed * (isharder ? harderSpeedMultiplier : 1), rg.velocity.y);

            // Check to make sure its not going off the edge
            if (IsAtBoundary())
            {
                moveDirection *= -1; 
            }

            // Change direction after a set time
            if (Time.time >= nextDirectionChangeTime)
            {
                moveDirection *= -1; 
                nextDirectionChangeTime = Time.time + changeDirectionTime; 
            }

            HandleJumping();
        }
    }

    private void HandleJumping()
    {
        if (canJump)
        {
            rg.velocity = new Vector2(rg.velocity.x, jumpspeed * (isharder ? harderJumpMultiplier : 1));
            canJump = false; // disables jumping
            jumpCooldownTimer = Time.time + jumpCooldown; // Sets cooldown 
        }

        // Reset canJump after cooldown
        if (Time.time >= jumpCooldownTimer)
        {
            canJump = true; 
        }
    }

    private bool IsAtBoundary()
    {
        if (boundaryCollider != null)
        {
            //Makes sure that the enemy isnt going off the edge
            Bounds bounds = boundaryCollider.bounds;

            if ((moveDirection > 0 && transform.position.x >= bounds.max.x) ||
                (moveDirection < 0 && transform.position.x <= bounds.min.x))
            {
                return true; 
            }
        }

        // a raycast detects if there is ground in the moving direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * moveDirection, 0.1f);
        return hit.collider == null; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Get the player's Rigidbody2D

            // Check if the collision is from above 
            if (collision.contacts.Length > 0 && collision.contacts[0].point.y > transform.position.y)
            {
                jumpCounter++; 

                // Launch the player back
                if (playerRb != null)
                {
                    Vector2 launchDirection = new Vector2(-20f, 20.0f); 
                    if (jumpCounter == 1)
                    {
                        AudioManager.instance.Play("SlimeHurt");
                    }
                    playerRb.AddForce(launchDirection.normalized * 15.0f, ForceMode2D.Impulse); // Increase from 5.0f to 10.0f
                }

                // Make enemy harder after first hit
                if (jumpCounter == 1)
                {
                    isharder = true; // Make the enemy harder
                    jumpCooldown = 1.25f; 
                }

                if (jumpCounter == 1 && rg != null) //hits enemy up slightly after first hit
                {
                    Vector2 flingForce = new Vector2(0f, 5f); 
                    rg.AddForce(flingForce, ForceMode2D.Impulse);
                }

                // Destroy the enemy after two jumps
                if (jumpCounter >= 2)
                {
                    Instantiate(enemyParticleSystem, transform.position, Quaternion.identity); // Particles
                    AudioManager.instance.Play("EnemyPop");
                    Destroy(gameObject); // Destroys the enemy after two jumps
                }
            }
            else
            {
                playerController.transform.position = playerController.startingPosition; // go  to starting position
                playerRb.velocity = Vector2.zero; 

                HeartManager.health--;
                if (HeartManager.health <= 0)
                {
                    PlayerManager.isGameOver = true;
                    player.gameObject.SetActive(false);
                }

                // Reset all falling platforms
                ResetAllPlatforms();
            }
        }
    }

    private void ResetAllPlatforms()
    {
        FallingPlatform[] platforms = FindObjectsOfType<FallingPlatform>();
        foreach (FallingPlatform platform in platforms)
        {
            platform.ResetPlatform();
        }
    }
}
